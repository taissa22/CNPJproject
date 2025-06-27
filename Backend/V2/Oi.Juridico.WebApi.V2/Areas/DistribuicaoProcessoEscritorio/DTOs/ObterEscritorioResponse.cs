namespace Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.DTOs
{
    public class ObterEscritorioResponse
    {
        public int CodParamDistribuicao { get; set; }
        public int CodProfissional { get; set; }
        public string NomProfissional { get; set; } = "";
        public int CodSolicitante { get; set; }
        public string NomSolicitante { get; set; } = "";
        public DateTime DatVigenciaInicial { get; set; }
        public DateTime DatVigenciaFinal { get; set; }
        public string PorcentagemProcessos { get; set; } = string.Empty;
        public short Prioridade { get; set; }
        public decimal PorcentagemProcessosNumerico { get; set; }
        public long CodParamDistribEscrit { get; set; }
    }
}
