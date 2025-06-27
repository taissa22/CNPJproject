namespace Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.DTOs
{
    public class SalvarParametrizacaoEmLoteRequest
    {
        public SalvarParametrizacaoRequest[] Parametrizacoes { get; set; } = Array.Empty<SalvarParametrizacaoRequest>();
        public SalvarEscritorioRequest[] Escritorios { get; set; } = Array.Empty<SalvarEscritorioRequest>();
        public int[] Anexos { get; set; } = Array.Empty<int>();
    }
}
