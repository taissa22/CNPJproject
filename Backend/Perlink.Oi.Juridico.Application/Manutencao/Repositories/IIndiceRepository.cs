using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IIndiceRepository
    {
        CommandResult<PaginatedQueryResult<Indice>> ObterPaginado(int pagina, int quantidade,
            IndicesSort sort, bool ascending, string pesquisa = null);

        CommandResult<IReadOnlyCollection<Indice>> Obter(IndicesSort sort, bool ascending, string pesquisa = null);

        CommandResult<IReadOnlyCollection<Indice>> ObterTodos();

        CommandResult<bool> UtilizadoEmCotacao(int codIndice);
    }
}