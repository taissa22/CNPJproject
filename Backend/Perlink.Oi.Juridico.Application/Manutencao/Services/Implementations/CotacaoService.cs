using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class CotacaoService : ICotacaoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ICotacaoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public CotacaoService(IDatabaseContext databaseContext, ILogger<ICotacaoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarCotacaoCommand command)
        {
            string entityName = "Tipo de Pendência";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COTACAO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COTACAO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                var indice = DatabaseContext.Indices.FirstOrDefault(x => x.Id == command.CodigoIndice);

                if (DatabaseContext.Cotacoes.Any(x => x.Id.Equals(command.CodigoIndice) && x.DataCotacao.Date.Equals(command.DataCotacao.Date)))
                {
                    string mensagem = "Já existe uma cotação para o mês/ano deste índice.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));

                decimal? ultimaValorCotacao = null;

                //CÁLCULO PARA PERCENTUAL ACUMULADO
                if (indice.Acumulado.GetValueOrDefault())
                {
                    var cotacaoAnterior = DatabaseContext.Cotacoes.Where(c => c.DataCotacao < command.DataCotacao && c.Id == command.CodigoIndice).OrderByDescending(c => c.DataCotacao)
                 .FirstOrDefault();

                    if (indice.AcumuladoAutomatico == true)
                        ultimaValorCotacao = Math.Round(((1 + (command.Valor / 100)) * (cotacaoAnterior != null ? cotacaoAnterior.ValorAcumulado != null ? Math.Round(cotacaoAnterior.ValorAcumulado.GetValueOrDefault(), 8) : 1 : 1)), 8);

                    else ultimaValorCotacao = command.Valor;
                }

                //CRIA NOVA COTAÇÃO
                Cotacao cotacao = Cotacao.Criar(indice, command.DataCotacao, command.Valor, ultimaValorCotacao);

                //ATUALIZA COTAÇÕES FUTURAS POR PERCENTUAL SE FLAG " AcumuladoAutomatico " FOR TRUE
                if (indice.Acumulado.GetValueOrDefault() && indice.CodigoValorIndice.Equals("P") && indice.AcumuladoAutomatico == true)
                {
                    var cotacoesFuturas = DatabaseContext.Cotacoes.Where(c => c.DataCotacao > command.DataCotacao && c.Id == command.CodigoIndice)
                        .OrderBy(c => c.DataCotacao);

                    decimal? valorAcumuladoAuxiliar = ultimaValorCotacao;

                    if (cotacoesFuturas != null)
                    {
                        foreach (var cota in cotacoesFuturas)
                        {
                            cota.Atualizar(cota.Valor, Math.Round(((1 + (cota.Valor / 100)) * (valorAcumuladoAuxiliar > 0 ? Math.Round(valorAcumuladoAuxiliar.GetValueOrDefault(), 8) : 1)), 8));//VALOR ACUMULADO FUTURO

                            valorAcumuladoAuxiliar = cota.ValorAcumulado.GetValueOrDefault();
                        }
                    }
                }

                if (cotacao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(cotacao.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(cotacao);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult Atualizar(AtualizarCotacaoCommand command)
        {
            string entityName = "Tipo de Pendência";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COTACAO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COTACAO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{command.CodigoIndice}, {command.DataCotacao}"));
                Cotacao cotacao = DatabaseContext.Cotacoes.FirstOrDefault(x => x.Id == command.CodigoIndice && x.DataCotacao.Date == command.DataCotacao.Date);

                if (cotacao is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.CodigoIndice}, {command.DataCotacao}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.CodigoIndice}, {command.DataCotacao}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, $"{command.CodigoIndice}, {command.DataCotacao}"));

                var indice = DatabaseContext.Indices.FirstOrDefault(x => x.Id == command.CodigoIndice);

                if (!indice.CodigoValorIndice.Equals("P") || !indice.Acumulado.GetValueOrDefault())
                {
                    cotacao.Atualizar(command.Valor, null);
                }

                //CÁLCULO PARA FATOR PERCENTUAL
                if (indice.Acumulado.GetValueOrDefault())
                {
                    //CÁLCULO PARA FATOR ACUMULADO
                    var cotacaoCorrente = DatabaseContext.Cotacoes.Single(c => c.DataCotacao == command.DataCotacao && c.Id == command.CodigoIndice);
                    var cotacaoAnterior = DatabaseContext.Cotacoes.Where(c => c.DataCotacao < cotacaoCorrente.DataCotacao && c.Id == command.CodigoIndice).OrderByDescending(c => c.DataCotacao)
                        .FirstOrDefault();

                    if (indice.AcumuladoAutomatico == true)
                        cotacaoCorrente.Atualizar(command.Valor, Math.Round(((1 + (command.Valor / 100)) * (cotacaoAnterior != null ? cotacaoAnterior.ValorAcumulado != null ? Math.Round(cotacaoAnterior.ValorAcumulado.GetValueOrDefault(), 8) : 1 : 1)), 8));

                    else
                        cotacaoCorrente.Atualizar(command.Valor, command.Valor);

                    //ATUALIZA COTAÇÕES FUTURAS POR PERCENTUAL
                    var cotacoesFuturas = DatabaseContext.Cotacoes.Where(c => c.DataCotacao > cotacaoCorrente.DataCotacao && c.Id == command.CodigoIndice)
                        .OrderBy(c => c.DataCotacao);

                    decimal valorAcumuladoAuxiliar = cotacaoCorrente.ValorAcumulado.GetValueOrDefault();

                    if (cotacoesFuturas != null && indice.AcumuladoAutomatico == true)
                    {
                        foreach (var cota in cotacoesFuturas)
                        {
                            cota.Atualizar(cota.Valor, Math.Round(((1 + (cota.Valor / 100)) * (valorAcumuladoAuxiliar > 0 ? Math.Round(valorAcumuladoAuxiliar, 8) : 1)), 8));//VALOR ACUMULADO FUTURO

                            valorAcumuladoAuxiliar = cota.ValorAcumulado.GetValueOrDefault();
                        }
                    }
                }

                if (cotacao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{command.CodigoIndice}, {command.DataCotacao}"));
                    return CommandResult.Invalid(cotacao.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{command.CodigoIndice}, {command.DataCotacao}"));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult Remover(int codigoIndice, DateTime dataCotacao)
        {
            string entityName = "Tipo de Pendência";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COTACAO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COTACAO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{codigoIndice}, {dataCotacao}"));
                Cotacao cotacao = DatabaseContext.Cotacoes.FirstOrDefault(x => x.Id == codigoIndice && x.DataCotacao == dataCotacao);

                if (cotacao is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{codigoIndice}, {dataCotacao}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{codigoIndice}, {dataCotacao}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{codigoIndice}, {dataCotacao}"));
                DatabaseContext.Remove(cotacao);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{codigoIndice}, {dataCotacao}"));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        
    }
}
