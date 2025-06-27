using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class PagamentoProcesso : Notifiable, IEntity, INotifiable
    {
        private PagamentoProcesso()
        {
        }

        public int Sequencial { get; private set; }

        internal int ProcessoId { get; private set; }

        internal int? ParteId { get; private set; }
        public Parte Parte { get; private set; }

        internal int? ProfissionalId { get; private set; }
        public Profissional Profissional { get; private set; }
    }
}