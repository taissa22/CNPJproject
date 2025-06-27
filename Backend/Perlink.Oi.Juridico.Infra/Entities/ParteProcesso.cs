using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities {

    public sealed class ParteProcesso : Notifiable, IEntity, INotifiable {

        private ParteProcesso() {
        }

        public int ProcessoId { get; private set; }
        public Processo Processo { get; private set; }

        public int ParteId { get; private set; }
        public Parte Parte { get; private set; }

        public int TipoParticipacaoId { get; private set; }
    }
}