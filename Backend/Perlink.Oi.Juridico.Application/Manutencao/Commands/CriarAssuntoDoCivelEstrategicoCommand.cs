using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarAssuntoDoCivelEstrategicoCommand : Validatable, IValidatable 
    {
        public int? IdConsumidor { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public bool Ativo { get; set; } = false;

        public override void Validate() {
            if (string.IsNullOrEmpty(Descricao)) {
                AddNotification(nameof(Descricao), "O campo deve ser preenchido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(40)) {
                AddNotification(nameof(Descricao), "Limite de caracteres excedido");
            }
        }
    }
}
