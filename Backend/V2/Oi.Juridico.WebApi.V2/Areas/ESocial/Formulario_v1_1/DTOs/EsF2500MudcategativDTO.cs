namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs
{
    public class EsF2500MudcategativDTO
    {
        public long IdEsF2500Infocontrato { get; set; }
        public DateTime? LogDataOperacao { get; set; }
        public string LogCodUsuario { get; set; } = string.Empty;
        public short? MudcategativCodcateg { get; set; }
        public byte? MudcategativNatatividade { get; set; }
        public string MudcategativNatatividadeConcatenada { get; set; }
        public DateTime? MudcategativDtmudcategativ { get; set; }
        public long IdEsF2500Mudcategativ { get; internal set; }
        public string DescricaoNaturezaDeAtividade { get; set; }
        public string DescricaoCodCategoria { get; set; }

    }
}
