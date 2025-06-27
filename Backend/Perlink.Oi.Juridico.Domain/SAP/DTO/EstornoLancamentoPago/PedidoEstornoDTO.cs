using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class PedidoEstornoDTO
    {
        public long CodigoProcesso { get; set; }
        public long CodigoPedido { get; set; }
        public string DescricaoPedido { get; set; }
        public string CodigoRiscoPerda { get; set; }
        public string NumeroContrato { get; set; }

    }
}
