using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarEsferaCommand : Validatable, IValidatable
    {
        public string Nome { get; set; }
        public bool CorrigePrincipal { get; set; }
        public bool CorrigeMultas { get; set; }
        public bool CorrigeJuros { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo requerido");
            }
        }
    }
}