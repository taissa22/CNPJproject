namespace Oi.Juridico.WebApi.V2.Areas.AgendamentoRelatorioNegociacao.DTOs
{
    public class AgendamentoRelatorioNegociacaoRequest
    {
        public DateTime? DatProxExec { get; set; }
        public bool IndProcessoCc { get; set; }
        public bool IndProcessoJec { get; set; }
        public bool IndProcessoProcon { get; set; }
        public byte PeriodicidadeExecucao { get; set; }
        public byte? DiaDaSemana { get; set; }
        public bool IndUltimoDiaMes { get; set; }
        public byte? DiaDoMes { get; set; }
        public DateTime? DatInicioNegociacao { get; set; }
        public DateTime? DatFimNegociacao { get; set; }
        public byte? PeriodoSemanal { get; set; }
        public byte? PeriodoMensal { get; set; }
        public bool IndNegociacoesAtivas { get; set; }
    }
}
