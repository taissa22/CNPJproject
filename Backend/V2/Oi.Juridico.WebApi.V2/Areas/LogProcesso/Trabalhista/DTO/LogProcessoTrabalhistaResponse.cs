namespace Oi.Juridico.WebApi.V2.Areas.LogProcesso.Trabalhista.DTO
{
    public class LogProcessoTrabalhistaResponse
    {

        public string CodUsuarioUltAlteracao { get; set; } = string.Empty;
        public string? NomPrepostoAntes { get; set; } = string.Empty;
        public string? NomPrepostoDepois { get; set; } = string.Empty;
        public string Operacao { get; set; } = string.Empty;
        public string DatLog { get; set; } = string.Empty;
        public string? DatAudiencia { get; set; }
        public string? HoraAudiencia { get; set; }
        public string? NomReclamadaAntes { get; set; }
        public string? NomReclamadaDepois { get; set; }
        public string NomUsuario { get; set; } = string.Empty;
        public string IndUsuarioInternet { get; set; } = string.Empty;

    }
}
