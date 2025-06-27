namespace Oi.Juridico.WebApi.V2.Areas.VEP.DTOs
{
    public class AgendarRelatorioPagamentoEscritorioResponse
    {
        public long Cod { get; set; }
        public DateTime DatAgendamento { get; set; }
        public DateTime? DatInicioExecucao { get; set; }
        public DateTime? DatFimExecucao { get; set; }
        public DateTime? DatProximaExecucao { get; set; }
        public string UsrCodUsuario { get; set; } = string.Empty;
        public byte PeriodicidadeExecucao { get; set; }
        public DateTime MesReferencia { get; set; }
        public byte? DiaDoMes { get; set; }
        public byte Status { get; set; }
    }
}
