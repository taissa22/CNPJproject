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

    internal class ComarcaRepository : IComarcaRepository {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IComarcaRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public ComarcaRepository(IDatabaseContext databaseContext, ILogger<IComarcaRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<Comarca>> ObterPorEstado(string estadoId) {
            string logName = "Comarca";

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_AGENDA_CE)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_AGENDA_CE, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Comarca>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.Comarcas
                            .Where(p => p.Estado.Id == estadoId)
                            .OrderBy(p => p.Nome)
                            .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Comarca>>.Valid(result);
        }
    }
}