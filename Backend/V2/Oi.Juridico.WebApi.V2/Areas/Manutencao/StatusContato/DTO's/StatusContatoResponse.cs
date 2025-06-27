namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.StatusContato.DTO_s
{
    public class StatusContatoResponse
    {
        public byte CodStatusContato { get; set; }
        public string DscStatusContato { get; set; } = string.Empty;
        public string IndContatoRealizado { get; set; } = string.Empty;
        public string IndAcordoRealizado { get; set; } = string.Empty;
        public string IndAtivo { get; set; } = string.Empty;
    }
}
