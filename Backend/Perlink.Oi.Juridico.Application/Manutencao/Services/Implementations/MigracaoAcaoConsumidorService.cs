using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
   public class MigracaoAcaoConsumidorService : IMigracaoAcaoConsumidorService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IMigracaoAcaoConsumidorService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }
        public MigracaoAcaoConsumidorService(IDatabaseContext databaseContext, ILogger<IMigracaoAcaoConsumidorService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }
    

        public CommandResult AtualizarMigracaoAssutoConsumidor(AlterarMigracaoAcaoConsumidor command)
        {
            string entityName = "Migração Ação Consumidor";
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

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.CodAcaoCivel));
                MigracaoAcaoConsumidor migracaoAcaoConsumidor = DatabaseContext.MigracaoAcaoConsumidor.FirstOrDefault(a => a.CodAcaoCivel == command.CodAcaoCivelEstrategico);

                if (migracaoAcaoConsumidor is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodAcaoCivel));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodAcaoCivel));
                }

                migracaoAcaoConsumidor.AtualizarAcaoConsumidor(command.CodAcaoCivel, command.CodAcaoCivelEstrategico);

                if (migracaoAcaoConsumidor.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(migracaoAcaoConsumidor.Notifications.ToNotificationsString());
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

        public CommandResult CriarMigracaoAssutoConsumidor(CriarMigracaoAcaoConsumidor command)
        {
            string entityName = "Migração Ação Cível Consumidor";
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
                MigracaoAcaoConsumidor migracaoAcaoConsumidor = MigracaoAcaoConsumidor.CriarMigracaoAcaoConsumidor(command.CodAcaoCivel, command.CodAcaoCivelEstrategico);

                if (migracaoAcaoConsumidor.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(migracaoAcaoConsumidor.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(migracaoAcaoConsumidor);
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
