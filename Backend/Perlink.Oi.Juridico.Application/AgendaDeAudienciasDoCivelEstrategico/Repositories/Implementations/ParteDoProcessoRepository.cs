using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories.Implementations {

    internal class ParteDoProcessoRepository : IParteDoProcessoRepository {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IParteDoProcessoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public ParteDoProcessoRepository(IDatabaseContext databaseContext, ILogger<IParteDoProcessoRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<ParteProcesso>> ObterPorProcesso(int processoId) {
            string logName = "Parte do Processo";

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_AGENDA_CE)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_AGENDA_CE, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<ParteProcesso>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.PartesProcessos
                                .Where(x => x.ProcessoId == processoId)
                                .OrderBy(p => p.ParteId)
                                    .ThenBy(x => x.Parte.Nome)
                                .AsNoTracking()
                                .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<ParteProcesso>>.Valid(result);
        }
    }
}