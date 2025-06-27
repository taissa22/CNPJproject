namespace Oi.Juridico.WebApi.V2.Areas.VEP.DTOs
{
    public class AgendarCalculoVepRequest
    {
        public DateTime? DatEspecifica { get; set; }
        public byte PeriodicidadeExecucao { get; set; }
        public byte NumMeses { get; set; }
        public bool IndUltimoDiaDoMes { get; set; }
        public byte? DiaDoMes { get; set; }
    }
}
