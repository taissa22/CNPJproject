using Shared.Domain.Impl.Entity;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class Procedimento : Entity
    {
        #region "ATTRIBUTE"

        #endregion

        #region "CONSTRUCTOR"

        #endregion

        #region "PROPERTY"

        public string Descricao { get; private set; }

        public bool EhOrgao1 { get; private set; }

        public bool EhOrgao2 { get; private set; }

        public bool EhAdministrativo { get; private set; }

        public bool EhTributario { get; private set; }

        public bool EhTrabalhistaAdmin { get; private set; }

        public bool EhProvisionado { get; private set; }

        public bool EhPoloPassivoUnico { get; private set; }

        public bool EstaAtivo { get; private set; }

        public bool EhCriminalAdmin { get; private set; }

        public bool EhCivelAdmin { get; private set; }

        #endregion

        #region "RELATIONSHIP"
        public long? TipoParticipacao1Id { get; private set; }

        public TipoParticipacao TipoParticipacao1 { get; private set; }

        public long? TipoParticipacao2Id { get; private set; }

        public TipoParticipacao TipoParticipacao2 { get; private set; }
        #endregion

        #region "METHOD"
        public void Atualizar(string descricao)
        {
        }
        #endregion
    }
}
