﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.AgendaAudienciaContext.Entities
{
    public partial class ModalidadeAudiencia
    {
        public ModalidadeAudiencia()
        {
            AudienciaProcesso = new HashSet<AudienciaProcesso>();
        }

        public int CodModalidadeAudiencia { get; set; }
        public string DscModalidadeAudiencia { get; set; }
        public string IndAtivo { get; set; }

        public virtual ICollection<AudienciaProcesso> AudienciaProcesso { get; set; }
    }
}