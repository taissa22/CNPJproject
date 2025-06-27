using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands {

    public class CriarAcaoDoCivelEstrategicoCommand : Validatable, IValidatable {

        public int? IdConsumidor { get; set; }
        public string Descricao { get; set; } = string.Empty;

        public bool Ativo { get; set; } = false;

        public override void Validate() {
            if (string.IsNullOrEmpty(Descricao)) {
                AddNotification(nameof(Descricao), "Campo requerido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(30)) {
                AddNotification(nameof(Descricao), "Limite de caracteres excedido");
            }
        }
    }
}