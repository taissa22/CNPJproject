using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities {
    public sealed class LoteLancamento : Notifiable, IEntity, INotifiable {
        private LoteLancamento() {
        }

        public int ProcessoId { get; private set; }

        internal int LoteId { get; private set; }
        public Lote Lote { get; private set; }

        public int LancamentoId { get; private set; }
        public Lancamento Lancamento { get; private set; }
    }
}
