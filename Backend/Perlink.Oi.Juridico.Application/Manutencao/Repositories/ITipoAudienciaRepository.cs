using Perlink.Oi.Juridico.Application.Manutencao.Results.TiposAudiencias;
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
    public interface ITipoAudienciaRepository
    {
        CommandResult<PaginatedQueryResult<TipoAudienciaCommandResult>> ObterPaginado(int pagina, int quantidade,
          TipoAudienciaSort sort, bool ascending, TipoProcesso tipoProcesso, string pesquisa = null);

        CommandResult<IReadOnlyCollection<TipoAudienciaCommandResult>> Obter(TipoAudienciaSort sort, bool ascending, TipoProcesso tipoProcesso, string pesquisa = null);

        CommandResult<IEnumerable<TipoProcesso>> ObterComboboxTipoProcesso();
    }
}
