using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class PagamentoObjetoProcesso : Notifiable, IEntity, INotifiable
    {
        private PagamentoObjetoProcesso()
        {
        }

        internal int ProcessoId { get; private set; }
        internal int PedidoId { get; private set; }
        public int Sequencial { get; private set; }
        public DateTime DataInicial { get; private set; }
        public DateTime DataFinal { get; private set; }

        internal int? ParteId { get; private set; }
        public Parte Parte { get; private set; }
    }
}