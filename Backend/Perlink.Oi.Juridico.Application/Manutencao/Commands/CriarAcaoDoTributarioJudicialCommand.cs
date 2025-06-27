using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands {

    public class CriarAcaoDoTributarioJudicialCommand : Validatable, IValidatable {
        public string Descricao { get; set; } = string.Empty;

        public override void Validate() {
            if (string.IsNullOrEmpty(Descricao)) {
                AddNotification(nameof(Descricao), "O campo deve ser preenchido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(30)) {
                AddNotification(nameof(Descricao), "Limite de caracteres excedido");
            }
        }
    }
}