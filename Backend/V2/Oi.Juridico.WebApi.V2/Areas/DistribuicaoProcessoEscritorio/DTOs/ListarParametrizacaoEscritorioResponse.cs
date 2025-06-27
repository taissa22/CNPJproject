using Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.DTOs
{
    public class ListarParametrizacaoEscritorioResponse
    {
        public int Codigo { get; set; }
        public string CodEstado { get; set; } = "";
        public short? CodComarca { get; set; }
        public string NomComarca { get; set; } = "";
        public short? CodVara { get; set; }
        public short? CodTipoVara { get; set; }
        public string? NomTipoVara { get; set; } = "";
        public short? CodTipoProcesso { get; set; }
        public string DscTipoProcesso { get; set; } = "";
        public short? CodEmpresaCentralizadora { get; set; }
        public string NomEmpresaCentralizadora { get; set; } = "";
        public string Status { get; set; } = "";
        public int CodParamDistribuicao { get; set; }
        public int CodProfissional { get; set; }
        public string NomProfissional { get; set; } = "";
        public int CodSolicitante { get; set; }
        public string NomSolicitante { get; set; } = "";
        public DateTime DatVigenciaInicial { get; set; }
        public DateTime DatVigenciaFinal { get; set; }
        public decimal PorcentagemProcessos { get; set; }
        public short Prioridade { get; set; }
        public ICollection<ParamDistribEscritorio> ParamDistribuicaoEscritorio { get; set; } = new List<ParamDistribEscritorio>();
    }
}
