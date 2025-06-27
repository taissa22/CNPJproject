using System;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao
{
    public class LoteCriacaoDTO
    {
        public long Id { get; set; }
        public string IdentificacaoLote { get; set; }
        public double ValorLote { get; set; }
        public DateTime? DataCriacaoLote = DateTime.Now;
        public long CodigoTipoProcesso { get; set; }
        public long CodigoParteEmpresa { get; set; }
        public long CodigoFornecedor { get; set; }
        public long CodigoCentroCusto { get; set; }
        public long CodigoFormaPagamento { get; set; }
        public long CodigoCentroSAP { get; set; }
        public long CodigoUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public long CodigoStatusPagamento = Convert.ToInt64(StatusPagamentoEnum.LoteGeradoAguardandoEnvioSAP);

        //public IList<LoteLancamento> loteLancamento { get; set; }
        //public IList<BorderoDTO> Borderos { get; set; }
    }
    public class DadosLoteCriacaoLancamentoDTO {
        public long CodigoProcesso { get; set; }
        public long CodigoLancamento { get; set; }
        public decimal ValorLancamento { get; set; }
        public long CodigoStatusPagamento { get; set; }
        public string MensagemErro { get; set; }
    }
}