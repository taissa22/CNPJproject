using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class NaturezaAcaoBB : Notifiable, IEntity, INotifiable
    {
        private NaturezaAcaoBB()
        {
        }

        public int Id { get; private set; }

        public int Codigo { get; private set; }

        public string Nome { get; private set; }
    }
}