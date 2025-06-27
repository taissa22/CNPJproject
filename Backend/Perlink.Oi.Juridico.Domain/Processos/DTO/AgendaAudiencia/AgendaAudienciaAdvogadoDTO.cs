using Perlink.Oi.Juridico.Domain.SAP.DTO;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia
{
    public class AgendaAudienciaAdvogadoDTO
    {
        public IEnumerable<AdvogadoEscritorioDTO> ListaAdvogados { get; set; }
    }
}
