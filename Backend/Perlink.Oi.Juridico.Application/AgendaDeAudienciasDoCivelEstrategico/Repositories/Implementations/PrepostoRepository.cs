using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories.Implementations {

    internal class PrepostoRepository : IPrepostoRepository {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IPrepostoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public PrepostoRepository(IDatabaseContext databaseContext, ILogger<IPrepostoRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<Preposto>> Obter() {
            string logName = "Preposto";

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_AGENDA_CE)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_AGENDA_CE, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Preposto>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.AudienciasDosProcessos
                            .AsNoTracking()
                            .Where(x => x.Processo.TipoProcessoId == TipoProcesso.CIVEL_ESTRATEGICO.Id && !x.Preposto.EhCivelEstrategico)
                                .Select(x => x.Preposto)
                                    .Union(DatabaseContext.Prepostos.Where(x => x.EhCivelEstrategico))
                                .OrderBy(x => x.Nome)
                                .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Preposto>>.Valid(result);
        }
    }
}