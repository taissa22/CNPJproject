using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class MigracaoPedidoConsumidor : Notifiable, IEntity, INotifiable
    {
        public MigracaoPedidoConsumidor()
        {

        }

        public MigracaoPedidoConsumidor(int codPedidoCivel, int codPedidoCivelEstrat)
        {
            CodPedidoCivel = codPedidoCivel;
            CodPedidoCivelEstrat = codPedidoCivelEstrat;
        }

        public static MigracaoPedidoConsumidor CriarMigracaoPedidoConsumidor(int codPedidoCivel, int? codPedidoCivelEstrat)
        {
            var MigracaoPedidoConsumidor = new MigracaoPedidoConsumidor()
            {
                CodPedidoCivel = codPedidoCivel,
                CodPedidoCivelEstrat = codPedidoCivelEstrat,
            };

            return MigracaoPedidoConsumidor;
        }

        public void AtualizarMigracaoPedidoConsumidor(int codPedidoCivel, int codPedidoCivelEstrat)
        {
            CodPedidoCivel = codPedidoCivel;
            CodPedidoCivelEstrat = codPedidoCivelEstrat;
        }


        public int? CodPedidoCivel { get; set; }
        public int? CodPedidoCivelEstrat { get; set; }
    }
}