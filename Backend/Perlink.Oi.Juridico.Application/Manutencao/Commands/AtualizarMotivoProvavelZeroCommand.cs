using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarMotivoProvavelZeroCommand : Validatable, IValidatable
    {

        public int Id { get; set; }
        public string Descricao { get; set; }

        public override void Validate()
        {
            if (Id < 0)
            {
                AddNotification(nameof(Id), "Id deve ser maior que zero");
            }

            if (String.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Descrição não pode ser vazio");
            }

        }

    }
}
