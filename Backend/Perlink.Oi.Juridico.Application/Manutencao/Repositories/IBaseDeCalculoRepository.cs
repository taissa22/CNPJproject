using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IBaseDeCalculoRepository
    {
        CommandResult<PaginatedQueryResult<BaseDeCalculo>> ObterPaginado(int pagina, int quantidade, BaseDeCalculoSort sort, bool ascending,
            string pesquisa);

        CommandResult<IReadOnlyCollection<BaseDeCalculo>> Obter(BaseDeCalculoSort sort, bool ascending, string pesquisa);
    }
}