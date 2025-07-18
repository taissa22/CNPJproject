﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.PautaJuizadoComposicaoContext.Entities
{
    public partial class Pedido
    {
        public Pedido()
        {
            PedidoProcesso = new HashSet<PedidoProcesso>();
        }

        public short CodPedido { get; set; }
        public string DscPedido { get; set; }
        public string IndPedidoCivel { get; set; }
        public string IndPedidoTrabalhista { get; set; }
        public string IndPedidoRegulatorio { get; set; }
        public string CodRiscoPerda { get; set; }
        public string IndPedidoTributarioAdm { get; set; }
        public string IndPedidoTributarioJud { get; set; }
        public string IndPedidoTrabalhistaAdm { get; set; }
        public string IndPedidoJuizado { get; set; }
        public string IndEscritorioObrigatorio { get; set; }
        public string IndProvavelZero { get; set; }
        public string IndProprioTerceiro { get; set; }
        public string IndPedidoAtivo { get; set; }
        public string IndInfluenciaContingencia { get; set; }
        public short? CppCodClassificacaoPedido { get; set; }
        public string IndCivelEstrategico { get; set; }
        public short? GrpdCodGrupoPedido { get; set; }
        public decimal PercRateioRisco { get; set; }
        public decimal PercMelhorRealizavel { get; set; }
        public decimal ValorReceitaMedia { get; set; }
        public DateTime? DataBaseReceitaMedia { get; set; }
        public string IndCriminalJudicial { get; set; }
        public string IndCriminalAdm { get; set; }
        public string IndCivelAdm { get; set; }
        public string IndProcon { get; set; }
        public string IndPex { get; set; }
        public string IndAtivoTributarioAdm { get; set; }
        public string IndAtivoTributarioJud { get; set; }
        public string IndRequerAtualizacaoDebito { get; set; }

        public virtual ICollection<PedidoProcesso> PedidoProcesso { get; set; }
    }
}