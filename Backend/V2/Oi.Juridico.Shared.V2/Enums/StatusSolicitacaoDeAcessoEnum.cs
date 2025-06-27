using System.ComponentModel;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum StatusSolicitacaoDeAcessoEnum
    {
        [Description("Pendente Aprovação Administrador")]
        PendenteAdministrador = 1,

        [Description("Pendente Aprovação Gestor")]
        PendenteGestor = 2,

        [Description("Negada Administrador")]
        NegadaAdministrador = 3,

        [Description("Negada Gestor")]
        NegadaGestor = 4,

        [Description("Reativação Realizada")]
        AcessoCriado = 5
    }
}
