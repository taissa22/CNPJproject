using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Relatorios.Contingencia
{
    public class DetalheContingenciaTrabalhistaPoResponse : CabecalhoProvisaoTrabalhistaPoResponse
    {
        public decimal Id { get; set; }
        public decimal IfptrIdItemFechProvTrab { get; set; }
        public short PedCodPedido { get; set; }
        public string NomePedido { get; set; }
        //public decimal PercPerdaProvavel { get; set; }
        //public decimal PercPerdaPossivel { get; set; }
        //public decimal QtePedidosPossivel { get; set; }
        //public decimal QtePedidosProvavel { get; set; }
        //public decimal QtePedidosRemoto { get; set; }
        //public decimal ValorMedioDesemJurosPr { get; set; }
        //public decimal ValorMedioDesemPrincipalPr { get; set; }
        public decimal? ValProvContPrincipalP { get; set; }
        public decimal? ValProvContJurosP { get; set; }

        //HÍBRIDO
        public decimal QtePedidosSemDesembolsoH { get; set; }
        public decimal QtePedidoNaoConcluidoPoH { get; set; }
        public decimal QtePedidoNaoConcluidoReH { get; set; }
        public decimal PerPerdaPedNaoConcluidoH { get; set; }
        public decimal PerResponsOi { get; set; }
        public decimal? ValProvContPrincipalH { get; set; }
        public decimal? ValProvContJurosH { get; set; }
        public decimal? ValProvContPrincipal { get; set; }
        public decimal? ValProvContJuros { get; set; }
        public decimal QtePedidosProvavelH { get; internal set; }
        public decimal QtePedidosPossivelH { get; internal set; }
        public decimal QtePedidosRemotoH { get; internal set; }
        public decimal PercPerdaPossivelH { get; internal set; }
        public decimal PercPerdaProvavelH { get; internal set; }
        public decimal ValorMedioDesemPrincPr { get; internal set; }        
        public decimal ValorMedioDesemJurosPr { get; internal set; }
        public decimal QtePedidosProvavel { get; internal set; }
        public decimal QtePedidosPossivelP { get; internal set; }
        public decimal QtePedidosRemoto { get; internal set; }
        public decimal PercPerdaPossivel { get; internal set; }
        public decimal PercPerdaProvavel { get; internal set; }
    }
}
