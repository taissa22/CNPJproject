using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IGrupoPedidoRepository
    {        
        CommandResult<PaginatedQueryResult<GrupoPedido>> ObterPaginado(TipoProcesso tipoProcesso, GrupoPedidoSort sort, bool ascending, int pagina, int quantidade, string pesquisa);
        CommandResult<IReadOnlyCollection<GrupoPedido>> Obter(TipoProcesso tipoProcesso, GrupoPedidoSort sort, bool ascending, string pesquisa = null);
        CommandResult<IReadOnlyCollection<GrupoPedido>> ObterTodos(TipoProcesso tipoProcesso);
    }
}
