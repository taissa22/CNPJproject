using System;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public  class LancamentoViewModel
    {
        public long Id { get; set; }
        public string NumeroProcesso { get; set; }
        public string Comarca { get; set; }
        public long Vara { get; set; }
        public DateTime DataEnvioEscritorio { get; set; }
        public string Escritorio { get; set; }
        public string TipoLancamento { get; set; }
        public string CategoriaPagamento { get; set; }
        public string StatusPagamento { get; set; }
        public DateTime DataLancamento { get; set; }
        public  long NumeroPedidoSAP { get; set; }
        public DateTime DataRecebimentoFiscal { get; set; }
        public DateTime PagamentoPedido { get; set; }
        public  double ValorLiquido { get; set; }
        public  string TextoSAP { get; set; }
        public string Comentario { get; set; }
        public long NumeroGuia { get; set; }
        public string Autor { get; set; }
        public long NumeroContaJudicial { get; set; }
        public long NumeroParcelaJudicial { get; set; }
        public long AutenticacaoEletronica { get; set; }
        public long StatusParcela { get; set; }
        public DateTime DataEfetivacaoParcela { get; set; }
    }
}
