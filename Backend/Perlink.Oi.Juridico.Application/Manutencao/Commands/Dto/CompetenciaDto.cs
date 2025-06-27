using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands.Dto
{
    public class CompetenciaDTO : Validatable, IValidatable
    {
        public string Nome { get; set; } = string.Empty;
        public int? Sequencial { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "O campo deve ser preenchido");
            }

            if (!string.IsNullOrEmpty(Nome) && !Nome.HasMaxLength(40))
            {
                AddNotification(nameof(Nome), "Limite de caracteres exedido");
            }

            if (Sequencial != null && Sequencial == 0)
            {
                AddNotification(nameof(Sequencial), "Sequencial inválivo");
            }
        }
    }
}
