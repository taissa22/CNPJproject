using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations {

    internal class AssuntoService : IAssuntoService {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAssuntoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public AssuntoService(IDatabaseContext databaseContext, ILogger<IAssuntoService> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        #region Civel Consumidor

        public CommandResult CriarDoCivelConsumidor(CriarAssuntoDoCivelConsumidorCommand command) {
            string entityName = "Assunto do Cível Consumidor";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Assunto assunto = Assunto.CriarDoCivelConsumidor(command.Descricao, command.Ativo,command.Negociacao, command.Proposta, command.CodigoTipoContingencia);
     

                if (assunto.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(assunto.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(assunto);

                if (command.IdEstrategico.HasValue)
                {
                    MigracaoAssunto assuntoMigracao = MigracaoAssunto.CriarMigracaoAssuntoConsumidor(assunto.Id, command.IdEstrategico.Value);
                    DatabaseContext.Add(assuntoMigracao);
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

        public CommandResult AtualizarDoCivelConsumidor(AtualizarAssuntoDoCivelConsumidorCommand command) {
            string entityName = "Assunto do Cível Consumidor";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.AssuntoId));
                Assunto assunto = DatabaseContext.Assuntos.FirstOrDefault(x => x.Id == command.AssuntoId && x.EhCivelConsumidor);

                if (assunto is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.AssuntoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.AssuntoId));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.AssuntoId));
                assunto.AtualizarDoCivelConsumidor(command.Descricao,command.Ativo, command.Negociacao, command.Proposta, command.CodigoTipoContingencia);

                MigracaoAssunto assuntoMigracao = DatabaseContext.MigracaoAssunto.FirstOrDefault(x => x.CodAssuntoCivel == assunto.Id);

                if (assuntoMigracao != null)
                {
                    DatabaseContext.Remove(assuntoMigracao);
                }

                if (command.IdEstrategico.HasValue)
                {
                    var migAssunto = MigracaoAssunto.CriarMigracaoAssuntoConsumidor(assunto.Id, command.IdEstrategico.Value);
                    DatabaseContext.Add(migAssunto);
                }        

                if (assunto.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(assunto.Notifications.ToNotificationsString());
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

        public CommandResult RemoverDoCivelConsumidor(int assuntoId) {

            string entityName = "Assunto do Cível Consumidor";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, assuntoId));
                Assunto assunto = DatabaseContext.Assuntos.FirstOrDefault(x => x.Id == assuntoId && x.EhCivelConsumidor);

                if (assunto is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, assuntoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, assuntoId));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                ValidarExclusao(assunto);

                MigracaoAssunto assuntoMigracao = DatabaseContext.MigracaoAssunto.FirstOrDefault(x => x.CodAssuntoCivel == assunto.Id);

                if (assuntoMigracao != null)
                {
                    DatabaseContext.Remove(assuntoMigracao);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, assuntoId));
                DatabaseContext.Remove(assunto);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        #endregion Civel Consumidor

        #region Civel Estrategico

        public CommandResult CriarDoCivelEstrategico(CriarAssuntoDoCivelEstrategicoCommand command) {
            string entityName = "Assunto do Cível Estratégico";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));            

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ASSUNTO_CIVEL_ESTRATEGICO)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ASSUNTO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Assunto assunto = Assunto.CriarDoCivelEstrategico(command.Descricao, command.Ativo);

                if (assunto.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(assunto.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(assunto);

                if (command.IdConsumidor.HasValue)
                {
                    MigracaoAssuntoEstrategico assuntoEstrategicoMigracao = MigracaoAssuntoEstrategico.CriarMigracaoAssuntoEstrategico(assunto.Id, command.IdConsumidor.Value);
                    DatabaseContext.Add(assuntoEstrategicoMigracao);
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

        public CommandResult AtualizarDoCivelEstrategico(AtualizarAssuntoDoCivelEstrategicoCommand command) {
            string entityName = "Assunto do Cível Estratégico";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));            

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ASSUNTO_CIVEL_ESTRATEGICO)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ASSUNTO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.AssuntoId));
                Assunto assunto = DatabaseContext.Assuntos.FirstOrDefault(x => x.Id == command.AssuntoId && x.EhCivelEstrategico);

                if (assunto is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.AssuntoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.AssuntoId));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.AssuntoId));
                assunto.AtualizarDoCivelEstrategico(command.Descricao, command.Ativo);

                MigracaoAssuntoEstrategico assuntoMigracaoEstrategico = DatabaseContext.MigracaoAssuntoEstrategico.FirstOrDefault(x => x.CodAssuntoCivelEstrat == assunto.Id);

                if (assuntoMigracaoEstrategico != null)
                {
                    DatabaseContext.Remove(assuntoMigracaoEstrategico);
                }

                if (command.IdConsumidor.HasValue)
                {
                    var migAssuntoEstrategico = MigracaoAssuntoEstrategico.CriarMigracaoAssuntoEstrategico(assunto.Id, command.IdConsumidor.Value);
                    DatabaseContext.Add(migAssuntoEstrategico);
                }

                if (assunto.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(assunto.Notifications.ToNotificationsString());
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

        public CommandResult RemoverDoCivelEstrategico(int assuntoId) {
            string entityName = "Assunto do Cível Estratégico";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));
            
            try {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ASSUNTO_CIVEL_ESTRATEGICO)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ASSUNTO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, assuntoId));
                Assunto assunto = DatabaseContext.Assuntos.FirstOrDefault(x => x.Id == assuntoId && x.EhCivelEstrategico);

                if (assunto is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, assuntoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, assuntoId));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                ValidarExclusao(assunto);

                MigracaoAssuntoEstrategico assuntoMigracaoEstrategico = DatabaseContext.MigracaoAssuntoEstrategico.FirstOrDefault(x => x.CodAssuntoCivelEstrat == assunto.Id);

                if (assuntoMigracaoEstrategico != null)
                {
                    DatabaseContext.Remove(assuntoMigracaoEstrategico);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, assuntoId));
                DatabaseContext.Remove(assunto);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        #endregion Civel Estrategico

        private void ValidarExclusao(Assunto assunto) {
            if (DatabaseContext.Processos.Any(x => x.Assunto == assunto)) {
                Logger.LogInformation($"Assunto '{ assunto.Id }' vinculado a 'Processos'");
                throw new InvalidOperationException("O Assunto selecionado se encontra relacionado a um processo.");
            }
        }
    }
}