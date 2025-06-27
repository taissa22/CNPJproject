using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IEmpresaDoGrupoRepository
    {   
        CommandResult<IReadOnlyCollection<EmpresaDoGrupo>> Obter(EmpresaDoGrupoSort sort, bool ascending, DataString? nome, CNPJ? cnpj, DataString? centroSap);

        CommandResult<PaginatedQueryResult<EmpresaDoGrupo>> ObterPaginado(EmpresaDoGrupoSort sort, bool ascending, int pagina, int quantidade, DataString? nome, CNPJ? cnpj, DataString? centroSap);

        CommandResult<bool> Existe(DataString nome, int? id);

        CommandResult<IEnumerable<string>> ObterNomesPorCNPJ(CNPJ cnpj);
    }
}