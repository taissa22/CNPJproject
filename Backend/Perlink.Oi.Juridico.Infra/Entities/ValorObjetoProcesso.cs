using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class ValorObjetoProcesso : Notifiable, IEntity, INotifiable
    {
        private ValorObjetoProcesso()
        {
        }
        public int Id { get; private set; }
        public int PedidoId { get; private set; }

    }
}

