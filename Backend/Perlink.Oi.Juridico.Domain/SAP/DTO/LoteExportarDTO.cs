using CsvHelper.Configuration.Attributes;
using System;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO {

    public class LoteExportarDTO {

        public long Id { get; set; }
        public string DataCriacaoLote { get; set; }
        public string DescricaoLote { get; set; }
        public string DescricaoStatusPagamento { get; set; }
        public string DataCriacaoPedido { get; set; }
        public string NumeroPedido { get; set; }
        public string DescricaoEmpresaGrupo { get; set; }
        public string DescricaoFornecedor { get; set; }
        public string DescricaoCentroCusto { get; set; }
        public string ValorLote { get; set; }
        public string QuantidadeLancamento { get; set; }
        public string DataGeracaoArquivoBB { get; set; }
        public string DataRetornoBB { get; set; }
        public string NumeroLoteBB { get; set; }
        public string DataErroProcessamento { get; set; }
    }
}