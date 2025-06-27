using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarIndiceCorrecaoEsferaCommand : Validatable, IValidatable
    {
        public int EsferaId { get;  set; }
        public DateTime DataVigencia { get;  set; }
        public int IndiceId { get;  set; }

        public override void Validate()
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