using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Oi.Juridico.WebApi.V2.Areas.Processos.Enums
{
    public enum TipoFiltroNumeroProcessoEnum
    {
        [Description("Atual")]
        Atual = 0,
        [Description("Antigo")]
        Antigo = 1,
        [Description("Atual ou Antigo")]
        Ambos = 2
    }
}
