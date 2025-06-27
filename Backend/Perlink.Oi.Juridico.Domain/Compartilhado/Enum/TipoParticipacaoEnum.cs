using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Enum {
    public enum TipoParticipacaoEnum {
        [Description("Autor")]
        Autor = 1,
        [Description("Réu")]
        Reu = 2,
        [Description("Reclamante")]
        Reclamante = 3,
        [Description("Reclamada")]
        Reclamada = 4,
        [Description("Autuante")]
        Autuante = 5,
        [Description("Autuado")]
        Autuado = 6
    }
}