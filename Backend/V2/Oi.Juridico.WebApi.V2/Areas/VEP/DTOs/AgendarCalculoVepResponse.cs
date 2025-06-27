namespace Oi.Juridico.WebApi.V2.Areas.VEP.DTOs
{
    public class AgendarCalculoVepResponse
    {
        public int Cod { get; set; }
        public DateTime DatAgendamento { get; set; }
        public DateTime? DatInicioExecucao { get; set; }
        public DateTime? DatFimExecucao { get; set; }
        public DateTime? DatProximaExecucao { get; set; }
        public string UsrCodUsuario { get; set; } = string.Empty;
        public byte PeriodicidadeExecucao { get; set; }
        public byte NumMeses { get; set; }
        public string IndUltimoDiaDoMes { get; set; } = string.Empty;
        public byte? DiaDoMes { get; set; }
        public byte Status { get; set; }
    }
}
