using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class PedidoProcesso : Notifiable, IEntity, INotifiable
    {
        private PedidoProcesso()
        {
        }

        public static PedidoProcesso Criar(Processo processo, Pedido pedido) {
            var pedidoProcesso = new PedidoProcesso() {
                ProcessoId = processo.Id,
                Processo = processo,
                PedidoId = pedido.Id,
                Pedido = pedido
            };
            //TODO: Validate
            //PedidoProcesso.validate();
            return pedidoProcesso;
        }

        internal int PedidoId { get; private set; }
        public Pedido Pedido { get; private set; }

        public int ProcessoId { get; private set; }
        internal Processo Processo { get; private set; }

        public string Comentario { get; private set; }

        public bool AcessoRestrito { get; private set; }
    }
}