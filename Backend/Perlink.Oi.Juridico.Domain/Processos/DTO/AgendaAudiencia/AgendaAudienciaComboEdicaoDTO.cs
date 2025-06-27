using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia
{
    public class AgendaAudienciaComboEdicaoDTO
    {
        public IEnumerable<PrepostoDTO> ListaPrepostos{ get; set; }
        public IEnumerable<EscritorioDTO> ListaEscritorios { get; set; }

        
    }
}
