using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using System.Linq;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Providers;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    public class MigracaoAssuntoEstrategicoService : IMigracaoAssuntoEstrategicoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<MigracaoAssuntoEstrategicoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public MigracaoAssuntoEstrategicoService(IDatabaseContext databaseContext, ILogger<MigracaoAssuntoEstrategicoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult AtualizarMigracaoEstrategico(AtualizarAssuntoMigracaoEstrategicoCommand command)
        {
            string entityName = "Migração Assunto Estrategico";
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

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.CodAssuntoCivelEstrat.Value));
                MigracaoAssuntoEstrategico migracaoAssuntoEstrategico = DatabaseContext.MigracaoAssuntoEstrategico.FirstOrDefault(a => a.CodAssuntoCivelEstrat == command.CodAssuntoCivelEstrat);

                if (migracaoAssuntoEstrategico is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodAssuntoCivelEstrat.Value));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodAssuntoCivelEstrat.Value));
                }

                migracaoAssuntoEstrategico.AtualizarMigracaoAssuntoEstrategico(command.CodAssuntoCivelEstrat.Value, command.CodAssuntoCivelCons.Value);

                if (migracaoAssuntoEstrategico.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(migracaoAssuntoEstrategico.Notifications.ToNotificationsString());
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(entityName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult CriarMigracaoEstrategico(CriarMigracaoAssuntoEstrategicoCommand command)
        {
            string entityName = "Migração Cível Estrategico";

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

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                MigracaoAssuntoEstrategico migracaoAssuntoEstrategico = MigracaoAssuntoEstrategico.CriarMigracaoAssuntoEstrategico(command.CodAssuntoCivelEstrat, command.CodAssuntoCivelCons);

                if (migracaoAssuntoEstrategico.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(migracaoAssuntoEstrategico.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(migracaoAssuntoEstrategico);
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
