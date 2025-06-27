using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.ValueObjects;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Assunto : Notifiable, IEntity, INotifiable
    {
        private Assunto()
        {
        }

        /// <summary>
        /// Instancia um Assunto - <c>Cível Estratégico</c>
        /// </summary>
        /// <param name="descricao">Descrição do Assunto</param>
        /// <param name="ativo">Identificador de Ativo</param>
        public static Assunto CriarDoCivelEstrategico(string descricao, bool ativo)
        {
            var assunto = new Assunto()
            {
                Descricao = descricao,
                Ativo = ativo,
                EhCivelEstrategico = true               
            };
            assunto.Validate();
            return assunto;
        }

        /// <summary>
        /// Instancia um Assunto - <c>Cível Consumidor</c>
        /// </summary>
        /// <param name="descricao">Descrição do Assunto</param>
        /// <param name="ativo">Identificador de Ativo</param>
        /// <param name="negociacao">Descrição da Negociação</param>
        /// <param name="proposta">Descrição da Proposta</param>
        public static Assunto CriarDoCivelConsumidor(string descricao, bool ativo, string? negociacao, string? proposta, string? codTipoCalculoContingencia)
        {
            var assunto = new Assunto()
            {
                Descricao = descricao,
                Ativo = ativo,
                Negociacao = negociacao,
                Proposta = proposta,
                EhCivelConsumidor = true,
                CodTipoContingencia = codTipoCalculoContingencia

            };
            assunto.Validate();
            return assunto;
        }

        public int Id { get; private set; }

        public string Descricao { get; private set; }

        public bool Ativo { get; private set; } 

        public string Proposta { get; private set; } = null;

        public string Negociacao { get; private set; } = null;

        public bool EhCivelConsumidor { get; private set; }
        public bool EhCivelEstrategico { get; private set; }

        public string CodTipoContingencia { get; set; }       


        private void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "O Campo descrição é obrigatório.");
            }

            if (Descricao.Length > 40)
            {
                AddNotification(nameof(Descricao), "O Campo descrição pode conter no máximo 40 caracteres.");
            }
        }

        /// <summary>
        /// Atualiza o Assunto
        /// </summary>
        /// <param name="descricao">Descrição do Assunto</param>
        /// <param name="ativo">Identificador de Ativo</param>
        public void AtualizarDoCivelEstrategico(string descricao, bool ativo)
        {
            Descricao = descricao;
            Ativo = ativo;

            Validate();
        }

        /// <summary>
        /// Atualiza o Assunto - <c>Cível Consumidor</c>
        /// </summary>
        /// <param name="descricao">Descrição do Assunto</param>
        /// <param name="ativo">Identificador de Ativo</param>
        /// <param name="negociacao">Descrição da Negociação</param>
        /// <param name="proposta">Descrição da Proposta</param>
        public void AtualizarDoCivelConsumidor(string descricao, bool ativo, string? negociacao, string? proposta, string? codTipoCalculoContingencia)
        {
            Descricao = descricao;
            Ativo = ativo;
            Negociacao = negociacao;
            Proposta = proposta;
            CodTipoContingencia = codTipoCalculoContingencia;

            Validate();

            if (!EhCivelConsumidor)
            {
                AddNotification(nameof(EhCivelConsumidor), "Atualização inválida para o tipo de Assunto");
            }
        }
    }
}