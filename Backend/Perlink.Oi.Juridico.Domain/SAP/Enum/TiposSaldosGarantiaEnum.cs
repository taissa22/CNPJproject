using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.Enum
{
    public enum TiposSaldosGarantiaEnum
    {
        [Description("DEPÓSITO")]
        Deposito = 1,
        [Description("BLOQUEIO")]
        Bloqueio = 2,
        [Description("OUTROS")]
        Outros = 3,
    }
}
