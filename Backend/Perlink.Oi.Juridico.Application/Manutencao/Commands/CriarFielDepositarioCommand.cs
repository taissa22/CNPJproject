using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands {

    public class CriarFielDepositarioCommand : Validatable, IValidatable
    {
        public string Cpf { get; set; }

        public string Nome { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Cpf))
            {
                AddNotification(nameof(Cpf), "Campo Requerido");
            }

            if (!string.IsNullOrEmpty(Cpf) && !Cpf.HasMaxLength(11))
            {
                AddNotification(nameof(Cpf), "Limite de caracteres excedido");
            }


            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo Requerido");
            }

            if (!string.IsNullOrEmpty(Nome) && !Nome.HasMaxLength(100))
            {
                AddNotification(nameof(Nome), "Limite de caracteres excedido");
            }
        }
    }
}