using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Services.Implementatios
{
    internal class AudienciaService : IAudienciaService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAudienciaService> Logger { get; }
        public IUsuarioAtualProvider UsuarioAtual { get; }

        public AudienciaService(IDatabaseContext databaseContext, IUsuarioAtualProvider user, ILogger<IAudienciaService> logger)
        {
            DatabaseContext = databaseContext;
            UsuarioAtual = user;
            Logger = logger;
        }      

        public CommandResult Atualizar(AtualizarAudienciaCommand command)
        {
            string entityName = "Audiência";
            string commandName = $"Atualizar {entityName}";         

            try
            {
                command.Validate();
                if (command.Invalid) {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(entityName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ALTERAR_AUDIENCIA_CE)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ALTERAR_AUDIENCIA_CE, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Sequencial));
                AudienciaDoProcesso audiencia = DatabaseContext.AudienciasDosProcessos
                                                       .FirstOrDefault(x => x.Sequencial == command.Sequencial
                                                       && x.Processo.Id == command.ProcessoId);

                if (audiencia is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.ProcessoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.ProcessoId));
                }

                Escritorio escritorio = null;
                if (command.EscritorioId != null) {
                    Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade("Escritório", (int)command.EscritorioId));
                    escritorio = DatabaseContext.Escritorios.FirstOrDefault(y => y.Id == command.EscritorioId);
                }

                AdvogadoDoEscritorio advEscritorio = null; 
                if (command.AdvogadoId != null) {
                    Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade("Advogado do Escritório", (int)command.AdvogadoId));
                    advEscritorio = DatabaseContext.AdvogadosDosEscritorios.FirstOrDefault(y => y.Id == command.AdvogadoId && y.EscritorioId == command.EscritorioId);
                }

                Preposto preposto = null; 
                if (command.PrepostoId != null) {
                    Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade("Preposto", (int)command.PrepostoId));
                    preposto = DatabaseContext.Prepostos.FirstOrDefault(y => y.Id == command.PrepostoId);
                }

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));
                audiencia.AtualizarAgenda(advEscritorio, escritorio, preposto);               

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