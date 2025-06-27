using Flunt.Validations;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Entities
{
    public class TipoParticipacao : Entity
    {
        #region "ATTRIBUTE"
        private IList<ParteProcesso> _partesProcesso;

        private IList<Procedimento> _procedimentos1;

        private IList<Procedimento> _procedimentos2;
        #endregion

        #region "CONSTRUCTOR"
        protected TipoParticipacao()
        {
            _partesProcesso = new List<ParteProcesso>();
            _procedimentos1 = new List<Procedimento>();
            _procedimentos2 = new List<Procedimento>();
        }

        public TipoParticipacao(string descricao)
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(descricao, "TipoParticipacao.Descricao", "Descrição é obrigatoria.")
                .HasMaxLen(descricao, 20, "TipoParticipacao.Descricao", "Descrição só pode ter no máximo 20 caracteres.")
            );

            if (Valid)
            {
                Descricao = descricao;

                _partesProcesso = new List<ParteProcesso>();
                _procedimentos1 = new List<Procedimento>();
                _procedimentos2 = new List<Procedimento>();
            }
        }
        #endregion

        #region "PROPERTY"

        public string Descricao { get; private set; }

        #endregion

        #region "RELATIONSHIP"
        public ICollection<ParteProcesso> PartesProcesso { get { return _partesProcesso.ToArray(); } }

        public ICollection<Procedimento> Procedimentos1 { get { return _procedimentos1.ToArray(); } }

        public ICollection<Procedimento> Procedimentos2 { get { return _procedimentos2.ToArray(); } }
        #endregion

        #region "METHOD"
        public void Atualizar(string descricao)
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(descricao, "TipoParticipacao.Descricao", "Descrição é obrigatoria.")
                .HasMaxLen(descricao, 20, "TipoParticipacao.Descricao", "Descrição só pode ter no máximo 20 caracteres.")
            );

            if (Valid)
            {
                Descricao = descricao;
            }
        }
        #endregion
    }
}
