namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs
{
    public class RetornoPaginadoDTO<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Lista { get; set; }
        public bool ComCritica { get; set; }

    }
}
