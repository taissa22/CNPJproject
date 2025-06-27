using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface ITipoPendenciaRepository
    {
        CommandResult<PaginatedQueryResult<TipoPendencia>> ObterPaginado(int pagina, int quantidade,
          TipoPendenciaSort sort, bool ascending, string pesquisa = null);

        CommandResult<IReadOnlyCollection<TipoPendencia>> Obter(TipoPendenciaSort sort, bool ascending, string pesquisa = null);
    }
}
