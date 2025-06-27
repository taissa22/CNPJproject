using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class Despesa : Notifiable, IEntity, INotifiable
    {
        private Despesa()
        {
        }

        public int Sequencial { get; private set; }

        public int ProcessoId { get; private set; }
        public Processo Processo { get; private set; }

        internal Lancamento Lancamento { get; private set; }
    }
}