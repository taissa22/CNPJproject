using Perlink.Oi.Juridico.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoAudienciaResult
{
    public class TipoAudienciaConsumidorResultFactory : TipoAudienciaFactory
    {
        public override QueryCreator CreateQuery(IDatabaseContext context)
        {
            return new TipoAudienciaConsumidorQueryCreator(context);
        }
    }
}
