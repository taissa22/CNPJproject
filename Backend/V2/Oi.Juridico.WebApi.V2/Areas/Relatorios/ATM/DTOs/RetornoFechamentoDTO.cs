namespace Oi.Juridico.WebApi.V2.Areas.Relatorios.ATM.DTOs
{
    public class RetornoFechamentoDTO
    {
        public decimal? Id { get; set; }
        public decimal? CodSolicFechamentoCont { get; set; }
        public DateTime DataFechamento { get; set; }
        public string NomeUsuario { get; set; }
        public long NumeroMeses { get; set; }  
        public string Empresas { get; set; }
        public DateTime DataExecucao { get; set; }
        public DateTime? MesAnoFechamento { get; set; }
        public DateTime? DataAgendamento { get; set; }
    }
}
