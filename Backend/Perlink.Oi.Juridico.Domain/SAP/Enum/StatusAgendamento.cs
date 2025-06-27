using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.Enum
{
   public enum StatusAgendamento
    {
        [Description("Agendado")]
        Agendado = 1,
        [Description("Processando")]
        EmExecucao = 2,
        [Description("Finalizado")]
        Executado = 3,
        [Description("Erro")]
        Erro = 5

    }
}
