namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs
{
    public class EsF2501DedDepenDTO
    {
        public long IdEsF2501Infocrirrf { get; set; }
        public long IdEsF2501Deddepen { get; set; }
        public string? LogCodUsuario { get; set; }
        public DateTime LogDataOperacao { get; set; }
        public byte? DeddepenTprend { get; set; }
        public string? DeddepenCpfdep { get; set; }
        public decimal? DeddepenVlrdeducao { get; set; }
        public string? DescricaoTipoRend { get; set; }
    }
}
