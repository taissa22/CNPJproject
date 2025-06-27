using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum StatusAgendamentoResultadoNegociacaoEnum
    {
        [Description("Agendado")]
        Agendado = 1,

        [Description("Processando")]
        Processando = 2,

        [Description("Finalizado")]
        Processado = 3,

        [Description("Erro")]
        Erro = 4,
    }
}
