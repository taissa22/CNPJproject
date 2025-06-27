namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs
{
    public class EsF2501DedSuspDTO
    {
        public long IdEsF2501Infovalores { get; set; }
        public decimal IdEsF2501Dedsusp { get; set; }
        public string? LogCodUsuario { get; set; }
        public DateTime? LogDataOperacao { get; set; }
        public byte? DedsuspIndtpdeducao { get; set; }
        public decimal? DedsuspVlrdedsusp { get; set; }
    }
}
