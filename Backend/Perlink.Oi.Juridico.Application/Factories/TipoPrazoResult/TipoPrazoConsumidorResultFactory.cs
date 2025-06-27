using Perlink.Oi.Juridico.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoPrazoResult
{
    public class TipoPrazoConsumidorResultFactory : TipoPrazoFactory
    {
        public override QueryCreator CreateQuery(IDatabaseContext context)
        {
            return new TipoPrazoConsumidorQueryCreator(context);
        }
    }
}

