using Perlink.Oi.Juridico.Domain.SAP.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class LoteResultadoDTO
    {
        public long Id { get; set; }
        public long CodigoParte { get; set; }
        public string DescricaoLote { get; set; }
        public string FormaPagamento { get; set; }
        public string NomeEmpresaGrupo { get; set; }
        public long? NumeroLoteBB { get; set; }
        public long CodigoStatusPagamento { get; set; }
        public string NomeUsuario { get; set; }
        public string DataCriacao { get; set; }
        public string StatusPagamento { get; set; }
        public string DataCriacaoPedido { get; set; }
        public long? NumeroPedidoSAP { get; set; }
        public bool ExisteBordero { get; set; } 
        public double ValorLote { get; set; }
        public LogLoteDTO ultimo { get; set; }
        
    }
    public class TotaisLoteResultadoDTO {
        public int Total { get; set; }
        public long TotalLotes { get; set; }
        public Double TotalValorLotes { get; set; }
        public long QuantidadesLancamentos { get; set; }

    }
}
