using System;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class LoteLancamentoDTO
    {
        public long Id { get; set; }
        public string NumeroProcesso { get; set; }
        public string NomeComarca { get; set; }
        public string Vara { get; set; }
        public string  CodigoVara { get; set; }
        public string NomeTipoVara { get; set; }
        public string DataEnvioEscritorio { get; set; }
        public string Escritorio { get; set; }
        public string TipoLancamento { get; set; }
        public string CategoriaPagamento { get; set; }
        public string StatusPagamento { get; set; }
        public string DataLancamento { get; set; }  
        public string NumeroGuia  { get; set; }
        public string NumeroPedidoSAP  { get; set; }
        public string DataRecebimentoFiscal { get; set; }
        public string DataPagamentoPedido { get; set; }
        public string ValorLiquido { get; set; }
        public string Comentario { get; set; }
        public string TextoSAP { get; set; }
        public string Autor { get; set; }
        public string NumeroContaJudicial { get; set; }
        public string NumeroParcelaJudicial { get; set; }
        public string AutenticacaoEletronica { get; set; }
        public string StatusParcelaBancoDoBrasil { get; set; }
        public string DataEfetivacaoParcelaBancoDoBrasil { get; set; }
        public string Uf { get; set; }
        public bool LancamentoEstornado { get; set; }

        public long CodigoProcesso { get; set; }
        public long CodigoLancamento { get; set; }
    }

    public class LoteLancamentoExportacaoDTO {
        public long Id { get; set; }
        public string NumeroProcesso { get; set; }
        public string NomeComarca { get; set; }
        public string Vara { get; set; }
        public string CodigoVara { get; set; }
        public string NomeTipoVara { get; set; }
        public DateTime? DataEnvioEscritorio { get; set; }
        public string Escritorio { get; set; }
        public string TipoLancamento { get; set; }
        public string CategoriaPagamento { get; set; }
        public string StatusPagamento { get; set; }
        public string DataLancamento { get; set; }
        public string NumeroGuia { get; set; }
        public string NumeroPedidoSAP { get; set; }
        public string DataRecebimentoFiscal { get; set; }
        public string DataPagamentoPedido { get; set; }
        public string ValorLiquido { get; set; }
        public string Comentario { get; set; }
        public string TextoSAP { get; set; }
        public string Autor { get; set; }
        public string NumeroContaJudicial { get; set; }
        public string NumeroParcelaJudicial { get; set; }
        public string AutenticacaoEletronica { get; set; }
        public string StatusParcelaBancoDoBrasil { get; set; }
        public string DataEfetivacaoParcelaBancoDoBrasil { get; set; }
        public string Uf { get; set; }
        public string CodigoLote { get; set; }
        public string CodigoProcesso { get; set; }
        public string CodigoLancamento { get; set; }
        public string CodigoComarca { get; set; }
        public string CodigoTipoVara { get; set; }
        public string CodigoTipoLancamento { get; set; }
        public string CodigoCategoriaPagamento { get; set; }
        public string QuantidadeLancamento { get; set; }
        public string DataCriacaoPedido { get; set; }
        public string DataRecebimentoFisico { get; set; }
        public string CodigoUsuarioRecebedor { get; set; }
        public string IndicaExcluido { get; set; }
        public string CodigoProfissional { get; set; }
        public string CodigoStatusPagamento { get; set; }
    }

}
