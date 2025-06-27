namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs
{
    public class EsF2501InfoValoresDTO
    {
        public long IdEsF2501Infoprocret { get; set; }
        public long IdEsF2501Infovalores { get; set; }
        public string? LogCodUsuario { get; set; }
        public DateTime LogDataOperacao { get; set; }
        public int? InfovaloresIndapuracao { get; set; }
        public decimal? InfovaloresVlrnretido { get; set; }
        public decimal? InfovaloresVlrdepjud { get; set; }
        public decimal? InfovaloresVlrcmpanocal { get; set; }
        public decimal? InfovaloresVlrcmpanoant { get; set; }
        public decimal? InfovaloresVlrrendsusp { get; set; }
    }
}
