﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.ManutencaoContext.Entities
{
    public partial class DeParaStatusAppPreposto
    {
        public decimal Id { get; set; }
        public byte? StatusApp { get; set; }
        public byte? SubstatusApp { get; set; }
        public byte? StatusSisjur { get; set; }
        public string CriarAudiencia { get; set; }
        public byte? CodTipoProcesso { get; set; }

        public virtual StatusDeParaAppPreposto StatusAppNavigation { get; set; }
        public virtual StatusAudiencia StatusSisjurNavigation { get; set; }
        public virtual StatusDeParaAppPreposto SubstatusAppNavigation { get; set; }
    }
}