using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Infra.Entities.Internal
{
    internal sealed class ParteBase : IEntity
    {
        private ParteBase()
        {
        }

        public int Id { get; private set; }

        #region ENTITIES

        internal Parte Parte { get; private set; }
        internal EmpresaDoGrupo EmpresaDoGrupo { get; private set; }     

        internal Orgao Orgao { get; private set; }
        internal Estabelecimento Estabelecimento { get; private set; }

        #endregion ENTITIES
    }
}