using Microsoft.AspNetCore.Http;

namespace Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.DTOs
{
    public class IncluirAnexoRequest
    {
        public string Comentario { get; set; } = String.Empty;
        public required IFormFile Arquivo { get; set; }

    }
}
