namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs
{
    public class EsF2501InfoRraDTO
    {
        public long IdEsF2501Infocrirrf { get; set; }
        public int IdF2501 { get; set; }
        public DateTime LogDataOperacao { get; set; }
        public string LogCodUsuario { get; set; } = string.Empty;
        public string? InforraDescrra { get; set; }
        public decimal? InforraQtdmesesrra { get; set; }
        public decimal? DespprocjudVlrdespcustas { get; set; }
        public decimal? DespprocjudVlrdespadvogados { get; set; }
    }
}
