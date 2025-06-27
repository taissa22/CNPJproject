using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Infra.Entities.Internal
{
    internal sealed class UsuarioBase : IEntity
    {
        private UsuarioBase()
        {
        }

        public string Id { get; private set; }

        #region ENTITIES

        internal Usuario Usuario { get; private set; }
        internal ResponsavelInterno ResponsavelInterno { get; private set; }

        #endregion ENTITIES
    }
}