﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.RelatorioSolicitacaoLancamentoPexContext.Entities
{
    public partial class Estado
    {
        public Estado()
        {
            ProfissionalCodEstadoNavigation = new HashSet<Profissional>();
            ProfissionalCodEstadoOabNavigation = new HashSet<Profissional>();
        }

        public string CodEstado { get; set; }
        public string NomEstado { get; set; }
        public short? CodIndice { get; set; }
        public decimal? ValJuros { get; set; }
        public short? SeqMunicipio { get; set; }

        public virtual ICollection<Profissional> ProfissionalCodEstadoNavigation { get; set; }
        public virtual ICollection<Profissional> ProfissionalCodEstadoOabNavigation { get; set; }
    }
}