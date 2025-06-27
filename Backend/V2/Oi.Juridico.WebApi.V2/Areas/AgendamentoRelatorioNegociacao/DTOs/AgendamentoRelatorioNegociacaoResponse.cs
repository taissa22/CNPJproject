namespace Oi.Juridico.WebApi.V2.Areas.AgendamentoRelatorioNegociacao.DTOs
{
    public class AgendamentoRelatorioNegociacaoResponse
    {
        public int CodAgendExecRelNegociacao { get; set; }
        public string MensagemConfigExec { get; set; } = string.Empty;
        public string MensagemPeriodoExec { get; set; } = string.Empty;
        public string MensagemTipoProcesso { get; set; } = string.Empty;
        public string MensagemStatusProcesso { get; set; } = string.Empty;
        public string MensagemPeriodicidade { get; set; } = string.Empty;
        public string MensagemUsrDatSolicitante { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string MensagemErroTrace { get; set; } = string.Empty;
    }
}
