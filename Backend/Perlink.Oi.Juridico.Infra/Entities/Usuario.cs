using Perlink.Oi.Juridico.Infra.Entities.Internal;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Usuario : Notifiable, IEntity, INotifiable
    {
        private Usuario()
        {
        }

        public string Id { get; private set; }

        public string Nome { get; private set; }

        public bool Ativo { get; private set; }

        public bool IndPreposto { get; private set; }

        public bool Perfil { get; private set; }

        public IReadOnlyCollection<GrupoUsuario> Grupos { get; private set; }

        internal UsuarioBase UsuarioBase { get; private set; }
    }
}