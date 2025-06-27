using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities {

    public sealed class Feriado : Notifiable, IEntity, INotifiable {

        private Feriado() {
        }

        public int Id { get; private set; }
        public DateTime Data { get; private set; }
    }
}