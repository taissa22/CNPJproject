using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum PeriodicidadeExecucaoEnum
    {
        [Description("Execução Imediata")]
        ExecucaoImediata = 0,
        [Description("Data Específica")]
        DataEspecifica = 1,
        [Description("Diaria")]
        Diaria = 2,
        [Description("Semanal")]
        Semanal = 3,
        [Description("Mensal")]
        Mensal = 4
    }
}
