using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class ObjetoService : IObjetoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IObjetoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public ObjetoService(IDatabaseContext databaseContext, ILogger<IObjetoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult Criar(CriarObjetoCommand command)
        {
            string entityName = "Objeto";
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

                if (DatabaseContext.Pedidos.Any(x => (x.Descricao == command.Descricao) && command.EhTrabalhistaAdministrativo && (x.EhTrabalhistaAdministrativo == command.EhTrabalhistaAdministrativo) ))
                {
                    string message = "Já existe outro Objeto Trabalhista com este nome.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                if (DatabaseContext.Pedidos.Any(x => (x.Descricao == command.Descricao) && (x.EhTributarioAdministrativo == command.EhTributarioAdminstrativo || x.EhTributarioJudicial == command.EhTributarioJudicial) ))
                {
                    string message = "Já existe outro Objeto Tributário com este nome.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));

                Pedido pedido;

                if (command.EhTrabalhistaAdministrativo)
                  pedido = Pedido.CriarTrabalhistaAdministrativo(command.Descricao);
                else
                  pedido = Pedido.CriarPedidoDoTributario(command.Descricao, command.AtivoTributarioJudicial, command.AtivoTributarioAdminstrativo,
                                                                             command.EhTributarioJudicial, command.EhTributarioAdminstrativo, command.GrupoId);

                if (pedido.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(pedido.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(pedido);
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

        public CommandResult Atualizar(AtualizarObjetoCommand command)
        {
            string entityName = "Objeto";
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

                Pedido pedido = null;
                pedido = DatabaseContext.Pedidos.FirstOrDefault(x => x.Id == command.Id);            

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));

                if (pedido.EhTributarioJudicial && 
                    !command.EhTributarioJudicial && 
                    DatabaseContext.Processos.Any(x => x.PedidosDoProcesso.Any(p => p.Pedido.Id == command.Id) 
                                                                                    && x.TipoProcessoId == TipoProcesso.TRIBUTARIO_JUDICIAL.Id))
                {
                    string message = "Objeto Tributário Judicial não pode ser desmarcado, pois está sendo utilizado em Processo.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                if (pedido.EhTributarioAdministrativo &&
                    !command.EhTributarioAdminstrativo &&
                    DatabaseContext.Processos.Any(x => x.PedidosDoProcesso.Any(p => p.Pedido.Id == command.Id)
                                                                                    && x.TipoProcessoId == TipoProcesso.TRIBUTARIO_ADMINISTRATIVO.Id))
                {
                    string message = "Objeto Tributário Administrativo não pode ser desmarcado, pois está sendo utilizado em Processo.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                if (command.EhTrabalhistaAdministrativo)
                    pedido.AtualizarPedidoDoTrabalhistaAdministrativo(command.Descricao);
                else
                    pedido.AtualizarPedidoDoTributario(command.Descricao, command.AtivoTributarioJudicial, command.AtivoTributarioAdminstrativo,
                                                                              command.EhTributarioJudicial, command.EhTributarioAdminstrativo, command.GrupoId);

                if (pedido.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(pedido.Notifications.ToNotificationsString());
                }

                
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

        public CommandResult Remover(int id)
        {
            string entityName = "Objeto";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));
                Pedido pedido = DatabaseContext.Pedidos.FirstOrDefault(x => x.Id == id);

                if (pedido is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }

                if (DatabaseContext.PedidosDoProcesso.Any(x => x.Pedido.Id == id))
                {
                    string mensagem = "Não será possível excluir o Objeto selecionado, pois se encontra relacionado com Pedidos do Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.AndamentosPedidos.Any(x => x.Pedido.Id == id))
                {
                    string mensagem = "Não será possível excluir o Objeto selecionado, pois se encontra relacionado com Andamentos de Pedido.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.DecisaoObjetoProcessos.Any(x => x.PedidoId == id))
                {
                    string mensagem = "Não será possível excluir o Objeto selecionado, pois se encontra relacionado com Decisao de Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.ValorObjetoProcessos.Any(x => x.PedidoId == id))
                {
                    string mensagem = "Não será possível excluir o Objeto selecionado, pois se encontra relacionado com Valor de Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));
                DatabaseContext.Remove(pedido);

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

    }
}
