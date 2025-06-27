namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel
{
    public class FormaPagamentoViewModel
    {
        public long CodigoFormaPagamento { get; set; }
        public string DescricaoFormaPagamento { get; set; }
        public bool IndicaBordero { get; set; }
        public bool IndicaDadosBancarios { get; set; }
        public bool IndicaRestrita { get; set; }
    }
}
