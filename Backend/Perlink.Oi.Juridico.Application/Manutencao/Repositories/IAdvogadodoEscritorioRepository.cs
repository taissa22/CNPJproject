using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;


namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IAdvogadoDoEscritorioRepository
    {
        CommandResult<PaginatedQueryResult<AdvogadoDoEscritorio>> ObterPaginado(int escritorioId,string estado,  int pagina, int quantidade, AdvogadodoEscritorioSort sort, bool ascending, string pesquisa = null);

        CommandResult<IReadOnlyCollection<AdvogadoDoEscritorio>> Obter(int escritorioID, string estado, AdvogadodoEscritorioSort sort, bool ascending, string pesquisa = null);

        CommandResult<IEnumerable<AdvogadoDoEscritorio>> ObterTodos(AdvogadodoEscritorioSort sort, bool ascending, string pesquisa = null);

    }
}
