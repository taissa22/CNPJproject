﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Oi.Juridico.Contextos.V2.AtmPexContext.Entities
{
    public partial class Indice
    {
        public Indice()
        {
            AtmIndiceUfPadrao = new HashSet<AtmIndiceUfPadrao>();
        }

        public short CodIndice { get; set; }
        public string NomIndice { get; set; }
        public string CodTipoIndice { get; set; }
        public string CodValorIndice { get; set; }
        public string IndAcumulado { get; set; }
        public string IndCalcAcumuladoAuto { get; set; }

        public virtual ICollection<AgendFechAtmPexUf> AgendFechAtmPexUf { get; set; }
        public virtual ICollection<AtmIndiceUfPadrao> AtmIndiceUfPadrao { get; set; }
        public virtual ICollection<Estado> Estado { get; set; }
    }
}