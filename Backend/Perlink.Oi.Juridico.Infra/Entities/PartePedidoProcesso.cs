using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{ 
     public sealed class PartePedidoProcesso : Notifiable, IEntity, INotifiable
    {

        private PartePedidoProcesso()
        {
        }

        public int ProcessoId { get; private set; }
        public Processo Processo { get; private set; }

        public int ParteId { get; private set; }
        public Parte Parte { get; private set; }

        public int PedidoId { get; private set; }
        public Pedido Pedido { get; private set; }

        public int? CodOrientacaoJuridica { get; private set; }

        public int CodBaseCalculo { get; private set; }

        public int? CodMotivoProvavelZero { get; private set; }
    }
}
