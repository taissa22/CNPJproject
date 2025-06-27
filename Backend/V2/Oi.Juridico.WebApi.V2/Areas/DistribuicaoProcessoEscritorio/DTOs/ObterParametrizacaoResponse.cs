namespace Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.DTOs
{
    public class ObterParametrizacaoResponse
    {
        public int Codigo { get; set; }
        public string CodEstado { get; set; } = "";
        public short? CodComarca { get; set; }
        public string NomComarca { get; set; } = "";
        public short? CodVara { get; set; }
        public short? CodTipoVar { get; set; }
        public string? NomTipoVara { get; set; } = "";
        public short? CodTipoProcesso { get; set; }
        public string DscTipoProcesso { get; set; } = "";
        public short? CodEmpresaCentralizadora { get; set; }
        public string NomEmpresaCentralizadora { get; set; } = "";
        public string Status { get; set; } = "";
        public string Codigos { get; set; } = "";
    }
}
