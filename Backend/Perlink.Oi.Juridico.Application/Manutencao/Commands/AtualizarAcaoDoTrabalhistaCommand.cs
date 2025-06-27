using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarAcaoDoTrabalhistaCommand: Validatable, IValidatable
    {
        public int id { get; set; }

        public string Descricao { get; set; }

        public override void Validate()
        {

            if (id == 0)
            {
                AddNotification(nameof(id), "Campo Requerido");
            }

            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo requerido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(30))
            {
                AddNotification(nameof(Descricao), "Limite de caracteres exedido");
            }
        }
    }
}
