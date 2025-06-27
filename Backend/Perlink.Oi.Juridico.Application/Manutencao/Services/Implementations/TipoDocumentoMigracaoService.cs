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
    public class TipoDocumentoMigracaoService : ITipoDocumentoMigracaoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoDocumentoMigracaoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoDocumentoMigracaoService(IDatabaseContext databaseContext, ILogger<ITipoDocumentoMigracaoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }        

        public CommandResult AtualizarTipoDocumentoMigracaoConsumidor(AlterarTipoDocumentoMigracaoConsumidor command)
        {
            string entityName = "Migração Tipo Documento Consumidor";
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

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.CodTipoDocCivelConsumidor));
                TipoDocumentoMigracao tipoDocumentoMigracao = DatabaseContext.TipoDocumentoMigracao.FirstOrDefault(a => a.CodTipoDocCivelConsumidor == command.CodTipoDocCivelEstrategico);

                if (tipoDocumentoMigracao is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodTipoDocCivelConsumidor));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.CodTipoDocCivelConsumidor));
                }

                tipoDocumentoMigracao.AtualizarAcaoConsumidor(command.CodTipoDocCivelConsumidor, command.CodTipoDocCivelEstrategico);

                if (tipoDocumentoMigracao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(tipoDocumentoMigracao.Notifications.ToNotificationsString());
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

        public CommandResult CriarTipoDocumentoMigracaoConsumidor(CriarTipoDocumentoMigracaoConsumidor command)
        {
            string entityName = "Migração Tipo Documento Consumidor";
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
                TipoDocumentoMigracao tipoDocumentoMigracao = TipoDocumentoMigracao.CriarTipoDocumentoMigracao(command.CodTipoDocCivelConsumidor, command.CodTipoDocCivelEstrategico);

                if (tipoDocumentoMigracao.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(tipoDocumentoMigracao.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(tipoDocumentoMigracao);
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
