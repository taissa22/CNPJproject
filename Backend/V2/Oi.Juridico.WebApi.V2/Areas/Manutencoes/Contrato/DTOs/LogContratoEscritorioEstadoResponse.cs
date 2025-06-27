namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.DTOs
{
    public class LogContratoEscritorioEstadoResponse
    {
        public long? CodContratoEscritorio { get; set; }
        public string? DatLog { get; set; }
        public string? Operacao { get; set; }
        public string ?CodUsuario { get; set; }
        public int? CodProfissionalA { get; set; }
        public int? CodProfissionalD { get; set; }
        public string? CodEstadoA { get; set; }
        public string? CodEstadoD { get; set; }
    }
}
