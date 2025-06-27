namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.ResultadoNegociacao.DTO_s
{
    public class ResultadoNegociacaoResponse
    {
        public int CodResultadoNegociacao { get; set; }
        public string DscResultadoNegociacao { get; set; } = string.Empty;
        public string IndNegociacao { get; set; } = string.Empty;
        public string IndPosSentenca { get; set; } = string.Empty;
        public byte CodTipoResultado { get; set; }
        public string IndAtivo { get; set; } = string.Empty;
    }
}
