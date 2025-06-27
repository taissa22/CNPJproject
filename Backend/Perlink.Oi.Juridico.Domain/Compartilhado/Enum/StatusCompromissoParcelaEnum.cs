using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Enum {
    public enum StatusCompromissoParcelaEnum {
        [Description("Agendada")]
        Agendada = 1,
        [Description("Atrasada")]
        Atrasada = 2,
        [Description("Pagamento em tramitação")]
        PagamentoEmTramitação = 3,
        [Description("Paga")]
        Paga = 4,
        [Description("Suspensa")]
        Suspensa = 5,
        [Description("Cancelada")]
        Cancelada = 6,
    }
}
