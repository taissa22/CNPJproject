using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IJurosVigenciasCiveisRepository
    {
        CommandResult<PaginatedQueryResult<JurosCorrecaoProcesso>> ObterPaginado(TipoProcesso tipoProcesso, DateTime dataInicial, DateTime dataFinal, int pagina, int quantidade,
           JurosVigenciasCiveisSort sort, bool ascending);

        CommandResult<IReadOnlyCollection<JurosCorrecaoProcesso>> Obter(TipoProcesso tipoProcesso, DateTime dataInicial, DateTime dataFinal, JurosVigenciasCiveisSort sort, bool ascending);

        CommandResult<IEnumerable<TipoProcesso>> obterParaComboboxTipoProcesso();
    }
}