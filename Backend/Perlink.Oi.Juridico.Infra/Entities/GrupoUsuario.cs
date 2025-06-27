using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class GrupoUsuario : Notifiable, IEntity, INotifiable
    {
        private GrupoUsuario()
        {
        }

        public string Aplicacao { get; private set; }

        internal string UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }

        public string GrupoDeAplicacao { get; private set; }
        public string Nome { get; private set; }
    }
}