﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.CriminalContext.Entities
{
    public partial class Assunto
    {
        public Assunto()
        {
            AssuntoProcesso = new HashSet<AssuntoProcesso>();
            Processo = new HashSet<Processo>();
        }

        public short CodAssunto { get; set; }
        public string DscAssunto { get; set; }
        public string IndCivel { get; set; }
        public string IndJuizado { get; set; }
        public string DscProposta { get; set; }
        public string DscNegociacao { get; set; }
        public string IndCivelEstrategico { get; set; }
        public string IndAtivo { get; set; }
        public string IndCriminalAdm { get; set; }
        public string IndCriminalJudicial { get; set; }
        public string IndCivelAdm { get; set; }
        public decimal? FtgerIdFatoGerador { get; set; }
        public string IndProcon { get; set; }
        public string CodTipoCalculoContingencia { get; set; }

        public virtual ICollection<AssuntoProcesso> AssuntoProcesso { get; set; }
        public virtual ICollection<Processo> Processo { get; set; }
    }
}