using System.ComponentModel;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum TipoSolicitacaoAcessoEnum
    {
        [Description("N")]
        Novo,
        [Description("R")]
        Renovacao,
        [Description("A")]
        Reativacao,
        [Description("E")]
        Ampliacao
    }
}
