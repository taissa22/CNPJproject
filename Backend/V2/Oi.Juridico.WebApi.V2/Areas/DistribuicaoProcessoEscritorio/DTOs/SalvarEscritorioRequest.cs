using Humanizer.Localisation;
using Newtonsoft.Json;

namespace Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.DTOs
{
    public class SalvarEscritorioRequest
    {
        public long CodParamDistribEscrit { get; set; }
        public int CodParamDistribuicao { get; set; }
        public int CodProfissional { get; set; }
        public int CodSolicitante { get; set; }
        public DateTime DatVigenciaInicial { get; set; }
        public DateTime DatVigenciaFinal { get; set; }
        public decimal PorcentagemProcessos { get; set; }
        public short Prioridade { get; set; }
    }
}
