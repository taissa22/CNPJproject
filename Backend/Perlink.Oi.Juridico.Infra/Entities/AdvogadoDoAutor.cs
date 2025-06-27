using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using Perlink.Oi.Juridico.Infra.ValueObjects;

namespace Perlink.Oi.Juridico.Infra.Entities
{

    public sealed class AdvogadoDoAutor : Notifiable, IEntity, INotifiable {

        private AdvogadoDoAutor() {
        }

        public static AdvogadoDoAutor Criar(int profissionalId, DataString? comentario) {
            var advogadoDoAutor = new AdvogadoDoAutor() {
                ProfissionalId = profissionalId,
                Descricao = comentario
            };
            advogadoDoAutor.Validate();
            return advogadoDoAutor;
        }

        public int ProcessoId { get; private set; }

        internal Processo Processo { get; private set; }

        public int ProfissionalId { get; private set; }
        public string Descricao { get; private set; }

        public void Validate() {
            ClearNotifications();
            if (!Descricao.HasMaxLength(4000)) {
                AddNotification(nameof(Descricao), "O Comentário permite no máximo 4000 caracteres.");
            }
        }

        internal void AdicionarProcesso(Processo processo) {
            Processo = processo;
            ProcessoId = processo.Id;
            Validate();
        }
    }
}