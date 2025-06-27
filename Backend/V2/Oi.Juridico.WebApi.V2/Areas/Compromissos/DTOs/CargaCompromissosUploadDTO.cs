using Microsoft.AspNetCore.Http;

namespace Oi.Juridico.WebApi.V2.Areas.AgendamentoCargaCompromissos.DTOs
{
    public class CargaCompromissosUploadDTO
    {
        public IFormFile ArquivoCompromisso { get; set; }

    }
}
