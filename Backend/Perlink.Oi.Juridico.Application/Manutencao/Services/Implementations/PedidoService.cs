using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations {

    internal class PedidoService : IPedidoService {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IPedidoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public PedidoService(IDatabaseContext context, ILogger<IPedidoService> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = context;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        #region Trabalhista

        public CommandResult CriarDoTrabalhista(CriarPedidoDoTrabalhistaCommand command) {
            string entityName = "Pedido do trabalhista";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_TRABALHISTA)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_TRABALHISTA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                RiscoPerda riscoPerda = RiscoPerda.PorId(command.RiscoPerdaId);

                ProprioTerceiro proprioTerceiro = string.IsNullOrEmpty(command.ProprioTerceiroId) ? ProprioTerceiro.PorId(null) :  ProprioTerceiro.PorId(command.ProprioTerceiroId);

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Pedido pedido = Pedido.CriarTrabalhista(DataString.FromString(command.Descricao), riscoPerda, command.Ativo, command.ProvavelZero, proprioTerceiro);

                if (pedido.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(pedido.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(pedido);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult AtualizarDoTrabalhista(AtualizarPedidoDoTrabalhistaCommand command) {
            string entityName = "Pedido do trabalhista";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_TRABALHISTA)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_TRABALHISTA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.PedidoId));
                Pedido pedido = DatabaseContext.Pedidos.FirstOrDefault(a => a.Id == command.PedidoId);

                if (pedido is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.PedidoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.PedidoId));
                }

                RiscoPerda riscoPerda = RiscoPerda.PorId(command.RiscoPerdaId);

                ProprioTerceiro proprioTerceiro = string.IsNullOrEmpty(command.ProprioTerceiroId) ? ProprioTerceiro.PorId(null) : ProprioTerceiro.PorId(command.ProprioTerceiroId);

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.PedidoId));
                pedido.AtualizarPedidoDoTrabalhista(DataString.FromString(command.Descricao), riscoPerda, command.Ativo, command.ProvavelZero, proprioTerceiro);

                if (pedido.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(pedido.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult RemoverDoTrabalhista(int id) {
            string entityName = "Pedido do Trabalhista";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_TRABALHISTA)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_TRABALHISTA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));
                Pedido pedido = DatabaseContext.Pedidos.FirstOrDefault(x => x.Id == id && x.EhTrabalhista);

                if (pedido is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                ValidarExclusao(pedido);

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));
                DatabaseContext.Remove(pedido);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        #endregion Trabalhista

        #region Cível Estratégico

        public CommandResult CriarDoCivelEstrategico(CriarPedidoDoCivelEstrategicoCommand command) {
            string entityName = "Pedido do Civel Estratégico";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_CIVEL_ESTRATEGICO)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Pedido pedido = Pedido.CriarCivelEstrategico(DataString.FromString(command.Descricao), command.Ativo);

                if (pedido.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(pedido.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(pedido);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                if (command.IdConsumidor.HasValue)
                {
                    MigracaoPedido pedidoMigracao = MigracaoPedido.CriarMigracaoPedido(pedido.Id, command.IdConsumidor.Value);

                    if (pedidoMigracao.HasNotifications)
                    {
                        Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                        return CommandResult.Invalid(pedidoMigracao.Notifications.ToNotificationsString());
                    }
                    DatabaseContext.Add(pedidoMigracao);
                }

                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult AtualizarDoCivelEstrategico(AtualizarPedidoDoCivelEstrategicoCommand command) {
            string entityName = "Pedido do cível estratégico";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_CIVEL_ESTRATEGICO)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.PedidoId));
                Pedido pedido = DatabaseContext.Pedidos.FirstOrDefault(a => a.Id == command.PedidoId);

                if (pedido is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.PedidoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.PedidoId));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.PedidoId));
                pedido.AtualizarPedidoDoCivelEstrategico(DataString.FromString(command.Descricao), command.Ativo);

                if (pedido.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(pedido.Notifications.ToNotificationsString());
                }

                MigracaoPedido pedidoMigracao = DatabaseContext.MigracaoPedido.FirstOrDefault(x => x.CodPedidoCivelEstrat == pedido.Id);

                if (pedidoMigracao != null)
                {
                    DatabaseContext.Remove(pedidoMigracao);
                }

                if (command.IdConsumidor.HasValue)
                {
                    MigracaoPedido migracao = MigracaoPedido.CriarMigracaoPedido(pedido.Id, command.IdConsumidor.Value);

                    if (migracao.HasNotifications)
                    {
                        Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                        return CommandResult.Invalid(migracao.Notifications.ToNotificationsString());
                    }
                    DatabaseContext.Add(migracao);
                }               

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult RemoverDoCivelEstrategico(int id) {
            string entityName = "Pedido do cível estratégico";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_CIVEL_ESTRATEGICO)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));
                Pedido pedido = DatabaseContext.Pedidos.Single(x => x.Id == id && x.EhCivelEstrategico);

                if (pedido is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                ValidarExclusao(pedido);

                MigracaoPedido migracao = DatabaseContext.MigracaoPedido.FirstOrDefault(x => x.CodPedidoCivelEstrat == pedido.Id);

                if (migracao != null)
                {
                    DatabaseContext.Remove(migracao);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));
                DatabaseContext.Remove(pedido);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();                               

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        #endregion Cível Estratégico

        private void ValidarExclusao(Pedido pedido) {
            if (DatabaseContext.Processos.Any(p => p.PedidosDoProcesso.Any(x => x.Pedido == pedido))) {
                Logger.LogInformation($"Pedido '{ pedido.Id }' vinculado a 'Processos'");
                throw new InvalidOperationException("O pedido selecionado se encontra relacionado a um processo.");
            }

            if (DatabaseContext.AndamentosPedidos.Any(x => x.Pedido == pedido)) {
                Logger.LogInformation($"Pedido '{ pedido.Id }' vinculado a 'Processos'");
                throw new InvalidOperationException("O pedido selecionado se encontra relacionado a um processo.");
            }
        }

        #region Cível Consumidor

        public CommandResult CriarDoCivelConsumidor(CriarPedidoDoCivelConsumidorCommand command)
        {
            string entityName = "Pedido do Civel Consumidor";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_CIVEL_CONSUMIDOR))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Pedido pedido = Pedido.CriarCivelConsumidor(DataString.FromString(command.Descricao), command.Audiencia, command.Ativo);

                if (pedido.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(pedido.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(pedido);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                if (command.IdEstrategico.HasValue)
                {
                    MigracaoPedidoConsumidor pedidoMigracaoConsumidor = MigracaoPedidoConsumidor.CriarMigracaoPedidoConsumidor(pedido.Id, command.IdEstrategico.Value);

                    if (pedidoMigracaoConsumidor.HasNotifications)
                    {
                        Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                        return CommandResult.Invalid(pedidoMigracaoConsumidor.Notifications.ToNotificationsString());
                    }
                    DatabaseContext.Add(pedidoMigracaoConsumidor);
                }

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

        public CommandResult AtualizarDoCivelConsumidor(AtualizarPedidoDoCivelConsumidorCommand command)
        {
            string entityName = "Pedido do cível consumidor";
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

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_CIVEL_CONSUMIDOR))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.PedidoId));
                Pedido pedido = DatabaseContext.Pedidos.FirstOrDefault(a => a.Id == command.PedidoId);

                if (pedido is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.PedidoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.PedidoId));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.PedidoId));
                pedido.AtualizarPedidoDoCivelConsumidor(DataString.FromString(command.Descricao), command.Ativo, command.Audiencia);

                if (pedido.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(pedido.Notifications.ToNotificationsString());
                }

                MigracaoPedidoConsumidor pedidoMigracaoConsumidor = DatabaseContext.MigracaoPedidoConsumidor.FirstOrDefault(x => x.CodPedidoCivel == pedido.Id);

                if (pedidoMigracaoConsumidor != null)
                {
                    DatabaseContext.Remove(pedidoMigracaoConsumidor);
                }

                if (command.IdEstrategico.HasValue)
                {
                    MigracaoPedidoConsumidor migracao = MigracaoPedidoConsumidor.CriarMigracaoPedidoConsumidor(pedido.Id, command.IdEstrategico.Value);

                    if (migracao.HasNotifications)
                    {
                        Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                        return CommandResult.Invalid(migracao.Notifications.ToNotificationsString());
                    }
                    DatabaseContext.Add(migracao);
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

        public CommandResult RemoverDoCivelConsumidor(int id)
        {
            string entityName = "Pedido do cível consumidor";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_CIVEL_CONSUMIDOR))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));
                Pedido pedido = DatabaseContext.Pedidos.Single(x => x.Id == id && x.EhCivel);

                if (pedido is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                ValidarExclusao(pedido);

                MigracaoPedidoConsumidor migracao = DatabaseContext.MigracaoPedidoConsumidor.FirstOrDefault(x => x.CodPedidoCivel == pedido.Id);

                if (migracao != null)
                {
                    DatabaseContext.Remove(migracao);
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

        #endregion Cível Consumidor

    }
}