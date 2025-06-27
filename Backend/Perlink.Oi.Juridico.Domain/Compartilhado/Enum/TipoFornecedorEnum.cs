using System.ComponentModel;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Enum
{
    public enum TipoFornecedorEnum
    {
        [Description("Banco")]
        Banco = 1,
        [Description("Profissional")]
        Profissional = 2,
        [Description("Escritório")]
        Escritorio = 3
    }
    public enum TipoFornecedorEnumCategoriaPagamento
    {
        [Description("BANCO")]
        Banco = 1,
        [Description("ESCRITÓRIO/PROFISSIONAL")]
        EscritorioProfissional = 2,
        [Description("TODOS")]
        Todos = 3
    }
}
