namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs
{
    public class EsF2500ObservacoesDTO
    {
        public long IdEsF2500Infocontrato { get; set; }
        public DateTime? LogDataOperacao { get; set; }
        public string LogCodUsuario { get; set; } = string.Empty;
        public string ObservacoesObservacao { get; set; } = string.Empty;
        public long IdEsF2500Observacoes { get; internal set; }
    }
}
