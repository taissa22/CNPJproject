using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface ITipoDeParticipacaoRepository
    {
        CommandResult<PaginatedQueryResult<TipoDeParticipacao>> ObterPaginado(int pagina, int quantidade,
          TipoDeParticipacaoSort sort, bool ascending, string pesquisa = null);

        CommandResult<IReadOnlyCollection<TipoDeParticipacao>> Obter(TipoDeParticipacaoSort sort, bool ascending, string pesquisa = null);

    }
}
