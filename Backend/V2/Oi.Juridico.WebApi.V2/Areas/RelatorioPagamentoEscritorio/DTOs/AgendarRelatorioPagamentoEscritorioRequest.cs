namespace Oi.Juridico.WebApi.V2.Areas.VEP.DTOs
{
    public class AgendarRelatorioPagamentoEscritorioRequest
    {
        public DateTime? DatEspecifica { get; set; }
        public byte PeriodicidadeExecucao { get; set; }
        public DateTime MesReferencia { get; set; }
        public byte? DiaDoMes { get; set; }
    }
}
