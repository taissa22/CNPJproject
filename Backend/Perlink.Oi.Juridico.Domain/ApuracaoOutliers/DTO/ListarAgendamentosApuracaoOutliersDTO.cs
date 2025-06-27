using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO {
    public class ListarAgendamentosApuracaoOutliersDTO {
        public long Id { get; set; }
        public long CodigoEmpresaCentralizadora { get; set; }
        public DateTime MesAnoFechamento { get; set; }
        public DateTime DataFechamento { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public int NumeroMeses { get; set; }
        public decimal FatorDesvioPadrao { get; set; }
        public string Observacao { get; set; }
        public string NomeUsuario { get; set; }
        public string ArquivoBaseFechamento { get; set; }
        public string ArquivoResultado { get; set; }
        public AgendarApuracaoOutliersStatusEnum Status { get; set; }
        public string MgsStatusErro { get; set; }
        public string Descricao { get; set; }
        public bool IndFechamentoMensal { get; set; }
    }
}
