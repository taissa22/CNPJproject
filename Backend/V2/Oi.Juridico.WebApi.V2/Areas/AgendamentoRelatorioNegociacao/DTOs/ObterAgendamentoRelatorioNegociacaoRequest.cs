namespace Oi.Juridico.WebApi.V2.Areas.AgendamentoRelatorioNegociacao.DTOs
{
    public class ObterAgendamentoRelatorioNegociacaoRequest
    {
        public DateTime? DataInicioAgendamento { get; set; }
        public DateTime? DataFimAgendamento { get; set; }
        public int Page { get; set; } = 0;
    }
}
