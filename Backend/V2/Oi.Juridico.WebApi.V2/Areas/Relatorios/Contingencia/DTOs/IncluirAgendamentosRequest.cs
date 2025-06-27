namespace Oi.Juridico.WebApi.V2.Areas.Relatorios.Contingencia.DTOs
{
    public class IncluirAgendamentosRequest
    {
        public byte iniCodTipoOutlier { get; set; }
        public DateTime iniDataFechamento { get; set; }
        public string iniEmpresas { get; set; } = "";
        public bool iniIndFechamentoParcial { get; set; }
        public short iniNumMesesMediaHistorica { get; set; }
        public string iniIndMensal { get; set; } = "";
        public int iniValOutlier { get; set; }
        public short fimCodTipoOutlier { get; set; }
        public DateTime fimDataFechamento { get; set; }
        public string fimEmpresas { get; set; } = "";
        public bool fimIndFechamentoParcial { get; set; }
        public short fimNumMesesMediaHistorica { get; set; }
        public string fimIndMensal { get; set; } = "";
        public int fimValOutlier { get; set; }
    }
}
