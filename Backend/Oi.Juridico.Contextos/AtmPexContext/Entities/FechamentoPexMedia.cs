﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Oi.Juridico.Contextos.AtmPexContext.Entities
{
    public partial class FechamentoPexMedia
    {
        public decimal Id { get; set; }
        public DateTime DatFechamento { get; set; }
        public short NroMesesMediaHistorica { get; set; }
        public string IndMensal { get; set; }
        public DateTime? DatIndMensal { get; set; }
        public DateTime DatGeracao { get; set; }
        public string CodUsuario { get; set; }
        public short CodEmpresaCentralizadora { get; set; }
        public decimal? PerHaircut { get; set; }
        public string IndAplicarHaircutProcGar { get; set; }
        public decimal? ValMultDesvioPadrao { get; set; }
        public int? CodSolicFechamentoCont { get; set; }

        public virtual AcaUsuario CodUsuarioNavigation { get; set; }
    }
}