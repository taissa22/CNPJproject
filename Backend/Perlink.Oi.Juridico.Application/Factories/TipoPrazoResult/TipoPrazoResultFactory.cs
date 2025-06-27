using Perlink.Oi.Juridico.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoPrazoResult
{
    public class TipoPrazoResultFactory : TipoPrazoFactory
    {
        public override QueryCreator CreateQuery(IDatabaseContext contextPrazo)
        {
            return new TipoPrazoQueryCreator(contextPrazo);
        }
    }
}