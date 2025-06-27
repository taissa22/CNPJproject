namespace Oi.Juridico.WebApi.V2.Areas.AgendamentoCargaCompromissos.DTOs
{
    public class AgendamentoCargaCompromissoResponse
    {
        public int CodAgendCargaComp { get; set; }
        public string MensagemConfigExec { get; set; } = string.Empty;
        public string MensagemPeriodoExec { get; set; } = string.Empty;
        public string MensagemTipoProcesso { get; set; } = string.Empty;
        public string MensagemStatusProcesso { get; set; } = string.Empty;
        public string MensagemPeriodicidade { get; set; } = string.Empty;
        public string MensagemUsrDatSolicitante { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string MensagemErroTrace { get; set; } = string.Empty;


        public string TipoProcesso { get; set; }
        public string ConfigExec { get; set; }
        //public byte Status { get; set; }
        public string Mensagem { get; set; }
        //public string MensagemErroTrace { get; set; }
        public DateTime? DatIniExec { get; set; }
        public DateTime? DatFimExec { get; set; }
        public string UsrCodUsuario { get; set; }
        public string NomArquivoBase { get; set; }
        public string EmpresaCentralizadora { get; set; }
        public string NomArquivoGerado { get; set; }
        public DateTime? DatSolicitacao { get; set; }
        public DateTime DatAgendamento { get; set; }
    }
}
