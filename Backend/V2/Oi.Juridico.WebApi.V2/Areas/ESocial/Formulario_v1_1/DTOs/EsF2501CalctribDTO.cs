namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs
{
    public class EsF2501CalctribDTO
    {
        public long IdEsF2501Calctrib { get; set; }
        public int IdEsF2501 { get; set; }
        public string LogCodUsuario { get; set; } = string.Empty;
        public DateTime LogDataOperacao { get; set; }
        public string CalctribPerref { get; set; } = string.Empty;
        public decimal? CalctribVrbccpmensal { get; set; }
        public decimal? CalctribVrbccp13 { get; set; }
        public decimal? CalctribVrrendirrf { get; set; }
        public decimal? CalctribVrrendirrf13 { get; set; }
    }
}
