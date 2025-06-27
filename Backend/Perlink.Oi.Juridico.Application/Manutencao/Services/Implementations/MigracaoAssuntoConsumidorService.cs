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
    public class MigracaoAssuntoConsumidorService : IMigracaoAssuntoConsumidorService

    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IMigracaoAssuntoConsumidorService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public MigracaoAssuntoConsumidorService(IDatabaseContext databaseContext, ILogger<IMigracaoAssuntoConsumidorService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult AtualizarMigracaoConsumidor(AtualizarAssuntoMigracaoConsumidorCommand command)
        {
            string entityName = "Migração Assunto Consumidor";            
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

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.CodAssuntoCivel.Value ));
                MigracaoAssunto migracaoAssuntoConsumidor = DatabaseContext.MigracaoAssunto.FirstOrDefault(a => a.CodAssuntoCivel == command.CodAssuntoCivel);

                if (migracaoAssuntoConsumidor is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodAssuntoCivel.Value));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodAssuntoCivel.Value));
                }

                migracaoAssuntoConsumidor.AtualizarMigracaoAssuntoConsumidor(command.CodAssuntoCivel.Value, command.CodAssuntoCivelEstrategico.Value);

                if (migracaoAssuntoConsumidor.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(migracaoAssuntoConsumidor.Notifications.ToNotificationsString());
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

        public CommandResult CriarMigracaoConsumidor(CriarMigracaoAssuntoConsumidorCommand command)
        {
            string entityName = "Migração Cível Consumidor";
            
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
                MigracaoAssunto migracaoAssuntoConsumidor = MigracaoAssunto.CriarMigracaoAssuntoConsumidor(command.CodAssuntoCivel, command.CodAssuntoCivelEstrategico);

                if (migracaoAssuntoConsumidor.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(migracaoAssuntoConsumidor.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(migracaoAssuntoConsumidor);
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
