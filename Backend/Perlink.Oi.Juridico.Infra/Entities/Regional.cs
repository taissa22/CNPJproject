using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Regional : Notifiable, IEntity, INotifiable
    {
        private Regional()
        {
        }

        public int Id { get; private set; }

        public string Nome { get; private set; }        
    }
}