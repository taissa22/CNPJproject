using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class DecisaoObjetoProcesso : Notifiable, IEntity, INotifiable
    {
        private DecisaoObjetoProcesso()
        {
        }

        public int Id { get; private set; }

        public int PedidoId { get; private set; }
        public int? EventoId { get; set; }
        public int? DecisaoId { get; set; }

    }
}