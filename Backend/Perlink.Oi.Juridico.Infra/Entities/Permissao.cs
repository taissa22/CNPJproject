using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Permissao : Notifiable, IEntity, INotifiable
    {
        private Permissao()
        {
        }

        public string Aplicacao { get; private set; }
        internal string GrupoUsuario { get; private set; }
        public string Janela { get; private set; }
        public string Menu { get; private set; }
    }
}