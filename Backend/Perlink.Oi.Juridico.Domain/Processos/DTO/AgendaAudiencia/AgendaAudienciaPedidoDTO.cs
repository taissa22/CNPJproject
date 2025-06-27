using Perlink.Oi.Juridico.Domain.SAP.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia
{
    public class AgendaAudienciaPedidoDTO
    {
        public IEnumerable<PedidoEstornoDTO> Pedidos { get; set; }
    }
}
