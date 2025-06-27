using Perlink.Oi.Juridico.Application.SAP.Enuns;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.SAP.Repositories
{
    public interface IAgendamentosMigracaoPedidosSapRepository
    {
        CommandResult<PaginatedQueryResult<AgendamentoMigracaoPedidosSap>> ObterPaginado(int pagina);

        CommandResult<string> ObterArquivos(TipoArquivo tipoArquivo, int? agendamentoId = null);

    }
}
