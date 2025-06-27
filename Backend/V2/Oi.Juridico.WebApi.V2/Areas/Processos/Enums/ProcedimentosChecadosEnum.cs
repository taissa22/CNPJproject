using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Oi.Juridico.WebApi.V2.Areas.Processos.Enums
{
    public enum ProcedimentosChecadosEnum
    {
        [Description("Indiferente")]
        Indiferente = 0,
        [Description("Sim")]
        Sim = 1,
        [Description("Não")]
        Nao = 2
    }
}
