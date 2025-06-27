
namespace Perlink.Oi.Juridico.Domain.Compartilhado.DTO
{
    public class OrdernacaoPaginacaoDTO
    {
        public int Pagina { get; set; }
        public int Quantidade { get; set; }
        public int Total { get; set; }
        public string Ordenacao { get; set; }
        public bool Ascendente { get; set; }
    }
}
