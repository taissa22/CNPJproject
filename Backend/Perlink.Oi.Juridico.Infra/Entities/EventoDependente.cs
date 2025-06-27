using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class EventoDependente : Notifiable, IEntity, INotifiable
    {
        private EventoDependente()
        {
        }

        public static EventoDependente Criar(int eventoId, int eventoDependenteId)
        {
            EventoDependente obj = new EventoDependente();
            obj.EventoId = eventoId;
            obj.EventoDependenteId = eventoDependenteId;
            obj.Validate();
            return obj;
        }

        public void Atualizar(int eventoId, int eventoDependenteId)
        {
            this.EventoId = eventoId;
            this.EventoDependenteId = eventoDependenteId;
            Validate();
        }   
    

        public void Validate()
        {
        }


        public int EventoId { get; private set; }
        public int EventoDependenteId { get; private set; }
    
    }
}