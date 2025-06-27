using Perlink.Oi.Juridico.Application.Manutencao.Results.Evento;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
#nullable enable

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarEventoDependenteCommand : Validatable, IValidatable
    {
        public List<EventoDisponivelCommandResult> Lista { get; set; }
        public int EventoId { get; set; }


        public override void Validate()
        {
           if (this.Lista.Count <= 0) {
                AddNotification(nameof(Lista), "Campo requerido");
           }
        }
    }
}