using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities {
    public sealed class Bordero : Notifiable, IEntity, INotifiable {
        private Bordero() {
        }

        internal int LoteId { get; private set; }
        public Lote Lote { get; private set; }

        public int Sequencial { get; private set; }

        public decimal ValorPago { get; private set; }
        public string Comentario { get; private set; }
    }
}
