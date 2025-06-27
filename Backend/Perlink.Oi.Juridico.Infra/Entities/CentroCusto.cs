using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class CentroCusto : Notifiable, IEntity, INotifiable
    {
        private CentroCusto()
        {
        }

        public int Id { get; private set; }

        public string Descricao { get; private set; }

        public bool Ativo { get; private set; }
    }
}