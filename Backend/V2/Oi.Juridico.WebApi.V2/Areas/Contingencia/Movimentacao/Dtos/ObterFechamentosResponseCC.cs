
using Newtonsoft.Json;

namespace Oi.Juridico.WebApi.V2.Areas.Contingencia.Movimentacao.Dtos
{
    public class ObterFechamentosResponseCC
    {
        public DateTime? DataExecucao { get; internal set; }
        public DateTime DataFechamento { get; internal set; }
        public decimal? PercHaircut { get; internal set; }
        public short NumeroMeses { get; internal set; }       
        public string Empresas { get; internal set; } = string.Empty;
        public string IndicaFechamentoMensal { get; internal set; } = string.Empty;
        public bool? IndicaFechamentoParcial { get; internal set; }
    }
}
