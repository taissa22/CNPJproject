namespace Oi.Juridico.WebApi.V2.Areas.Relatorios.Contingencia.DTOs
{
    public class ListarFechamentosResponse
    {
        public DateTime DataFechamento { get; set; }
        public DateTime? DataExecucao { get; set; }
        public short NumeroMeses { get; set; }
        public string IndFechamentoMensal { get; set; } = "";
        public string EmpresasGrupo { get; set; } = "";
        public bool? IndFechamentoParcial { get; set; }
        public byte CodTipoOutlier { get; set; }
        public decimal? ValOutlier { get; set; }
    }
}
