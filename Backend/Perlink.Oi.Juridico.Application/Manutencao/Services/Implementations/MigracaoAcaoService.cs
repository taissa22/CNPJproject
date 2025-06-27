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
    public class MigracaoAcaoService : IMigracaoAcaoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IMigracaoAcaoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public MigracaoAcaoService(IDatabaseContext databaseContext, ILogger<IMigracaoAcaoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult CriarMigracaoAssutoConsumidor(CriarMigracaoAcaoCommand command)
        {
            string entityName = "Migração Ação Cível Estrategico";
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
                MigracaoAcao migracaoAcaoConsumidor = MigracaoAcao.CriarMigracaoAcao(command.CodAcaoConsumidor, command.CodAcaoEstrategico);

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

        public CommandResult AtualizarMigracaoAssutoConsumidor(AlterarMigracaoAcaoCommand command)
        {
            string entityName = "Migração Ação Estrategico";
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

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.CodAcaoConsumidor));
                MigracaoAcao migracaoAcaoConsumidor = DatabaseContext.MigracaoAcao.FirstOrDefault(a => a.CodAcaoConsumidor == command.CodAcaoConsumidor);

                if (migracaoAcaoConsumidor is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodAcaoConsumidor));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodAcaoConsumidor));
                }

                migracaoAcaoConsumidor.AtualizarMigracaoAcao(command.CodAcaoConsumidor, command.CodAcaoEstrategico);

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

    }
}
