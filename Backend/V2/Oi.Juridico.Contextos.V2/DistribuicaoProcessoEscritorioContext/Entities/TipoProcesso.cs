﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Entities
{
    public partial class TipoProcesso
    {
        public TipoProcesso()
        {
            ParamDistribuicao = new HashSet<ParamDistribuicao>();
        }

        public short CodTipoProcesso { get; set; }
        public string DscTipoProcesso { get; set; }
        public string IndDistribuicaoEscritorio { get; set; }

        public virtual ICollection<ParamDistribuicao> ParamDistribuicao { get; set; }
    }
}