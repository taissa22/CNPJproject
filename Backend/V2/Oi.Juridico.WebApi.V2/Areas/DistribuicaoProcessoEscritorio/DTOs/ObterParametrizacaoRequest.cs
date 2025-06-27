namespace Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.DTOs
{
    public class ObterParametrizacaoRequest
    {
        public string CodEstado { get; set; } = "";
        public int CodComarca { get; set; }
        public string Codigos { get; set; } = "";
        public int CodVara => int.Parse(Codigos.Split("|")[0]);
        public int CodTipoVara => int.Parse(Codigos.Split("|")[1]);
        public int CodTipoProcesso { get; set; }
        public int CodEmpresaCentralizadora { get; set; }
        public int CodProfissional { get; set; }
    }
}
