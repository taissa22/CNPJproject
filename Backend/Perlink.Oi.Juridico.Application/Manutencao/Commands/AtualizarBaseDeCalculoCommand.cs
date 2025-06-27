using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarBaseDeCalculoCommand : Validatable, IValidatable
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public bool IndBaseInicial { get; set; } = false;

        public override void Validate()
        {
            if (Codigo == 0)
            {
                AddNotification(nameof(Codigo), "Campo Requerido");
            }

            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo Requerido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(50))
            {
                AddNotification(nameof(Descricao), "Limite de caracteres excedido");
            }
        }
    }
}