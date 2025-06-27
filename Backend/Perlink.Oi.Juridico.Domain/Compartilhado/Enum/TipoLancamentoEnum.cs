using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Enum
{
    public enum TipoLancamentoEnum
    {
        Garantias = 1,
        [Description("Despesas Judiciais")]
        DespesasJudiciais = 2,
        [Description("Pagamentos")]
        Pagamentos = 3,
        [Description("Honorários")]
        Honorarios = 4,
        [Description("Recuperação de Pagamentos")]
        RecuperacaoPagamento = 5
    }
}
