using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IIndiceCorrecaoEsferaRepository
    {
        CommandResult<PaginatedQueryResult<IndiceCorrecaoEsfera>> ObterPaginado(int esferaId ,int pagina, int quantidade,IndiceCorrecaoEsferaSort sort, bool ascending);

        CommandResult<IReadOnlyCollection<IndiceCorrecaoEsfera>> Obter(int esferaId, IndiceCorrecaoEsferaSort sort, bool ascending);
    }
}
