using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarEstadoCommand : Validatable, IValidatable
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public decimal ValorJuros { get; set; }

        public override void Validate()
        {
            if (String.IsNullOrEmpty(Id))
            {
                AddNotification(nameof(Id), "Campo Requerido");
            }

            if (String.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo Requerido");
            }
         
        }
    }
}
