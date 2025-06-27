using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO {
    public class AgendamentoApuracaoValoresCalculadosDTO {
        public long Id { get; set; }
        public decimal MediaPagamentos { get; set; }
        public decimal DesvioPadrao { get; set; }
        public decimal ValorTotalPagamentos { get; set; }
        public long QtdProcessosPagamentos { get; set; }
        public decimal ValorCorteOutliers { get; set; }
    }
}
