using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;


namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IMotivoProvavelZeroRepository
    {
        CommandResult<PaginatedQueryResult<MotivoProvavelZero>> ObterPaginado(int pagina, int quantidade, MotivoProvavelZeroSort sort, bool ascending, string pesquisa = null);

        CommandResult<IReadOnlyCollection<MotivoProvavelZero>> Obter(MotivoProvavelZeroSort sort, bool ascending, string pesquisa = null);


    }
}
