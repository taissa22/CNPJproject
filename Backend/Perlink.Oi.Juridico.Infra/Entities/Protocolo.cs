using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class Protocolo : Notifiable, IEntity, INotifiable
    {
        private Protocolo()
        {
        }

        public int Id { get; private set; }

        public int? ProfissionalId { get; private set; }

        public int? CodTipoDocumento { get; private set; }

        public int? ComarcaId { get; private set; }
        public int? VaraId { get; private set; }
        public int? TipoVaraId { get; private set; }
        public int? TipoProcessoId { get; private set; }
        public TipoProcesso TipoProcesso => TipoProcesso.PorId(TipoProcessoId.GetValueOrDefault());

    }
}