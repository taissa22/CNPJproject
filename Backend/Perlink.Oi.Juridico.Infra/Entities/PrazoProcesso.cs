using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class PrazoProcesso : Notifiable, IEntity, INotifiable
    {
        private PrazoProcesso()
        {
        }

        internal int TipoPrazoId { get; private set; }
        public TipoPrazo TipoPrazo { get; private set; }

        public int ProcessoId { get; private set; }
        internal Processo Processo { get; private set; }

        public int Sequencial { get; private set; }

        public DateTime DataPrazo { get; private set; }

        public DateTime HoraPrazo { get; private set; }

        public int SequencialServico { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public DateTime DataCumprimentoPrazo { get; private set; }

        public string Comentario { get; private set; }
    }
}

