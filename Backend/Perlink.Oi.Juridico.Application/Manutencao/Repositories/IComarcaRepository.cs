using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;


namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IComarcaRepository
    {
        CommandResult<PaginatedQueryResult<Comarca>> ObterPaginado(int pagina, int quantidade, string codigoEstado, ComarcaSort sort, bool ascending, string pesquisa = null);

        CommandResult<IReadOnlyCollection<Comarca>> Obter(string codigoEstado, ComarcaSort sort, bool ascending, string pesquisa = null);


    }
}
