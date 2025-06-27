using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface ITipoProcedimentoRepository
    {
        CommandResult<PaginatedQueryResult<Procedimento>> ObterPaginado(TipoProcesso tipoProcesso, int pagina, int quantidade,
        TipoProcedimentoSort sort, bool ascending, string pesquisa = null);

        CommandResult<IReadOnlyCollection<Procedimento>> Obter(TipoProcesso tipoProcesso, TipoProcedimentoSort sort, bool ascending, string pesquisa = null);

        CommandResult<IReadOnlyCollection<Procedimento>> ObterTodos();

        CommandResult<IEnumerable<TipoProcesso>> ObterComboboxTipoProcesso();

    }
}
