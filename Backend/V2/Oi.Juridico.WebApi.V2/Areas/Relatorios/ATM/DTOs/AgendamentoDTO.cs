using NuGet.Protocol.Plugins;
using Oi.Juridico.Shared.V2.Enums;

namespace Oi.Juridico.WebApi.V2.Areas.Relatorios.ATM.DTOs
{
    public class AgendamentoDTO
    { 
        public decimal CodFechContCcMedia { get; set; }
        public DateTime MesAnoContabil { get; set; }
        public DateTime DatFechamento { get; set; }
        public string IndFechMensal { get; set;     } 
        public string empresas { get; set; }          
        public int NumeroMeses { get; set; }          
        public List<UFIndiceDTO> UFs { get; set; }
    }
}
