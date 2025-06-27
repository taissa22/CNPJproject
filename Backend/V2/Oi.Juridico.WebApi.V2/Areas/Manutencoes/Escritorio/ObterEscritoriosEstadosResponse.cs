namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Escritorio
{
    public class ObterEscritoriosEstadosResponse
    {
        public ObterEscritoriosEstadosResponse(string estado, bool selecionado, int tipoProcessoId)
        {
            Id = estado;
            Selecionado = selecionado;
            TipoProcessoId = tipoProcessoId;
        }

        public string Id { get; set; }
        public bool Selecionado { get; set; }
        public int TipoProcessoId { get; set; }
    }
}
