using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class FechamentoCivel : Notifiable, IEntity, INotifiable
    {
        private FechamentoCivel()
        {
        }

        public int CodigoEmpresaCentralizadora { get; private set; }
        public DateTime MesAno { get; private set; }
        public DateTime Data { get; private set; }
        public int TipoProcessoId { get; private set; }
    }
}