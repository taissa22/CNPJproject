using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{

    public sealed class StatusAudiencia : Notifiable, IEntity, INotifiable {

        private StatusAudiencia() {
        }

        public static StatusAudiencia Criar(int id, string descricao) {
            var statusAudiencia = new StatusAudiencia() {
                Id = id,
                Descricao = descricao
            };
            return statusAudiencia;
        }

        public int Id { get; private set; }

        public string Descricao { get; private set; } = string.Empty;
    }
}