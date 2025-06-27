namespace Oi.Juridico.WebApi.V2.Areas.PautaJuizado.DTOs
{
    public class ListarPautaComposicaoResponse
    {
        public string Data { get; set; }
        public DateTime Dat { get; set; }
        public string Juizado { get; set; }
        public short CodComarca { get; set; }
        public short CodVara { get; set; }
        public short  CodTipoVara { get; set; }
        public string CodEstado { get; set; }
        public short CodTipoAudiencia { get; set; }
        public short CodStatusAudiencia { get; set; }
        public int CodEmpresaGrupo { get; set; }
        public short CodEmpresaCentralizadora { get; set; }
        public short CodGrupoJuizado { get; set; }
    }
}
