namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.ResultadoNegociacao.DTO_s
{
    public class ListaResultadoNegociacaoResponse
    {
        public int CodResultadoNegociacao { get; set; }
        public string DscResultadoNegociacao { get; set; } = string.Empty;
        public string DscTipoNegociacao { get; set; } = string.Empty;
        public string DscTipoResultado { get; set; } = string.Empty;
        public string IndAtivo { get; set; } = string.Empty;
    }
}
