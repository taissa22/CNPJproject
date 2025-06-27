using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarDecisaoEventoCommand : Validatable, IValidatable
    {
        public int Id { get;  set; }
        public int EventoId { get; set; }
        public string Descricao { get; set; }
        public bool? RiscoPerda { get; set; }
        public string? PerdaPotencial { get; set; }
        public bool DecisaoDefault { get; set; }
        public bool? ReverCalculo { get; set; }

        public override void Validate()
        {
            if (Id <= 0)
            {
                AddNotification(nameof(Id), "Campo requerido");
            }

            if (EventoId <= 0)
            {
                AddNotification(nameof(EventoId), "Campo requerido");
            }

            if (String.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo requerido");
            }
        }
    }
}