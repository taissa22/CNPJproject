using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

#nullable enable

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarComplementoDeAreaEnvolvidaCommand : Validatable, IValidatable
    {
        public string Nome { get; set; } = null!;
        public bool Ativo { get; set; }
        public int TipoProcessoId { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "O campo nome não pode estar vazio");
            }

            if (Nome?.Length > 100)
            {
                AddNotification(nameof(Nome), "O Campo Nome pode conter no máximo 100 caracteres");
            }
            if(TipoProcessoId <= 0)
            {
                AddNotification("Tipo Processo", "O campor não pode ser invalido");
            }
        }
    }
}