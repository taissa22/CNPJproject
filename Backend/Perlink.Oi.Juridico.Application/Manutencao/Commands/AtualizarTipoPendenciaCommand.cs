using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarTipoPendenciaCommand : Validatable, IValidatable
    {
        public int Id { get; set; } = 0;

        public string Descricao { get; set; } = string.Empty;

        public override void Validate()
        {
            if (Id == 0)
            {
                AddNotification(nameof(Id), "Campo Requerido");
            }

            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo Requerido");
            }

            if (!string.IsNullOrEmpty(Descricao) && Descricao.Length > 50)
            {
                AddNotification(nameof(Descricao), "O Campo descrição pode conter no máximo 50 caracteres.");
            }
        }
    }
}
