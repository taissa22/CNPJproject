using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class AndamentoPedido : Notifiable, IEntity, INotifiable
    {

        private AndamentoPedido() {
        }
        public int Id { get; private set; }

        public int ProcessoId { get; private set; }

        public Pedido Pedido { get; private set; }

        public int? EventoId { get; set; }
        public int? DecisaoId { get; set; }
    }
}
