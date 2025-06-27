using System.ComponentModel;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum SituacaoUsuarioArquivoDeImportacaoEnum
    {
        [Description("Ativo")]
        A,
        [Description("Falecido")]
        D,
        [Description("Licença")]
        L,
        [Description("Licença Remunerada")]
        P,
        [Description("Aposentado c/ Remuneração")]
        Q,
        [Description("Aposentado")]
        R,
        [Description("Suspensão")]
        S,
        [Description("Desligado")]
        T,
        [Description("Desligado c/ Remuneração")]
        U,
        [Description("Pagamento Pensão Desligamento")]
        V,
        [Description("Aposentado - Admin Pensão")]
        X,
        [Description("Férias")]
        Y

    }
}
