using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class MigracaoPedido : Notifiable, IEntity, INotifiable
    {
        public MigracaoPedido()
        {

        }

        public MigracaoPedido(int codPedidoCivelEstrat, int codPedidoCivelConsumidor)
        {
            CodPedidoCivelEstrat = codPedidoCivelEstrat;
            CodPedidoCivelConsumidor = codPedidoCivelConsumidor;                
        }

        public static MigracaoPedido CriarMigracaoPedido(int? codPedidoCivelEstrat, int codPedidoCivelConsumidor)
        {
            var MigracaoPedido = new MigracaoPedido()
            {
                CodPedidoCivelEstrat = codPedidoCivelEstrat,
                CodPedidoCivelConsumidor = codPedidoCivelConsumidor,
            };

            return MigracaoPedido;
        }

        public void AtualizarMigracaoPedido(int codPedidoCivelEstrat, int codPedidoCivelConsumidor)
        {
            CodPedidoCivelEstrat = codPedidoCivelEstrat;
            CodPedidoCivelConsumidor = codPedidoCivelConsumidor;
        }


        public int? CodPedidoCivelEstrat { get; set; }
        public int? CodPedidoCivelConsumidor { get; set; }
    }
}
