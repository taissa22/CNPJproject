using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Oi.Juridico.WebApi.V2.Areas.Processos.Enums
{
    public enum TipoFiltroEnum
    {
        [Description("Igual")]
        Igual = 0,
        [Description("Começando Por")]
        Comecando = 1,
        [Description("Terminando Por")]
        Terminando = 2,
        [Description("Em Qualquer Parte")]
        QualquerParte = 3
    }
}
