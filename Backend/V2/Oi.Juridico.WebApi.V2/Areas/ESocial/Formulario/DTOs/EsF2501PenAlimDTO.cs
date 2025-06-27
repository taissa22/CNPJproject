namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs
{
    public class EsF2501PenAlimDTO
    {
        public long IdEsF2501Infocrirrf { get; set; }
        public long IdEsF2501Penalim { get; set; }
        public string? LogCodUsuario { get; set; }
        public DateTime LogDataOperacao { get; set; }
        public byte? PenalimTprend { get; set; }
        public string? PenalimCpfdep { get; set; }
        public decimal? PenalimVlrpensao { get; set; }
        public string? DescricaoTipoRend { get; set; }
    }
}
