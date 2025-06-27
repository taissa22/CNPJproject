using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia
{
    public class AgendaAudienciaPartesDTO
    {
        public IEnumerable<PartesDTO> Autores { get; set; }
        public IEnumerable<PartesDTO> Reus { get; set; }
    }
}
