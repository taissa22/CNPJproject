using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class PendenciaProcesso : Notifiable, IEntity, INotifiable
    {
        private PendenciaProcesso()
        {
        }

        internal int TipoPendenciaId { get; private set; }
        public TipoPendencia TipoPendencia { get; private set; }

        public int ProcessoId { get; private set; }
        internal Processo Processo { get; private set; }

        public int Sequencial { get; private set; }

        public DateTime DataPendencia { get; private set; }

        public DateTime DataBaixa { get; private set; }

        public string Comentario { get; private set; }
    }
}
