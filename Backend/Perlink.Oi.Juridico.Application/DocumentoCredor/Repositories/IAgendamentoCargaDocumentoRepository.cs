using Perlink.Oi.Juridico.Application.DocumentoCredor.Enums;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Repositories {

    public interface IAgendamentoCargaDocumentoRepository {

        CommandResult<PaginatedQueryResult<AgendamentoCargaDocumento>> ObterPaginado(int pagina);

        CommandResult<string> ObterArquivos(TipoArquivo tipoArquivo, int? agendamentoId = null);
    }
}