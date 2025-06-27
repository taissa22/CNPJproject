using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao {
    public class LoteCriacaoResultadoEmpresaGrupoDTO {
        public long CodigoEmpresaGrupo { get; set; }
        public string DescricaoEmpresaGrupo { get; set; }
        public string Uf { get; set; }
        public string DescricaoFornecedor { get; set; }
        public string DescricaoFormaPagamento { get; set; }
        public string DescricaoCentroCusto { get; set; }
        public string CentroSAP { get; set; }
        public int TotalLancamneto { get; set; }
        public long CodigoCentroCusto { get; set; }
        public long CodigoFormaPagamento { get; set; }
        public long CodigoFornecedor { get; set; }
        public string FornecedorSAP { get; set; }
        public string IndicaBordero { get; set; }
    }
}
