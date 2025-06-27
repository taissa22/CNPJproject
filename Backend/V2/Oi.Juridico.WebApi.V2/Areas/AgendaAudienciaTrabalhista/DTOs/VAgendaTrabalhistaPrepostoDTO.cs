using Oi.Juridico.Shared.V2.Enums;

namespace Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.DTOs
{
    public class VAgendaTrabalhistaPrepostoDTO
    {
        public int? CodProcesso { get; set; }        
        public int? SeqAudiencia { get; set; }
        public int CodParte { get; set; }
    }
}
