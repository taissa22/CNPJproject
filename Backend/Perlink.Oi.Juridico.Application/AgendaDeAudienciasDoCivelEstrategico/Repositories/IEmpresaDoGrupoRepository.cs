using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories {

    public interface IEmpresaDoGrupoRepository {

        CommandResult<IReadOnlyCollection<EmpresaDoGrupo>> Obter();

        CommandResult<PaginatedQueryResult<EmpresaDoGrupo>> ObterParaDropdown(int pagina, int quantidade, int empresaDoGrupoId = 0);
    }
}