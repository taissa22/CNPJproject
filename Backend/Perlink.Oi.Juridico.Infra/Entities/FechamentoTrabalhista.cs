using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class FechamentoTrabalhista : Notifiable, IEntity, INotifiable
    {
        private FechamentoTrabalhista()
        {
        }

        public int CodigoEmpresaCentralizadora { get; private set; }
        public DateTime MesAno { get; private set; }
        public DateTime Data { get; private set; }
    }
}