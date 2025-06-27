using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao {
    public class LoteCriacaoResultadoLancamentoDTO {
        public long CodigoProcesso { get; set; }
        public long CodigoLancamento { get; set; }
        public string DescricaoEscritorio { get; set; }
        public string NumeroProcesso { get; set; }
        public double ValorLiquido { get; set; }
        public string Uf { get; set; }
        public long CodigoComarca { get; set; }
        public string NomeComarca { get; set; }
        public long CodigoVara { get; set; }
        public long CodigoTipoVara { get; set; }
        public string NomeTipoVara { get; set; }
        public long CodigoTipoLancamento { get; set; }
        public string DescricaoTipoLancamento { get; set; }
        public long CodigoCategoriaPagamento { get; set; }
        public string DescricaoCategoriaPagamento { get; set; }
        public DateTime DataCriacaoLancamento { get; set; }
        public double QuantidadeLancamento { get; set; }
        public string TextoSAP { get; set; }
        public long? NumeroGuia { get; set; }
        public string NomeParte { get; set; }
        public long CodigoStatusPagamento { get; set; }
        public long? CodigoParte { get; set; }
    }
}
