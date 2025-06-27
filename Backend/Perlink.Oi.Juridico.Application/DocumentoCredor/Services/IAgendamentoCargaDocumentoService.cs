using Microsoft.AspNetCore.Http;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Services {

    public interface IAgendamentoCargaDocumentoService {

        CommandResult Remover(int agendamentoCargaDocumentoId);

        CommandResult Criar(IFormFile documentos);

    }
}