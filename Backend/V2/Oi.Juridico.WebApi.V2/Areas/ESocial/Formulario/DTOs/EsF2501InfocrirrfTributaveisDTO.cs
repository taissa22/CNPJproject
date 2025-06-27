namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs
{
    public class EsF2501InfocrirrfTributaveisDTO
    {
        public long IdEsF2501Infocrirrf { get; set; }
        public int IdF2501 { get; set; }
        public DateTime LogDataOperacao { get; set; }
        public string LogCodUsuario { get; set; } = string.Empty;
        public int? InfocrcontribTpcr { get; set; }
        public decimal? InfocrcontribVrcr { get; set; }
        public string DescricaoTpcr { get; set; } = string.Empty;
        public decimal? InfoirVrrendmolegrave13 { get; set; }
        public decimal? InfoirRrendisen65dec { get; set; }
        public decimal? InfoirVrjurosmora13 { get; set; }
        public decimal? InfoirVrprevoficial13 { get; set; }
        public decimal? InfocrcontribVrcr13 { get; set; }

        public decimal? InfoirVrrendtrib { get; set; }
        public decimal? InfoirVrrendtrib13 { get; set; }
        public decimal? InfoirVrrendmolegrave { get; set; }
        public decimal? InfoirVrrendisen65 { get; set; }
        public decimal? InfoirVrjurosmora { get; set; }
        public decimal? InfoirVrrendisenntrib { get; set; }
        public string? InfoirDescisenntrib { get; set; }
        public decimal? InfoirVrprevoficial { get; set; }

        public decimal? InfoirVlrDiarias { get; set; }
        public decimal? InfoirVlrAjudaCusto { get; set; }
        public decimal? InfoirVlrIndResContrato { get; set; }
        public decimal? InfoirVlrAbonoPec { get; set; }
        public decimal? InfoirVlrAuxMoradia { get; set; }
    }
}
