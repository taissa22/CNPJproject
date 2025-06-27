using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Enum
{
    public enum AgendarApuracaoOutliersStatusEnum
    {
        [Description("Agendado")]
        Agendado = 0,
        [Description("Processando")]
        Processando = 1,
        [Description("Finalizado")]
        Finalizado = 2,
        [Description("Erro")]
        Erro = 3
    }
}
