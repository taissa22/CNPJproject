using FluentValidation.Results;

namespace Shared.Domain.Impl.Validator
{
    public static class ValidatorExtensions
    {
        public static void AdicionarMensagens(this ValidationResult principal, ValidationResult validacao, string agrupador)
        {
            if (validacao != null && validacao.Errors.Count > 0)
            {
                principal.Errors.Add(new ValidationFailure("Agrupador", agrupador));

                principal.AdicionarMensagens(validacao);
            }
        }

        public static void AdicionarMensagens(this ValidationResult principal, ValidationResult validacao)
        {
            if (validacao != null && validacao.Errors.Count > 0)
            {
                foreach (var error in validacao.Errors)
                    principal.Errors.Add(error);
            }
        }

        public static ResultadoValidacao Transformar(this ValidationResult principal)
        {
            return new ResultadoValidacao(principal.Errors);
        }
    }
}
