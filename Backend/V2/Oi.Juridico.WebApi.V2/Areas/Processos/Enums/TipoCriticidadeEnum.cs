using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Oi.Juridico.WebApi.V2.Areas.Processos.Enums
{
    public enum TipoCriticidadeEnum
    {
        [Description("Alta")]
        Alta = 1,
        [Description("Média")]
        Media = 2,
        [Description("Baixa")]
        Baixa = 3
    }
}
