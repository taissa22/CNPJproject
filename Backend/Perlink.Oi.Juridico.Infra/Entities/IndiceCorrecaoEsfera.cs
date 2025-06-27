using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class IndiceCorrecaoEsfera : Notifiable, IEntity, INotifiable
    {
        private IndiceCorrecaoEsfera()
        {
        }

        public static IndiceCorrecaoEsfera Criar(int esferaId, DateTime dataVigencia, int indiceId)
        {
            IndiceCorrecaoEsfera indice = new IndiceCorrecaoEsfera();

            indice.DataVigencia = dataVigencia;
            indice.EsferaId = esferaId;
            indice.IndiceId = indiceId;
            indice.Validate();
            return indice;
        }       

        public int EsferaId { get; private set; }
        public DateTime DataVigencia { get; private set; }
        public int IndiceId { get; private set; }
        public Indice Indice { get; set; }

        public void Validate()
        {
            if (EsferaId <= 0)
            {
                AddNotification(nameof(EsferaId), "Campo requerido");
            }

            if (IndiceId < 0)
            {
                AddNotification(nameof(IndiceId), "Campo requerido");
            }

            if (DataVigencia == null)
            {
                AddNotification(nameof(DataVigencia), "Campo requerido");
            }
        }

    }
}
