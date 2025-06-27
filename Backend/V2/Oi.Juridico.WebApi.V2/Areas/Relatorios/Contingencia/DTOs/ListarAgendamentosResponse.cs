namespace Oi.Juridico.WebApi.V2.Areas.Relatorios.Contingencia.DTOs
{
    public class ListarAgendamentosResponse
    {
        public decimal Id { get; set; }
        public DateTime DatAgendamento { get; set; }
        public DateTime IniDataFechamento { get; set; }
        public short IniNumMesesMediaHistorica { get; set; }
        public byte IniCodTipoOutlier { get; set; }
        public decimal IniValOutlier { get; set; }
        public string IniIndMensal { get; set; } = "";
        public bool IniIndFechamentoParcial { get; set; }
        public string IniEmpresas { get; set; } = "";
        public DateTime FimDataFechamento { get; set; }
        public short FimNumMesesMediaHistorica { get; set; }
        public short FimCodTipoOutlier { get; set; }
        public decimal FimValOutlier { get; set; }
        public string FimIndMensal { get; set; } = "";
        public bool FimIndFechamentoParcial { get; set; }
        public string FimEmpresas { get; set; } = "";
        public DateTime? DatInicioExecucao { get; set; }
        public DateTime? DatFimExecucao { get; set; }
        public string UsrCodUsuario { get; set; } = "";
        public byte Status { get; set; }
        public string MsgErro { get; set; } = "";
        public string NomArquivo { get; set; } = "";
    }
}
