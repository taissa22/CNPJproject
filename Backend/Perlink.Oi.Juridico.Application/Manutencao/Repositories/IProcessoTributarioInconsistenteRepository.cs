using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IProcessoTributarioInconsistenteRepository
    {
        CommandResult<PaginatedQueryResult<ProcessoTributarioInconsistente>> ObterPaginado(int pagina, int quantidade,
            ProcessoTributarioInconsistenteSort sort, bool ascending);

        CommandResult<IReadOnlyCollection<ProcessoTributarioInconsistente>> Obter(ProcessoTributarioInconsistenteSort sort, bool ascending);
    }
}
