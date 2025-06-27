using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class EmpresaSap : Notifiable, IEntity, INotifiable
    {
        private EmpresaSap()
        {
        }

        public int Codigo { get; private set; }
        public string Sigla { get; private set; }
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }
    }
}