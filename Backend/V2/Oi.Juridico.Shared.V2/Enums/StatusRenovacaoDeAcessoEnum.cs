using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum StatusRenovacaoDeAcessoEnum
    {
        [Description("Pendente Aprovação Administrador")]
        PendenteAdministrador = 1,

        [Description("Pendente Aprovação Gestor")]
        PendenteGestor = 2,

        [Description("Negada Administrador")]
        NegadaAdministrador = 3,

        [Description("Negada Gestor")]
        NegadaGestor = 4,

        [Description("Renovação Relizada")]
        RenovacaoRealizada = 5
    }
}
