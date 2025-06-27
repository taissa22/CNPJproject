using Perlink.Oi.Juridico.Application.Manutencao.Results.Tipo_Documento;
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
    public interface ITipoDocumentoRepository
    {
        CommandResult<PaginatedQueryResult<TipoDocumentoCommandResult>> ObterPaginado(TipoProcesso tipoProcesso, int pagina, int quantidade,
           TipoDocumentoSort sort, bool ascending, string pesquisa = null);

        CommandResult<IReadOnlyCollection<TipoDocumentoCommandResult>> Obter(TipoProcesso tipoProcesso, TipoDocumentoSort sort, bool ascending, string pesquisa = null);

        CommandResult<IEnumerable<TipoProcesso>> ObterComboboxTipoProcesso();
     
    }
}
