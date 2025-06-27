using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao {
    public class LoteCriacaoResultadoEmpresaCentralizadoraDTO {
        public long CodigoEmpresaCentralizadora { get; set; }
        public string DescricaoEmpresaCentralizadora { get; set; }
        public int TotalLote { get; set; }
    }
}
