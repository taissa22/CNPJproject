﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.Entities
{
    public partial class EmpresasCentralizadoras
    {
        public EmpresasCentralizadoras()
        {
            FechamentoPexMedia = new HashSet<FechamentoPexMedia>();
        }

        public byte Codigo { get; set; }
        public string Nome { get; set; }
        public byte NumOrdemClassifProcesso { get; set; }
        public byte? NumAgenciaDepositaria { get; set; }
        public bool? NumDigitoAgenciaDepositaria { get; set; }
        public byte? CodConvenioBb { get; set; }

        public virtual ICollection<FechamentoPexMedia> FechamentoPexMedia { get; set; }
    }
}