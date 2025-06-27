namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs
{
    public class EsF2501InfocrirrfDTO
    {
        public long IdEsF2501Infocrirrf { get; set; }
        public int IdF2501 { get; set; }
        public DateTime LogDataOperacao { get; set; }
        public string LogCodUsuario { get; set; } = string.Empty;
        public int? InfocrcontribTpcr { get; set; }
        public decimal? InfocrcontribVrcr { get; set; }
        public string DescricaoTpcr { get; set; } = string.Empty;
        public decimal? InfocrcontribVrcr13 { get; set; }
    }
}
