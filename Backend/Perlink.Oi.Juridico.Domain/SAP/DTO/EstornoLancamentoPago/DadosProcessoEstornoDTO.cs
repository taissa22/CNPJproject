using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.EstornoLancamentoPago
{
    public class DadosProcessoEstornoDTO {
        public string NumeroProcesso { get; set; }
        public string ClassificacaoHierarquica { get; set; }
        public long CodigoProcesso { get; set; }
        public string NomeComarca { get; set; }
        public string CodigoVara { get; set; }
        public string NomeTipoVara { get; set; }
        public string UF { get; set; }
        public string DescricaoEmpresaDoGrupo { get; set; }
        public string DescricaoEscritorio { get; set; }
        public string ResponsavelInterno { get; set; }
        public ICollection<DadosLancamentoEstornoDTO> DadosLancamentosEstorno { get; set; }
        public ICollection<PedidoEstornoDTO> Pedidos { get; set; }
        public ICollection<Reclamantes_ReclamadasDTO> Reclamantes { get; set; }
        public ICollection<Reclamantes_ReclamadasDTO> Reclamadas { get; set; }
    }

    public class DadosLancamentoEstornoDTO {
        public long CodigoProcesso { get; set; }
        public long CodigoLancamento { get; set; }
        public string DataCriacaoPedido { get; set; }
        public string PedidoSAP { get; set; }
        public string DataRecebimentoFiscal { get; set; }
        public string DataPagamento { get; set; }
        public string DataLancamento { get; set; }
        public decimal Valor { get; set; }
        public string StatusPagamento { get; set; }
        public long CodigoCategoriaPagamento { get; set; }
        public string CategoriaPagamento { get; set; }
        public string FormaPagamento { get; set; }
        public string Fornecedor { get; set; }
        public string Centro { get; set; }
        public string CentroCusto { get; set; }
        public string DataLevantamento { get; set; }
        public string ParteEfetivouLevantamento { get; set; }
        public string Comentario { get; set; }
        public int QuantidadeCredoresAssociados { get; set; }
        public bool ReduzirPagamentoCredor { get; set; }
        public bool CriarNovaParcelaFutura { get; set; }
        public long CodigoTipoLancamento { get; set; }
        public string DescricaoTipoLancamento { get; set; }
        public decimal ValorCompromisso { get; set; }
        public long CodigoCompromisso { get; set; }
        public long CodigoParcela { get; set; }
        public long CodigoTipoProcesso { get; set; }
    }       

}
