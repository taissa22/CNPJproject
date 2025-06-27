namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs
{
    public class DownloadRetornoDTO
    {
        public byte? StatusFormulario { get; set; }
        public string? InfoprocessoNrproctrab { get; set; }
        public string? CpfParte { get; set; }
        public string? TipoFormulario { get; set; }
        public byte[]? Dados { get; set; }

    }
}
