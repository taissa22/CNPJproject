using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IVaraRepository
    {
        CommandResult<PaginatedQueryResult<Vara>> ObterPaginado(int comarcaId, int pagina, int quantidade, VaraSort sort, bool ascending,string pesquisa);
        CommandResult<PaginatedQueryResult<Vara>> ObterPorComarca(int comarcaId);

    }
}
