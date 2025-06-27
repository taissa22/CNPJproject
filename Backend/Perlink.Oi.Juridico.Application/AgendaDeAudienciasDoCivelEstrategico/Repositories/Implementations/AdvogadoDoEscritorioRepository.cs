using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories.Implementations {

    internal class AdvogadoDoEscritorioRepository : IAdvogadoDoEscritorioRepository {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAdvogadoDoEscritorioRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public AdvogadoDoEscritorioRepository(IDatabaseContext databaseContext, ILogger<IAdvogadoDoEscritorioRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<AdvogadoDoEscritorio>> ObterPorEscritorio(int escritorioId) {
            string logName = "Advogado do Escritório";

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_AGENDA_CE)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_AGENDA_CE, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<AdvogadoDoEscritorio>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.AdvogadosDosEscritorios
                                .Where(p => p.EscritorioId == escritorioId 
                                    && p.Escritorio.CivelEstrategico == true)
                                .OrderBy(p => p.Nome)
                                .AsNoTracking()
                                .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<AdvogadoDoEscritorio>>.Valid(result);
        }
    }
}