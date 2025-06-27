namespace Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.DTOs
{
    public class SalvarParametrizacaoRequest
    {
        public string CodEstado { get; set; } = "";
        public short? CodComarca { get; set; }
        public string Codigos { get; set; } = string.Empty;
        public short? CodVara => short.Parse(Codigos.Split("|")[0]);
        public short? CodTipoVara => short.Parse(Codigos.Split("|")[1]);
        public short? CodTipoProcesso { get; set; }
        public short? CodEmpresaCentralizadora { get; set; }
        public short? CodParamDistribuicao { get; set; }
        public bool IndAtivo { get; set; }
    }
}
