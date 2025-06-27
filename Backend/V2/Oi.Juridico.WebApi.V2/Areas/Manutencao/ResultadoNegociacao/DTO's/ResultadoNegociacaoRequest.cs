namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.ResultadoNegociacao.DTO_s
{
    public class ResultadoNegociacaoRequest
    {
        public int CodResultadoNegociacao { get; set; }
        public string DscResultadoNegociacao { get; set; } = string.Empty;
        public bool IndNegociacao { get; set; }
        public bool IndPosSentenca { get; set; }
        public byte CodTipoResultado { get; set; }
        public bool IndAtivo { get; set; }
    }
}
