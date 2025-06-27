using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Manutencao.Sorts;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IFielDepositarioRepository {
        CommandResult<PaginatedQueryResult<FielDepositario>> ObterPaginado(int pagina, int quantidade,
            FielDepositarioSort sort, bool ascending);
        CommandResult<IReadOnlyCollection<FielDepositario>> Obter(FielDepositarioSort sort, bool ascending);
    }
}
