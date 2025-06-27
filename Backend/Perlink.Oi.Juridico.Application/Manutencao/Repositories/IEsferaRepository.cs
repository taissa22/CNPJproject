using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IEsferaRepository
    {
        CommandResult<PaginatedQueryResult<Esfera>> ObterPaginado(int pagina, int quantidade,
            EsferasSort sort, bool ascending);

        CommandResult<IReadOnlyCollection<Esfera>> Obter(EsferasSort sort, bool ascending);
    }
}
