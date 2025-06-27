using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class EscritorioDoUsuario : Notifiable, IEntity, INotifiable
    {
        private EscritorioDoUsuario()
        {
        }

        public string UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }

        public int EscritorioId { get; private set; }
        public Escritorio Escritorio { get; private set; }
    }
}