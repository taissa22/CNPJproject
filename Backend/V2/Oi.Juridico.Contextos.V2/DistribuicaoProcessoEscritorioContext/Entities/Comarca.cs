﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Entities
{
    public partial class Comarca
    {
        public Comarca()
        {
            ParamDistribuicao = new HashSet<ParamDistribuicao>();
            Vara = new HashSet<Vara>();
        }

        public short CodComarca { get; set; }
        public string CodEstado { get; set; }
        public string NomComarca { get; set; }
        public int? CodEscritorioCivel { get; set; }
        public int? CodEscritorioTrabalhista { get; set; }
        public string CodComarcaBb { get; set; }
        public int? ProfCodProfissionalCivEstr { get; set; }
        public decimal? BbcomIdBbComarca { get; set; }

        public virtual Profissional CodEscritorioCivelNavigation { get; set; }
        public virtual Profissional CodEscritorioTrabalhistaNavigation { get; set; }
        public virtual Estado CodEstadoNavigation { get; set; }
        public virtual Profissional ProfCodProfissionalCivEstrNavigation { get; set; }
        public virtual ICollection<ParamDistribuicao> ParamDistribuicao { get; set; }
        public virtual ICollection<Vara> Vara { get; set; }
    }
}