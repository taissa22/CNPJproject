using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Relatorios.Contingencia
{
    public class TotalizadoresProvisaoTrabalhistaPoResponse
    {
        public decimal QtePedidosPossivelP { get; set; }
        public decimal QtePedidosProvavelP { get; set; }
        public decimal QtePedidosRemotoP { get; set; }
        public int QtdRegistros { get; set; }
        public decimal? ValorPrincipalPedidosPo { get; set; }
        public decimal? ValorCorrecaoJurosPo { get; set; }
        public decimal? ValPriProvContPedNConH { get; internal set; }
        public decimal? ValJurProvContPedNConH { get; internal set; }
        public decimal? TotPrincProvContPedSDes { get; internal set; }
        public decimal? TotJurProvContPedSDes { get; internal set; }
        public decimal? QtePedidosPossivelH { get; internal set; }        
    }
}
