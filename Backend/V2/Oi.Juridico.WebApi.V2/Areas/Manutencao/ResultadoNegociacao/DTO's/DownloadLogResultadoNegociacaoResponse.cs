namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.ResultadoNegociacao.DTO_s
{
    public class DownloadLogResultadoNegociacaoResponse
    {
        public int? CodResultadoNegociacao { get; set; }
        public string Operacao { get; set; } = string.Empty;
        public DateTime? DatLog { get; set; }
        public string CodUsuario { get; set; } = string.Empty;
        public string DscResultadoNegociacaoA { get; set; } = string.Empty;
        public string DscResultadoNegociacaoD { get; set; } = string.Empty;
        public string IndNegociacaoA { get; set; } = string.Empty;
        public string IndNegociacaoD { get; set; } = string.Empty; 
        public string IndPosSentencaA { get; set; } = string.Empty;
        public string IndPosSentencaD { get; set; } = string.Empty;
        public string? CodTipoResultadoA { get; set; } = string.Empty;
        public string? CodTipoResultadoD { get; set; } = string.Empty;
        public string IndAtivoA { get; set; } = string.Empty;
        public string IndAtivoD { get; set; } = string.Empty;
    }
}
