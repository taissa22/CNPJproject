using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IDecisaoEventoRepository
    {
        CommandResult<PaginatedQueryResult<DecisaoEvento>> ObterPaginado(int eventoId, int pagina, int quantidade, DecisaoEventoSort sort, bool ascending);

        CommandResult<IReadOnlyCollection<DecisaoEvento>> Obter(int eventoId, DecisaoEventoSort sort, bool ascending);
    }
}
