namespace Oi.Juridico.WebApi.V2.Areas.Compromissos.DTOs
{
    public class AgendamentoCargaCompromissoRequest
    {
        //public int CodAgendCargaComp { get; set; }
        public string TipoProcesso { get; set; }
        public string ConfigExec { get; set; }
        public int Status { get; set; }
        
        public string Mensagem { get; set; }
        //public string MensagemErroTrace { get; set; }
        //public DateTime? DatIniExec { get; set; }
        //public DateTime? DatFimExec { get; set; }
        //public string UsrCodUsuario { get; set; }
        //public string NomArquivoBase { get; set; }
        //public string EmpresaCentralizadora { get; set; }
        //public string NomArquivoGerado { get; set; }
        //public DateTime? DatSolicitacao { get; set; }
        public DateTime? DatAgendamento { get; set; }
    }
}
