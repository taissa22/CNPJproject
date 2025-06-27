using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO {
    public class FechamentoJecDisponivelDTO {
        public DateTime DataFechamento { get; set; }
        public DateTime MesAnoFechamento { get; set; }
        public int NumeroMeses { get; set; }
        public long CodigoEmpresaCentralizadora { get; set; }
        public bool IndFechamentoMensal { get; set; }
    }
}
