using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class ValorProcesso : Notifiable, IEntity, INotifiable
    {
        private ValorProcesso()
        {
        }

        internal int ProcessoId { get; private set; }

        public int Sequencial { get; private set; }

        public int? COD_PARTE_EFETUOU { get; private set; }

        public int? COD_PARTE_LEVANTOU { get; private set; }
    }
}