namespace Oi.Juridico.WebApi.V2.Areas.Compromissos.DTOs
{
    public class ObterAgendamentoCargaCompromissoRequest
    {
        public DateTime? DataInicioAgendamento { get; set; }
        public DateTime? DataFimAgendamento { get; set; }
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 0;
    }
}
