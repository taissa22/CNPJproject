using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface ITipoVaraRepository
    {
        CommandResult<PaginatedQueryResult<TipoVara>> ObterPaginado(int pagina, int quantidade, TipoVaraSort sort, bool ascending, string pesquisa = null);

        CommandResult<IReadOnlyCollection<TipoVara>> Obter(TipoVaraSort sort, bool ascending, string pesquisa = null);

        CommandResult<IReadOnlyCollection<TipoVara>> ObterTodos();

        CommandResult<bool> UtilizadoEmProcesso(int codTipoVara, int codTipoProcesso);
    }
}