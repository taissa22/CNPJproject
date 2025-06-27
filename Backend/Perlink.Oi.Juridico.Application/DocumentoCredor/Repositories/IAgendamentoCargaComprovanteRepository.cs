using Perlink.Oi.Juridico.Application.DocumentoCredor.Enums;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.IO.Compression;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Repositories
{
    public interface IAgendamentoCargaComprovanteRepository
    {
        CommandResult<PaginatedQueryResult<AgendamentoCargaComprovante>> ObterPaginado(int pagina);
        CommandResult<byte[]> ObterArquivos(TipoArquivo tipoArquivo, int? agendamentoId);

    }
}
