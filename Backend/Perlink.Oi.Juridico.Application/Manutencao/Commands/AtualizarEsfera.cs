using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarEsferaCommand : Validatable, IValidatable
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool CorrigePrincipal { get; set; }
        public bool CorrigeMultas { get; set; }
        public bool CorrigeJuros { get; set; }
        public override void Validate()
        {
            if (Id <= 0)
            {
                AddNotification(nameof(Id), "Campo requerido");
            }

            if (String.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo requerido");
            }
        }
    }
}