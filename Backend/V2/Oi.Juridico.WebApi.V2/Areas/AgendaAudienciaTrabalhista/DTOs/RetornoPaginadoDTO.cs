namespace Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.DTOs
{
    public class RetornoPaginadoDTO<T>
    {
        public int Total { get; set; }
        public int TotalGeral { get; set; }
        public IEnumerable<T>? Lista { get; set; }
        public IEnumerable<RetornoListaUfDTO>? ListaEstados { get; set; }

    }
}
