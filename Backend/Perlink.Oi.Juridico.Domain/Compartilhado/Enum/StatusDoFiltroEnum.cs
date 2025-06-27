using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Enum
{
    public enum StatusDoFiltroEnum
    {
        [Description("Sim")]
        Sim = 1,
        [Description("Nao")]
        Nao = 2,
        [Description("Indiferente")]
        Indiferente = 3
    }
}
