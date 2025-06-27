using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class AgendamentoDeFechamentoAtmPexRepository : IAgendamentoDeFechamentoAtmPexRepository
    {
        private ILogger<IAgendamentoDeFechamentoAtmPexRepository> Logger { get; }
        private IParametroJuridicoProvider ParametroJuridico { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }
        private IDatabaseContext DatabaseContext { get; }

        public AgendamentoDeFechamentoAtmPexRepository(ILogger<IAgendamentoDeFechamentoAtmPexRepository> logger, IParametroJuridicoProvider parametroJuridico, IDatabaseContext databaseContext)
        {
            Logger = logger;
            ParametroJuridico = parametroJuridico;
            DatabaseContext = databaseContext;
        }

        public CommandResult Remover(int agendamentoId)
        {
            try
            {
                AgendamentoDeFechamentoAtmPex agendamentoDeFechamentoAtmPex = new AgendamentoDeFechamentoAtmPex();
                agendamentoDeFechamentoAtmPex = DatabaseContext.AgendamentosDeFechamentosAtmPex
                                                .AsNoTracking()
                                                .Where(x => x.AgendamentoId == agendamentoId)
                                                .OrderByDescending(x => x.DataAgendamento)
                                                .FirstOrDefault();

                var agendamentosDeFechamentoAtmUfPex = DatabaseContext.AgendamentoDeFechamentoAtmUfPex
                                                                      .AsNoTracking()
                                                                      .Where(x => x.AgendamentoId == agendamentoId).ToList();

                foreach (var agendamentoUf in agendamentosDeFechamentoAtmUfPex)
                {
                    DatabaseContext.Remove(agendamentoUf);
                }

                if (agendamentoDeFechamentoAtmPex is null)
                {
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada("AgendamentoDeFechamentoAtmPex", agendamentoId));
                }

                DatabaseContext.Remove(agendamentoDeFechamentoAtmPex);
                DatabaseContext.SaveChanges();

                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                return CommandResult.Invalid(ex.Message);
            }
        }
    }
}
