namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs
{
    public class EsF2501InfocrcontribDTO
    {
        public long IdEsF2501Infocrcontrib { get; set; }
        public long IdEsF2501Calctrib { get; set; }
        public DateTime LogDataOperacao { get; set; }
        public string LogCodUsuario { get; set; } = string.Empty;
        public int? InfocrcontribTpcr { get; set; }
        public decimal? InfocrcontribVrcr { get; set; }
        public string DescricaoTpcr { get; set; } = string.Empty;
    }
}
