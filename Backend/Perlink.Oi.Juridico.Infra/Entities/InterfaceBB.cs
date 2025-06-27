using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class InterfaceBB : Notifiable, IEntity, INotifiable
    {
        private InterfaceBB()
        {
        }

        public int CodigoDiretorio { get; private set; }    
        public string Descricao { get; private set; }
    }
}