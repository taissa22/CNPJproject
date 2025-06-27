using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class DespesaProfissional : Notifiable, IEntity, INotifiable
    {
        private DespesaProfissional()
        {
        }

        public int ProfissionalId { get; private set; }
        public int Sequencial { get; private set; }
    }
}