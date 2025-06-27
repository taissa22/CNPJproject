using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Infra.Entities.Internal
{
    internal sealed class ProfissionalBase : IEntity
    {
        private ProfissionalBase()
        {
        }

        public int Id { get; private set; }

        #region ENTITIES

        internal Escritorio Escritorio { get; private set; }

        internal Profissional Profissional { get; private set; }

        #endregion ENTITIES
    }
}