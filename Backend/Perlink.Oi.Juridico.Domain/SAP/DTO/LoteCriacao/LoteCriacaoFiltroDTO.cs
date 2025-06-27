using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao {
    public class LoteCriacaoFiltroEmpresaCentralizadoraDTO {
        public long CodigoTipoProcesso { get; set; }        
        public DateTime? DataCriacaoLancamentoInicio { get; set; }
        public DateTime? DataCriacaoLancamentoFim { get; set; }
        public double? ValorLancamentoInicio { get; set; }
        public double? ValorLancamentoFim { get; set; }        
    }
    public class LoteCriacaoFiltroEmpresaGrupoDTO {
        public long CodigoTipoProcesso { get; set; }
        public long CodigoEmpresaCentralizadora { get; set; }  
        public DateTime? DataCriacaoLancamentoInicio { get; set; }
        public DateTime? DataCriacaoLancamentoFim { get; set; }
        public double? ValorLancamentoInicio { get; set; }
        public double? ValorLancamentoFim { get; set; }
    }

    public class LoteCriacaoFiltroLancamentoDTO {
        public long CodigoTipoProcesso { get; set; }
        public long CodigoEmpresaGrupo { get; set; }
        public long CodigoCentroCusto { get; set; }
        public long CodigoFornecedor { get; set; }
        public long CodigoFormaPagamento { get; set; }
        public string Uf { get; set; }
        public DateTime? DataInicialLancamento { get; set; }
        public DateTime? DataFinalLancamento  { get; set; }
        public double? ValorInicialLancamento { get; set; }
        public double? ValorFinalLancamento { get; set; }
    }
}
