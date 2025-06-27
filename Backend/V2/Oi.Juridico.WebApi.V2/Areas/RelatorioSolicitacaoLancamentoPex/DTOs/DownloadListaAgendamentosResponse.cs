namespace Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.DTOs
{
    public class DownloadListaAgendamentosResponse
    {
        public decimal Id { get; set; }
        public string NomAgendamento { get; set; } = "";
        public string DscAgendamento { get; set; } = "";
        public DateTime DatCriacao { get; set; }
        public string UsrCodUsuario { get; set; } = "";
        public DateTime? DatUltExecucao { get; set; }
        public DateTime DatUltAlteracao { get; set; }
        public TimeSpan Duracao { get; set; }
        public DateTime DataProxExecucao { get; set; }
        public short  IdTipAgendamento { get; set; } = 0;
        public string TipoAgendamento { get; set; } = "";

    }
}
