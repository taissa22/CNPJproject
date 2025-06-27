using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum EsocialCodigoIrrf
    {
        [Display(Description = "IRRF - Decisão da Justiça do Trabalho")]
        IrrfDecisaoJustica = 593656,
        [Display(Description = "IRRF - CCP/NINTER")]
        IrrfCcpNinter = 056152,
        [Display(Description = "IRRF - RRA")]
        IrrfRra = 188951
    }
}

