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

    internal class AcaoService : IAcaoService {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAcaoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public AcaoService(IDatabaseContext databaseContext, ILogger<IAcaoService> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        #region Civel Estratégico

        public CommandResult CriarDoCivelEstrategico(CriarAcaoDoCivelEstrategicoCommand command) {
            string entityName = "Ação Cível Estratégico";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_CIVEL_ESTRATEGICO)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Acao acao = Acao.CriarDoCivelEstrategico(DataString.FromString(command.Descricao), command.Ativo);

                if (acao.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(acao.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(acao);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                if (command.IdConsumidor.HasValue)
                {
                    MigracaoAcao acaoMigracao = MigracaoAcao.CriarMigracaoAcao(acao.Id, command.IdConsumidor.Value);
                    DatabaseContext.Add(acaoMigracao);
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

        public CommandResult AtualizarDoCivelEstrategico(AtualizarAcaoDoCivelEstrategicoCommand command) {
            string entityName = "Ação Cível Estratégico";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_CIVEL_ESTRATEGICO)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.id));
                Acao acao = DatabaseContext.Acoes.FirstOrDefault(a => a.Id == command.id && a.EhCivelEstrategico);

                if (acao is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.id));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.id));
                acao.AtualizarDoCivelEstrategico(DataString.FromString(command.Descricao), command.Ativo);

                if (acao.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(acao.Notifications.ToNotificationsString());
                }

                MigracaoAcao acaoMigracao = DatabaseContext.MigracaoAcao.FirstOrDefault(x => x.CodAcaoEstrategico == acao.Id);

                if (acaoMigracao != null)
                {
                    DatabaseContext.Remove(acaoMigracao);
                }

                if (command.IdConsumidor.HasValue)
                {
                    var migAssunto = MigracaoAcao.CriarMigracaoAcao(acao.Id, command.IdConsumidor.Value);
                    DatabaseContext.Add(migAssunto);
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

        public CommandResult RemoverDoCivelEstrategico(int acaoId) {
            string entityName = "Ação Cível Estratégico";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_CIVEL_ESTRATEGICO)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, acaoId));
                Acao acao = DatabaseContext.Acoes.FirstOrDefault(a => a.Id == acaoId && a.EhCivelEstrategico);

                if (acao is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, acaoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, acaoId));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                ValidarExclusao(acao);

                MigracaoAcao acaoMigracao = DatabaseContext.MigracaoAcao.FirstOrDefault(x => x.CodAcaoEstrategico == acao.Id);

                if (acaoMigracao != null)
                {
                    DatabaseContext.Remove(acaoMigracao);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, acaoId));
                DatabaseContext.Remove(acao);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        #endregion Civel Estratégico

        #region Civel Consumidor

        public CommandResult CriarDoCivelConsumidor(CriarAcaoDoCivelConsumidorCommand command) {
            string entityName = "Ação Cível Consumidor";
            string entityNaturezaBBName = "Natureza Ação BB";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_CIVEL_CONSUMIDOR)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                NaturezaAcaoBB natureza = null;

                if (command.NaturezaAcaoBBId > 0) {
                    Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityNaturezaBBName, (int)command.NaturezaAcaoBBId));
                    natureza = DatabaseContext.NaturezasDasAcoesBB.FirstOrDefault(n => n.Id == (int)command.NaturezaAcaoBBId);

                    if (natureza is null) {
                        Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityNaturezaBBName, (int)command.NaturezaAcaoBBId));
                        return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityNaturezaBBName, (int)command.NaturezaAcaoBBId));
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Acao acao = Acao.CriarDoCivelConsumidor(DataString.FromString(command.Descricao), natureza, command.EnviarAppPreposto);

                if (acao.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(acao.Notifications.ToNotificationsString());
                }


                DatabaseContext.Add(acao);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                if (command.IdEstrategico.HasValue)
                {
                    MigracaoAcaoConsumidor acaoMigracaoConsumidor = MigracaoAcaoConsumidor.CriarMigracaoAcaoConsumidor(acao.Id, command.IdEstrategico.Value);
                    DatabaseContext.Add(acaoMigracaoConsumidor);
                }

                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult AtualizarDoCivelConsumidor(AtualizarAcaoDoCivelConsumidorCommand command) {
            string entityName = "Ação Cível Consumidor";
            string entityNaturezaBBName = "Natureza Ação BB";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_CIVEL_CONSUMIDOR)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.id));
                Acao acao = DatabaseContext.Acoes.FirstOrDefault(a => a.Id == command.id && a.EhCivelConsumidor);

                if (acao is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.id));
                }

                NaturezaAcaoBB natureza = null;

                if (command.NaturezaAcaoBBId > 0) {
                    Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityNaturezaBBName, (int)command.NaturezaAcaoBBId));
                    natureza = DatabaseContext.NaturezasDasAcoesBB.FirstOrDefault(n => n.Id == command.NaturezaAcaoBBId);
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityNaturezaBBName, command.id));
                acao.AtualizarDoCivelConsumidor(DataString.FromString(command.Descricao), natureza, command.EnviarAppPreposto);
            
                if (acao.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(acao.Notifications.ToNotificationsString());
                }

                MigracaoAcaoConsumidor acaoMigracaoConsumidor = DatabaseContext.MigracaoAcaoConsumidor.FirstOrDefault(x => x.CodAcaoCivel == acao.Id);

                if (acaoMigracaoConsumidor != null)
                {
                    DatabaseContext.Remove(acaoMigracaoConsumidor);
                }

                if (command.IdEstrategico.HasValue)
                {
                    var migAssunto = MigracaoAcaoConsumidor.CriarMigracaoAcaoConsumidor(acao.Id, command.IdEstrategico.Value);
                    DatabaseContext.Add(migAssunto);
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(entityName));
                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult RemoverDoCivelConsumidor(int acaoId) {
            string entityName = "Ação Cível Consumidor";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_CIVEL_CONSUMIDOR)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, acaoId));
                Acao acao = DatabaseContext.Acoes.FirstOrDefault(a => a.Id == acaoId && a.EhCivelConsumidor);

                if (acao is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, acaoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, acaoId));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                ValidarExclusao(acao);

                MigracaoAcaoConsumidor acaoMigracaoConsumidor = DatabaseContext.MigracaoAcaoConsumidor.FirstOrDefault(x => x.CodAcaoCivel == acao.Id);

                if (acaoMigracaoConsumidor != null)
                {
                    DatabaseContext.Remove(acaoMigracaoConsumidor);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, acaoId));
                DatabaseContext.Remove(acao);

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

        #region Trabalhista

        public CommandResult CriarDoTrabalhista(CriarAcaoDoTrabalhistaCommand command) {
            string entityName = "Ação Trabalhista";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_TRABALHISTA)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_TRABALHISTA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Acao acao = Acao.CriarDoTrabalhista(DataString.FromString(command.Descricao));

                if (acao.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(acao.Notifications.ToNotificationsString());
                }
                
                DatabaseContext.Add(acao);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult AtualizarDoTrabalhista(AtualizarAcaoDoTrabalhistaCommand command) {
            string entityName = "Ação Trabalhista";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_TRABALHISTA)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_TRABALHISTA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.id));
                Acao acao = DatabaseContext.Acoes.FirstOrDefault(a => a.Id == command.id && a.EhTrabalhista);

                if (acao is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.id));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.id));
                acao.AtualizarDoTrabalhista(DataString.FromString(command.Descricao));

                if (acao.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(acao.Notifications.ToNotificationsString());
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

        public CommandResult RemoverDoTrabalhista(int acaoId) {
            string entityName = "Ação Trabalhista";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_TRABALHISTA)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_TRABALHISTA, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, acaoId));
                Acao acao = DatabaseContext.Acoes.FirstOrDefault(a => a.Id == acaoId && a.EhTrabalhista);

                if (acao is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, acaoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, acaoId));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                ValidarExclusao(acao);

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, acaoId));
                DatabaseContext.Remove(acao);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandRemocao));
                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        #endregion Trabalhista

        #region Tributario Judicial

        public CommandResult CriarDoTributarioJudicial(CriarAcaoDoTributarioJudicialCommand command) {
            string entityName = "Ação Tributário Judicial";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_TRIBUTARIA_JUDICIAL)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_TRIBUTARIA_JUDICIAL, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                Acao acao = Acao.CriarDoTributario(DataString.FromString(command.Descricao));

                if (acao.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(acao.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(acao);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult AtualizarDoTributarioJudicial(AtualizarAcaoDoTributarioJudicialCommand command) {
            string entityName = "Ação Tributário Judicial";
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));
            
            try {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_TRIBUTARIA_JUDICIAL)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_TRIBUTARIA_JUDICIAL, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.id));
                Acao acao = DatabaseContext.Acoes.Where(x => x.EhTributaria).FirstOrDefault(a => a.Id == command.id);

                if (acao is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.id));
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.id));
                acao.AtualizarDoTributario(DataString.FromString(command.Descricao));

                if (acao.HasNotifications) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(acao.Notifications.ToNotificationsString());
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

        public CommandResult RemoverDoTributarioJudicial(int acaoId) {
            string entityName = "Ação Tributário Judicial";
            string commandName = $"Excluir {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ACAO_TRIBUTARIA_JUDICIAL)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ACAO_TRIBUTARIA_JUDICIAL, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, acaoId));
                Acao acao = DatabaseContext.Acoes.Where(x => x.EhTributaria).FirstOrDefault(a => a.Id == acaoId);

                if (acao is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, acaoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, acaoId));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));
                ValidarExclusao(acao);

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, acaoId));
                DatabaseContext.Remove(acao);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        #endregion Tributario Judicial

        private void ValidarExclusao(Acao acao) {
            var existeProcessoComAcao = DatabaseContext.Processos.Any(x => x.Acao == acao);
            if (existeProcessoComAcao) throw new InvalidOperationException("A Ação selecionada se encontra relacionada a um processo.");

            var existeAndamentoComAcao = DatabaseContext.AndamentosDosProcessos.Any(x => x.Acao == acao);
            if (existeAndamentoComAcao) throw new InvalidOperationException("A Ação selecionada se encontra relacionada a um processo.");
        }
    }
}