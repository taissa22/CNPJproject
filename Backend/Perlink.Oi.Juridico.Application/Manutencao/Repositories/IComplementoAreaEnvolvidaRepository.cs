using Perlink.Oi.Juridico.Infra.Entities;
using System.Collections.Generic;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using System;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IComplementoDeAreaEnvolvidaRepository
    {
        CommandResult<IReadOnlyCollection<ComplementoDeAreaEnvolvida>> Obter(TipoProcesso tipoProcesso, string pesquisa, ComplementoDeAreaEnvolvidaSort sort, bool ascending);
        CommandResult<PaginatedQueryResult<ComplementoDeAreaEnvolvida>> ObterPaginado(TipoProcesso tipoProcesso, string pesquisa, ComplementoDeAreaEnvolvidaSort sort, bool ascending, int pagina, int quantidade);
    }
}
