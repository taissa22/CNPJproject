﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Oi.Juridico.Contextos.RelatoriosContingenciaTrabalhistaContext.Entities
{
    public partial class ItemFechProvisaoProvaveis
    {
        public decimal Id { get; set; }
        public decimal IfptrIdItemFechProvTrab { get; set; }
        public short PedCodPedido { get; set; }
        public string NomePedido { get; set; }
        public decimal PercPerdaProvavel { get; set; }
        public decimal QtePedidosProvavelP { get; set; }
        public decimal QtePedidosConcluidosP { get; set; }
        public decimal QtePedidosDesembolsoP { get; set; }
        public decimal ValorTotalDesembolsoJurosP { get; set; }
        public decimal ValorTotalDesembPrincipalP { get; set; }
        public decimal ValorMedioDesembolsoJuros { get; set; }
        public decimal ValorMedioDesembPrincipal { get; set; }
        public long QteExpectativaPerdaP { get; set; }
        public decimal? ExpectativaPerdaP { get; set; }
        public decimal? ValProvContJurosP { get; set; }
        public decimal? ValProvContPrincipalP { get; set; }
        public decimal PerResponsOi { get; set; }
        public decimal QtePedidosProvavelH { get; set; }
        public decimal? ExpectativaPerdaH { get; set; }
        public decimal? ValProvContJurosH { get; set; }
        public decimal? ValProvContPrincipalH { get; set; }
        public decimal? QtePedidosProvavel { get; set; }
        public decimal? ValProvContJuros { get; set; }
        public decimal? ValProvContPrincipal { get; set; }

        public virtual ItemFechamentoProvisaoTrab IfptrIdItemFechProvTrabNavigation { get; set; }
    }
}