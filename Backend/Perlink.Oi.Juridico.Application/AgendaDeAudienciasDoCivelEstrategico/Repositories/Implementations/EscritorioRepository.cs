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

    internal class EscritorioRepository : IEscritorioRepository {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEscritorioRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EscritorioRepository(IDatabaseContext databaseContext, ILogger<IEscritorioRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<Escritorio>> Obter() {
            string logName = "Escritório";

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_AGENDA_CE)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_AGENDA_CE, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Escritorio>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.Escritorios
                                .Where(a => a.Ativo == true && a.CivelEstrategico == true)
                                .OrderBy(e => e.Nome)
                                .AsNoTracking()
                                .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Escritorio>>.Valid(result);
        }

        public CommandResult<IReadOnlyCollection<Escritorio>> ObterAutorizadosAoUsuarioLogado() {
            string logName = "Escritório";

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_AGENDA_CE)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_AGENDA_CE, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Escritorio>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            IReadOnlyCollection<Escritorio> escritorios;

            if (UsuarioAtualEhInterno()) {
                escritorios = DatabaseContext.Escritorios
                                .Where(x => x.CivelEstrategico == true)
                                .OrderBy(p => p.Nome)
                                .AsNoTracking()
                                .ToArray();
            } else {
                escritorios = DatabaseContext.EscritoriosDosUsuarios
                                .Where(x => x.Usuario.Id.Equals(UsuarioAtual.Login))
                                .Select(x => x.Escritorio)
                                    .Where(x => x.CivelEstrategico == true)
                                .AsNoTracking()
                                .ToArray();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Escritorio>>.Valid(escritorios);
        }

        private bool UsuarioAtualEhInterno() {
            return !DatabaseContext.EscritoriosDosUsuarios.Any(x => x.UsuarioId == UsuarioAtual.Login);
        }
    }
}