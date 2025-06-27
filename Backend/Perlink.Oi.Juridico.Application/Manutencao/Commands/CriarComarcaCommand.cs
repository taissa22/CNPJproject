using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{ 
    public class CriarMotivoProvavelZeroCommand : Validatable, IValidatable
    {
        public string Descricao { get; set; } 

        public override void Validate()
        {
            if (String.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Descrição não pode ser vazio");
            }
        }

    }
}
