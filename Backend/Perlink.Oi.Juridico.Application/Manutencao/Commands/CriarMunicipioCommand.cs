using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarMunicipioCommand : Validatable, IValidatable
    {
        public string EstadoId { get; set; }
        public string Nome { get; set; }

        public override void Validate()
        {
            if (String.IsNullOrEmpty(EstadoId))
            {
                AddNotification(nameof(EstadoId), "Campo Requerido");
            }

        }
    }
}
