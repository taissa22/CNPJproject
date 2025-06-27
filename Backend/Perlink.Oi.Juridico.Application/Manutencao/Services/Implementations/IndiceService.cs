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
    internal class IndiceService : IIndiceService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IIndiceService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public IndiceService(IDatabaseContext databaseContext, ILogger<IIndiceService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarIndiceCommand command)
        {
            string entityName = "Indice";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_INDICE))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_INDICE, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Indice indice = Indice.Criar(command.Descricao, command.Mensal, command.CodigoValorIndice, command.Acumulado, command.AcumuladoAutomatico);

                if (indice.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(indice.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(indice);
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

        public CommandResult Atualizar(AtualizarIndiceCommand command)
        {
            string entityName = "Indice";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_INDICE))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_INDICE, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (DatabaseContext.Cotacoes.Any(x => x.Indice.Id == command.CodigoIndice && x.Indice.Acumulado == true && x.Valor >= 0 && x.ValorAcumulado != null) && command.CodigoValorIndice.Equals("P") && command.Acumulado.Equals(false))
                {
                    string mensagem = "Não é possível desmarcar o Acumulado, pois o Índice está relacionado com Cotação de Índices com valores acumulados.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{command.CodigoIndice}"));
                Indice indice = DatabaseContext.Indices.FirstOrDefault(x => x.Id == command.CodigoIndice);

                if (indice is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.CodigoIndice}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{command.CodigoIndice}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, $"{command.CodigoIndice}"));
                indice.Atualizar(command.Descricao, command.Mensal, command.CodigoValorIndice, command.Acumulado, command.AcumuladoAutomatico);

                if (indice.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{command.CodigoIndice}"));
                    return CommandResult.Invalid(indice.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{command.CodigoIndice}"));
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

        public CommandResult Remover(int codigoIndice)
        {
            string entityName = "Indice";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_INDICE))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_INDICE, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (DatabaseContext.Cotacoes.Any(x => x.Indice.Id == codigoIndice))
                {
                    string mensagem = "Não é possível excluir o Índice, pois ele está relacionado com Cotação de Índices.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.Cotacoes.Any(x => x.Indice.Id == codigoIndice && x.Indice.Acumulado == true && x.Valor >= 0 &&  x.ValorAcumulado != null))
                {
                    string mensagem = "Não é possível excluir o Índice, pois ele está relacionado com Cotação de Índices com valores acumulados.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{codigoIndice}"));
                Indice indice = DatabaseContext.Indices.FirstOrDefault(x => x.Id == codigoIndice);

                if (indice is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{codigoIndice}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{codigoIndice}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{codigoIndice}"));
                DatabaseContext.Remove(indice);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{codigoIndice}"));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));

                if(ex.InnerException.Message.Contains("FK_INDICE_ESTADO"))
                {
                    string mensagem = "Não é possível excluir o Índice, pois ele está relacionado com a tabela Estado.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (ex.InnerException.Message.Contains("FK_AGEND_FECH_ATM_UF_JEC_COD_I"))
                {
                    string mensagem = "Não é possível excluir o Índice, pois ele está relacionado com a tabela de Fechamento ATM.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (ex.InnerException.Message.Contains("ICESF_IND_FK_01"))
                {
                    string mensagem = "Não é possível excluir o Índice, pois ele está relacionado com tabela Esfera de Correções.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (ex.InnerException.Message.Contains("ICPR_IND_FK"))
                {
                    string mensagem = "Não é possível excluir o Índice, pois ele está relacionado com a tabela de Correções do Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                return CommandResult.Invalid(ex.Message);
            }
        }
    }
}