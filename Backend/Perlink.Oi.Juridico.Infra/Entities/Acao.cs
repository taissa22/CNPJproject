using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.ValueObjects;

namespace Perlink.Oi.Juridico.Infra.Entities {

    public sealed class Acao : Notifiable, IEntity, INotifiable {

        private Acao() {
        }

        /// <summary>
        /// Instancia uma Ação - <c>Tributária</c>
        /// </summary>
        /// <param name="descricao">Descrição da Ação</param>
        public static Acao CriarDoTributario(DataString descricao) {
            var acao = new Acao() {
                Descricao = descricao,
                Ativo = true,
                EhTributaria = true
            };
            acao.Validate();
            return acao;
        }

        /// <summary>
        /// Instancia uma Ação - <c>Trabalhista</c>
        /// </summary>
        /// <param name="descricao">Descrição da Ação</param>
        public static Acao CriarDoTrabalhista(DataString descricao) {
            var acao = new Acao() {
                Descricao = descricao,
                Ativo = true,
                EhTrabalhista = true
            };
            acao.Validate();
            return acao;
        }

        /// <summary>
        /// Instancia uma Ação - <c>Cível Estratégica</c>
        /// </summary>
        /// <param name="descricao">Descrição da Ação</param>
        /// <param name="ativo">Identificador de Ativo</param>
        public static Acao CriarDoCivelEstrategico(DataString descricao, bool ativo) {
            var acao = new Acao() {
                Descricao = descricao,
                Ativo = ativo,
                EhCivelEstrategico = true,
            };
            acao.Validate();
            return acao;
        }

        /// <summary>
        /// Instancia uma Ação - <c>Cível Consumidor</c>
        /// </summary>
        /// <param name="descricao">Descrição da Ação</param>
        /// <param name="naturezaAcaoBB">Natureza da Ação no BB</param>
        public static Acao CriarDoCivelConsumidor(DataString descricao, NaturezaAcaoBB naturezaAcaoBB, bool? enviarAppPreposto) {
            var acao = new Acao() {
                Descricao = descricao,
                NaturezaAcaoBBId = naturezaAcaoBB != null ? naturezaAcaoBB.Id : (int?)null,
                NaturezaAcaoBB = naturezaAcaoBB,
                Ativo = true,
                EhCivelConsumidor = true,
                EnviarAppPreposto = enviarAppPreposto
            };
            acao.Validate();
            return acao;
        }

        public int Id { get; private set; }

        public string Descricao { get; private set; }

        public bool Ativo { get; private set; }      

        internal int? NaturezaAcaoBBId { get; private set; } = null;
        public NaturezaAcaoBB NaturezaAcaoBB { get; private set; } = null;

        public bool EhCivelEstrategico { get; private set; }
        public bool EhCivelConsumidor { get; private set; }
        public bool EhTrabalhista { get; private set; }
        public bool EhTributaria { get; private set; }
        public bool? EnviarAppPreposto { get; set; }

        private void Validate() {
            if (string.IsNullOrEmpty(Descricao)) {
                AddNotification(nameof(Descricao), "O Campo descrição é obrigatório.");
            }

            if (Descricao.Length > 30) {
                AddNotification(nameof(Descricao), "O Campo descrição pode conter no máximo 30 caracteres.");
            }
            //TODO: VALIDAR
            //if (EhCivelConsumidor)
            //{
            //    AddNotifications(NaturezaAcaoBB);
            //}
        }

        /// <summary>
        /// Atualiza a Ação - <c>Trabalhista</c>
        /// </summary>
        /// <param name="descricao">Descrição da Ação</param>
        public void AtualizarDoTrabalhista(DataString descricao) {
            Descricao = descricao;

            if (!EhTrabalhista) {
                AddNotification(nameof(EhTrabalhista), "Atualização inválida para o tipo de Ação");
            }

            Validate();
        }

        /// <summary>
        /// Atualiza a Ação - <c>Tributária</c>
        /// </summary>
        /// <param name="descricao">Descrição da Ação</param>
        public void AtualizarDoTributario(DataString descricao) {
            Descricao = descricao;

            if (!EhTributaria) {
                AddNotification(nameof(EhTributaria), "Atualização inválida para o tipo de Ação");
            }

            Validate();
        }

        /// <summary>
        /// Atualiza a Ação - <c>Cível Estratégica</c>
        /// </summary>
        /// <param name="descricao">Descrição da Ação</param>
        /// <param name="ativo">Identificador de Ativo</param>
        public void AtualizarDoCivelEstrategico(DataString descricao, bool ativo) {
            Descricao = descricao;
            Ativo = ativo;

            if (!EhCivelEstrategico) {
                AddNotification(nameof(EhCivelEstrategico), "Atualização inválida para o tipo de Ação");
            }

            Validate();
        }

        /// <summary>
        /// Atualiza uma Ação - <c>Cível Consumidor</c>
        /// </summary>
        /// <param name="descricao">Descrição da Ação</param>
        /// <param name="naturezaAcaoBB">Natureza da Ação no BB</param>
        public void AtualizarDoCivelConsumidor(DataString descricao, NaturezaAcaoBB naturezaAcaoBB, bool? enviarAppPreposto) {
            NaturezaAcaoBBId = naturezaAcaoBB != null ? naturezaAcaoBB.Id : (int?)null;
            NaturezaAcaoBB = naturezaAcaoBB;
            Descricao = descricao;
            EnviarAppPreposto = enviarAppPreposto;

            if (!EhCivelConsumidor) {
                AddNotification(nameof(EhCivelConsumidor), "Atualização inválida para o tipo de Ação");
            }

            Validate();
        }
    }
}