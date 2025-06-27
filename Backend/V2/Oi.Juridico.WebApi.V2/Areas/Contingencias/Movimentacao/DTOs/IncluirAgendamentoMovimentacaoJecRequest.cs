namespace Oi.Juridico.WebApi.V2.Areas.Contingencia.Movimentacao.DTOs
{
    public class IncluirAgendamentoMovimentacaoJecRequest
    {
        public DateTime IniDataFechamento { get; set; }
        public DateTime FimDataFechamento { get; set; }
        public int IdBaseMovIni { get; set; }
        public string IniEmpresas { get; set; } = "";
        public bool IniIndFechamentoParcial { get; set; }
        public string IniIndMensal { get; set; } = "";
        public short IniNumMesesMediaHistorica { get; set; }
        public decimal IniPercentualHaircut { get; set; }
        public decimal IniValCorteOutliers { get; set; }
        public int IdBaseMovFim { get; set; }
        public string FimEmpresas { get; set; } = "";
        public bool FimIndFechamentoParcial { get; set; }
        public string FimIndMensal { get; set; } = "";
        public short FimNumMesesMediaHistorica { get; set; }
        public decimal FimPercentualHaircut { get; set; }
        public decimal FimValCorteOutliers { get; set; }
    }
}
