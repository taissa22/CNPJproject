namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.StatusContato.DTO_s
{
    public class StatusContatoRequest
    {
        public byte CodStatusContato { get; set; }
        public string DscStatusContato { get; set; } = string.Empty;
        public bool? IndContatoRealizado { get; set; }
        public bool? IndAcordoRealizado { get; set; }
        public bool IndAtivo { get; set; }
    }
}
