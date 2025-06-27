using Microsoft.AspNetCore.Http;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Services
{
    public interface IAgendamentoCargaComprovanteService
    {
        CommandResult<bool> ValidarBaseSAP(IFormFile baseSAP);

        CommandResult Remover(int id);

        CommandResult Criar(IFormFile comprovantes, IFormFile baseSAP);
    }
}
