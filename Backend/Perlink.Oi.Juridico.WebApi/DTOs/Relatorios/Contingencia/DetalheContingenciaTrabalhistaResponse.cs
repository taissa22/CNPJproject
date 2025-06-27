using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Relatorios.Contingencia
{
    public class DetalheContingenciaTrabalhistaResponse : CabecalhoProvisaoTrabalhistaResponse
    {        
        public decimal Id { get; set; }
        public decimal IfptrIdItemFechProvTrab { get; set; }
        public short PedCodPedido { get; set; }
        public string NomePedido { get; set; }
        //public decimal PercPerdaProvavel { get; set; }
        //public decimal QtePedidosProvavel { get; set; }
        //public decimal QtePedidosConcluidos { get; set; }
        //public decimal QtePedidosDesembolso { get; set; }
        //public decimal ValorTotalDesembolsoJuros { get; set; }
        //public decimal ValorTotalDesembPrincipal { get; set; }
        //public decimal ValorMedioDesembolsoJuros { get; set; }
        //public decimal ValorMedioDesembPrincipal { get; set; }
        //public long QteExpectativaPerda { get; set; }

        //HÍBRIDO
        //public decimal QtdPedidosSemDesembolsoH { get; set; }
        //public decimal PerResponsOi { get; set; }
        //public decimal QtdPedSDesembExpcPerdaH { get; set; }
        //public decimal ValPriProvContPedSDesH { get; set; }
        //public decimal ValJurProvContPedSDesH { get; set; }
        //public decimal TotPrincProvContPedSDes { get; set; }
        //public decimal TotJurProvContPedSDes { get; set; }

        //public decimal ExpectativaPerda { get; set; }

        //public decimal ValorProvisaoContingenciaCorrecaoJuros { get; set; }

        //public decimal ValorProvisaoContingenciaPrincipal { get; set; }

        // PRÉ CLOSING
        public decimal? ValProvContJurosP { get; internal set; }
        public decimal PercPerdaProvavelP { get; internal set; }
        public decimal? ExpectativaPerdaP { get; internal set; }
        public decimal ValorMedioDesembPrincipalP { get; internal set; }
        public decimal ValorMedioDesembolsoJurosP { get; internal set; }
        public decimal? ValProvContPrincipalP { get; internal set; }
        public decimal QtePedidosProvavelP { get; internal set; }

        // HÍBRIDO
        public decimal PerResponsOi { get; internal set; }
        public decimal QtePedidosProvavelH { get; internal set; }
        public decimal PercPerdaProvavelH { get; internal set; }
        public decimal ValorMedioDesembPrincipalH { get; internal set; }
        public decimal ValorMedioDesembolsoJurosH { get; internal set; }
        public decimal? ValProvContPrincipalH { get; internal set; }
        public decimal? ValProvContJurosH { get; internal set; }

        // PRÉ CLOSING + HÍBRIDO
        public decimal? ValProvContPrincipal { get; internal set; }
        public decimal? ValProvContJuros { get; internal set; }
        public decimal? ExpectativaPerdaH { get; internal set; }
        public long QteExpectativaPerdaP { get; internal set; }
    }
}
