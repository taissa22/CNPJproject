using Perlink.Oi.Juridico.Application.Manutencao.Results.TipoPrazo;
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
    public interface ITipoPrazoRepository
    {
        CommandResult<PaginatedQueryResult<TipoPrazoCommandResult>> ObterPaginado(int pagina, int quantidade,
           TipoPrazoSort sort, bool ascending, TipoProcessoManutencao tipoProcesso, string pesquisa = null);

        CommandResult<IReadOnlyCollection<TipoPrazoCommandResult>> Obter(TipoPrazoSort sort, bool ascending, TipoProcessoManutencao tipoProcesso, string pesquisa = null);

        CommandResult<IEnumerable<TipoProcessoManutencao>> ObterComboboxTipoProcesso();
    }
}
