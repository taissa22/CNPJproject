using FluentValidation;

namespace Shared.Domain.Impl.Validator
{
    public static class NumeroValidators
    {
        public static IRuleBuilderOptions<T, string> NumeroValido<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => double.TryParse(x, out double valorPadrao)).WithMessage(Textos.Geral_Mensagem_Erro_NumeroInvalido);
        }
    }
}
