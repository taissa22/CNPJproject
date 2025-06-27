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

    internal class EmpresaDoGrupoRepository : IEmpresaDoGrupoRepository {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEmpresaDoGrupoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EmpresaDoGrupoRepository(IDatabaseContext databaseContext, ILogger<IEmpresaDoGrupoRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<EmpresaDoGrupo>> Obter() {
            string logName = "Empresa do Grupo";

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_AGENDA_CE)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_AGENDA_CE, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<EmpresaDoGrupo>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.EmpresasDoGrupoParaAgendaDeAudiencias
                            .Select(x => EmpresaDoGrupo.Obter(x.Id, x.Nome))
                            .OrderBy(e => e.Nome)
                            .AsNoTracking()
                            .ToArray();
            
            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<EmpresaDoGrupo>>.Valid(result);
        }

        public CommandResult<PaginatedQueryResult<EmpresaDoGrupo>> ObterParaDropdown(int pagina, int quantidade, int empresaDoGrupoId = 0) {
            string logName = "Empresa do Grupo";

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_AGENDA_CE)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_AGENDA_CE, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<EmpresaDoGrupo>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.EmpresasDoGrupoParaAgendaDeAudiencias
                            .OrderBy(e => e.Nome)
                            .AsNoTracking();            
           
            var data = result.Skip(pagina).Take(quantidade);

            if (empresaDoGrupoId > 0 && (!data.Any(x => x.Id == empresaDoGrupoId))) {
                var empresaDoGrupoQuery = DatabaseContext.EmpresasDoGrupoParaAgendaDeAudiencias.Where(x => x.Id == empresaDoGrupoId).OrderBy(x => x.Nome).AsNoTracking();
                data = empresaDoGrupoQuery.Concat(data);
            }

            var resultado = new PaginatedQueryResult<EmpresaDoGrupo>() {
                Total = 0,
                Data = data.Select(x => EmpresaDoGrupo.Obter(x.Id, x.Nome)).ToArray(),
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<EmpresaDoGrupo>>.Valid(resultado);
        }
    }
}