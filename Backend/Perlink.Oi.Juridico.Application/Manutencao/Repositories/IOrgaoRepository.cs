using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IOrgaoRepository
    {
        CommandResult<IReadOnlyCollection<Orgao>> Obter(TipoOrgao tipoOrgao, OrgaoSort sort, bool ascending);

        CommandResult<PaginatedQueryResult<Orgao>> ObterPaginado(TipoOrgao tipoOrgao, OrgaoSort sort, bool ascending, int pagina, int quantidade);
    }
}
