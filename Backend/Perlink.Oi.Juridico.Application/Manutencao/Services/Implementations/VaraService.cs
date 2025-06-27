using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class VaraService : IVaraService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IVaraService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public VaraService(IDatabaseContext databaseContext, ILogger<IVaraService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarVaraCommand command)
        {
            string entityName = "Vara";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMARCA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COMARCA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (DatabaseContext.Varas.Any(x => x.VaraId == command.VaraId && x.ComarcaId == command.ComarcaId && x.TipoVaraId == command.TipoVaraId))
                {
                    string mensagem = "Não pode ter o mesmo número e o mesmo tipo de vara para a comarca.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Vara vara = Vara.Criar(command.VaraId.Value, command.ComarcaId, command.TipoVaraId, command.Endereco, command.OrgaoBBId);

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));

                if (vara.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(vara.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(vara);
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

        public CommandResult Atualizar(AtualizarVaraCommand command)
        {
            string entityName = "Vara";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMARCA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COMARCA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }


                Vara vara = DatabaseContext.Varas.FirstOrDefault(x => x.VaraId == command.VaraId && x.ComarcaId == command.ComarcaId && x.TipoVaraId == command.TipoVaraId);
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.VaraId.Value));

                if (vara is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.VaraId.Value));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.VaraId.Value));
                }

                if ((vara.OrgaoBBId != null) && (vara.OrgaoBB.TribunalBBId != command.TribunalBBId) && verificaLancamentosBB(vara.ComarcaId, vara.VaraId, vara.TipoVaraId))
                {
                    string mensagem = "O campo Tribunal de Justiça BB não pode ser apagado, pois já possui lançamentos do BB.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if ((vara.OrgaoBBId != null) && (vara.OrgaoBBId != command.OrgaoBBId) && verificaLancamentosBB(vara.ComarcaId, vara.VaraId, vara.TipoVaraId))
                {
                    string mensagem = "O campo Vara BB não pode ser apagado, pois já possui lançamentos do BB.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                vara.Atualizar(command.VaraId.Value, command.ComarcaId, command.TipoVaraId, command.Endereco, command.OrgaoBBId);

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, $"{command.VaraId}, {command.ComarcaId}, {command.TipoVaraId}"));

                if (vara.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(vara.Notifications.ToNotificationsString());
                }
                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));


                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }


        public bool verificaLancamentosBB(int comarcaId, int varaId, int tipoVaraId)
        {
            var statusPagamento = new[] { StatusPagamento.PEDIDO_SAP_PAGO.Id, 
                                          StatusPagamento.PEDIDO_SAP_PAGO_MANUALMENTE.Id, 
                                          StatusPagamento.PEDIDO_SAP_RECEBIDO_FISCAL.Id 
                                        };
            var tipoProcesso = new[] { TipoProcesso.CIVEL_CONSUMIDOR.Id, TipoProcesso.JEC.Id };           
            var fornecedorBB = DatabaseContext.ParametrosJuridicos
                                               .Where(x => x.Parametro == "SAP_FORNECEDOR_BB")
                                               .Select(x => x.Conteudo).FirstOrDefault<string>()
                                               .Split('|')
                                               .Select(int.Parse)
                                               .ToArray();
            int formaPagamentoGuia = int.Parse( DatabaseContext.ParametrosJuridicos.Where(x => x.Parametro == "FORMA_PAGAMENTO_GUIA").Select(x => x.Conteudo).FirstOrDefault());


            var count = DatabaseContext.Lancamentos
                .Where(x => statusPagamento.Contains(x.StatusPagamentoId)
                        && fornecedorBB.Contains(x.FornecedorId.GetValueOrDefault())
                        && tipoProcesso.Contains(x.Processo.TipoProcessoId)
                        && x.FormaPagamentoId == formaPagamentoGuia
                        && x.Processo.EmpresaDoGrupo.GeraArquivoBB 
                        && x.Processo.ComarcaId == comarcaId
                        && x.Processo.VaraId == varaId
                        && x.Processo.TipoVaraId == tipoVaraId).Count();


            if (count > 0)
            {
                return true;
            }

            return false;
        }

        public CommandResult Remover(int VaraId, int ComarcaId, int TipoVaraId)
        {
            string entityName = nameof(Vara);
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMARCA))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COMARCA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                if (DatabaseContext.Processos.Any(x => x.VaraId == VaraId && x.ComarcaId == ComarcaId && x.TipoVaraId == TipoVaraId))
                {
                    string mensagem = "Não será possivel excluir a vara selecionada, pois se encontra relacionada com um " + nameof(Processo) + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.GrupoJuizadoVaras.Any(x => x.VaraId == VaraId && x.ComarcaId == ComarcaId && x.TipoVaraId == TipoVaraId))
                {
                    string mensagem = "Não será possivel excluir a vara selecionada, pois se encontra relacionada com um " + nameof(GrupoJuizadoVara) + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.ProcessoCpfCnpjs.Any(x => x.VaraId == VaraId && x.ComarcaId == ComarcaId && x.TipoVaraId == TipoVaraId))
                {
                    string mensagem = "Não será possivel excluir a vara selecionada, pois se encontra relacionada com um " + nameof(ProcessoCpfCnpj) + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.AlocacoesPrepostos.Any(x => x.VaraId == VaraId && x.ComarcaId == ComarcaId && x.TipoVaraId == TipoVaraId))
                {
                    string mensagem = "Não será possivel excluir a vara selecionada, pois se encontra relacionada com um " + nameof(AlocacaoPreposto) + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.Protocolos.Any(x => x.VaraId == VaraId && x.ComarcaId == ComarcaId && x.TipoVaraId == TipoVaraId))
                {
                    string mensagem = "Não será possivel excluir a vara selecionada, pois se encontra relacionada com um " + nameof(Protocolo) + ".";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Vara vara = DatabaseContext.Varas.FirstOrDefault(x => x.VaraId == VaraId && x.ComarcaId == ComarcaId && x.TipoVaraId == TipoVaraId);
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{VaraId}, {ComarcaId}, {TipoVaraId}"));

                if (vara is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{VaraId}, {ComarcaId}, {TipoVaraId}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{VaraId}, {ComarcaId}, {TipoVaraId}"));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));

                DatabaseContext.Remove(vara);
                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{VaraId}, {ComarcaId}, {TipoVaraId}"));

                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

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
