namespace Oi.Juridico.WebApi.V2.Areas.Contingencia.Movimentacao.DTOs
{
    public class ObterFechamentosJecResponse
    {
        public int Id { get; internal set; }
        public DateTime DataFechamento { get; internal set; }
        public decimal? PercHaircut { get; internal set; }
        public decimal? ValorCorteOutliers { get; internal set; }
        public byte NumeroMeses { get; internal set; }
        public string Empresas { get; internal set; } = string.Empty;
        public string IndicaFechamentoMensal { get; internal set; } = string.Empty;
        public bool IndicaFechamentoParcial { get; internal set; }
    }
}
