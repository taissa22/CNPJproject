using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class IndiceCorrecaoProcesso : Notifiable, IEntity, INotifiable
    {
        public IndiceCorrecaoProcesso()
        {

        }


        public int ProcessoId { get; set; }
        public DateTime DataVigencia { get; set; }
        public int IndiceId { get; set; }

        public Indice Indice { get; set; }

    }
}
