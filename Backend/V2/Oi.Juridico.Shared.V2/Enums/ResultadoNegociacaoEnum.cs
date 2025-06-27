using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ResultadoNegociacaoEnum
    {
        [Description("Motivo sem acordo")]
        Motivo_sem_acordo = 1,
        [Description("Opção de acordo")]
        Opcao_de_acordo = 2
    }
}
