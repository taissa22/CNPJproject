using Microsoft.AspNetCore.Http;

namespace Perlink.Oi.Juridico.WebApi.DTOs.DocumentoCredor
{
    public class CargaComprovantesUploadDTO
    {
        public IFormFile ArquivoComprovantes { get; set; }
        public IFormFile ArquivoBaseSAP { get; set; }

    }
}
