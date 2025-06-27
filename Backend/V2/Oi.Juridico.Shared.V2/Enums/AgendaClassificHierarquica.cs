using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum AgendaClassificHierarquica
    {
        [Display(Description = "Único")]
        Unico = 1,
        [Display(Description = "Primário")]
        Primario = 2,
        [Display(Description = "Secundário")]
        Secundario = 3
    }
}
