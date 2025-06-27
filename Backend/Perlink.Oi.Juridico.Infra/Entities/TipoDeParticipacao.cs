using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class TipoDeParticipacao : Notifiable, IEntity, INotifiable
    {
        private TipoDeParticipacao()
        {
        }

        /// <summary>
        /// Instancia uma Ação - <c>Tributária</c>
        /// </summary>
        /// <param name="descricao">Descrição da Ação</param>
        public static TipoDeParticipacao Criar(string descricao)
        {
            var tipoDeParticipacao = new TipoDeParticipacao();
            tipoDeParticipacao.Descricao = descricao.ToUpper();

            tipoDeParticipacao.Validate();
            return tipoDeParticipacao;
        }

        public void Atualizar(string descricao)
        {
            Descricao = descricao.ToUpper();

            Validate();
        }

        public int Codigo { get; private set; }

        public string Descricao { get; private set; }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "O Campo descrição é obrigatório.");
            }

            if (Descricao.Length > 20)
            {
                AddNotification(nameof(Descricao), "O Campo descrição pode conter no máximo 20 caracteres.");
            }
        }
    }
}