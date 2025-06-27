using Oi.Juridico.Contextos.V2.PautaJuizadoComposicaoContext.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.PautaJuizado.DTOs
{
    public class ListarPrepostoAlocadoResponse
    {
        public int Id { get; set; }
        
        public string? Preposto { get; set; }
        
        public bool Principal { get; set; }
    }
}
