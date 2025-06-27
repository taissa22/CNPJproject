using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarComplementoDeAreaEnvolvidaCommand : Validatable, IValidatable
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }

        public override void Validate()
        {
            if (Id <= 0)
            {
                AddNotification(nameof(Id), "O campo nome não pode estar vazio");
            }

            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "O campo nome não pode estar vazio");
            }

            if (Nome?.Length > 100)
            {
                AddNotification(nameof(Nome), "O Campo Nome pode conter no máximo 100 caracteres");
            }
        }
    }
}