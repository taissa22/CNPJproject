using System.ComponentModel;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Enum
{
    public enum StatusDoObjetoEnum
    {
        [Description("Ativo")]
        Ativo = 1,
        [Description("Inativo")]
        Inativo = 2,
        [Description("Ambos")]
        Ambos = 3
    }
}
