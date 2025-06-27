using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities.Internal
{
    public sealed class AcessoRecenteProcesso : Notifiable, IEntity, INotifiable {

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private AcessoRecenteProcesso() {
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        }

        public int Id { get; private set; }

        public DateTime UltimoAcesso { get; private set; }

        internal int ProcessoId { get; private set; }
        public Processo Processo { get; private set; }

        public string Usuario { get; private set; }

        public void AtualizaUltimoAcesso() {
            UltimoAcesso = DateTime.Now;
        }
    }
}
