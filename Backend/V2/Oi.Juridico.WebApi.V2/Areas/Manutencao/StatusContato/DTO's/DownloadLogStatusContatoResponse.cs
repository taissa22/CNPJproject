namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.StatusContato.DTO_s
{
    public class DownloadLogStatusContatoResponse
    {
        public int? CodStatusContato { get; set; }
        public string Operacao { get; set; } = string.Empty;
        public DateTime? DatLog { get; set; }
        public string CodUsuario { get; set; } = string.Empty;
        public string DscStatusContatoA { get; set; } = string.Empty;
        public string DscStatusContatoD { get; set; } = string.Empty;
        public string IndContatoRealizadoA { get; set; } = string.Empty;
        public string IndContatoRealizadoD { get; set; } = string.Empty;
        public string IndAcordoRealizadoA { get; set; } = string.Empty;
        public string IndAcordoRealizadoD { get; set; } = string.Empty;
        public string IndAtivoA { get; set; } = string.Empty;
        public string IndAtivoD { get; set; } = string.Empty;
    }
}
