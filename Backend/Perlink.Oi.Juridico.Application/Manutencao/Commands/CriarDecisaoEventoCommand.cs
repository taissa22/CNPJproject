using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarDecisaoEventoCommand : Validatable, IValidatable
    {
        public int EventoId { get; set; }
        public string Descricao { get; set; }
        public bool? RiscoPerda { get; set; }       
        public string? PerdaPotencial { get; set; }
        public bool DecisaoDefault { get; set; }
        public bool? ReverCalculo { get; set; }
        
        public override void Validate()
        {
            if (EventoId <= 0)
            {
                AddNotification(nameof(EventoId), "Campo requerido");
            }

            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo requerido");
            }
        }
    }
}