using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class LoteCancelamentoDTO
    {
        public long CodigoTipoProcesso { get; set; }
        public long CodigoLote { get; set; } 
        public byte? OpcaoCancelamento { get; set; }
    }
}
