using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarFatoGeradorCommand : Validatable, IValidatable
    {
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        
        public override void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "O Nome não pode estar vazia");
            }
        }
    }
}