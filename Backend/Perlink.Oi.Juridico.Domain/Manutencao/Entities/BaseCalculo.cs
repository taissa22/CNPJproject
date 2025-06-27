using Flunt.Validations;
using Shared.Domain.Impl.Entity;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Entities
{
    public class BaseCalculo : Entity
    {
        #region "ATTRIBUTE"

        #endregion

        #region "CONSTRUCTOR"
        public BaseCalculo(string descricao)
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(descricao, "BaseCalculo.Descricao", "Descrição é obrigatoria.")
                .HasMaxLen(descricao, 50, "BaseCalculo.Descricao", "Descrição só pode ter no máximo 50 caracteres.")
            );

            if (Valid)
            {
                Descricao = descricao;
                EhBaseInicial = false;
            }
        }
        #endregion

        #region "PROPERTY"

        public string Descricao { get; private set; }

        public bool EhBaseInicial { get; private set; }

        #endregion

        #region "RELATIONSHIP"

        #endregion

        #region "METHOD"
        public void Atualizar(string descricao)
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(descricao, "BaseCalculo.Descricao", "Descrição é obrigatoria.")
                .HasMaxLen(descricao, 50, "BaseCalculo.Descricao", "Descrição só pode ter no máximo 50 caracteres.")
            );

            if (Valid)
            {
                Descricao = descricao;
            }
        }

        public bool PodeMarcarBaseInicial(bool estaChecadoBaseInicial)
        {
            // Base de Cálculo que é base inicial e vem desmarcado => Dá erro dizendo que deve haver uma base inicial indicada
            if(EhBaseInicial && !estaChecadoBaseInicial)
            {
                AddNotification("BaseCalculo.AtualizacaoInvalida", "Deve haver uma base de cálculo inicial indicada no cadastro.");
                return false;
            }
            // Base de Cálculo que não é base inicial e vem marcado => Marca ele como base inicial e atualiza a antiga base inicial pra false
            else if (!EhBaseInicial && estaChecadoBaseInicial)
            {
                EhBaseInicial = true;
                return true;
            }

            // Base de Cálculo que é base inicial e vem marcado => Atualiza normalmente (EhBaseInicial && estaChecadoBaseInicial)
            // Base de Cálculo que não é base inicial e vem desmarcado => Atualiza normalmente (!EhBaseInicial && estaChecadoBaseInicial)
            return false;
        }

        public void DesmarcarBaseInicial()
        {
            EhBaseInicial = false;
        }
        #endregion
    }
}
