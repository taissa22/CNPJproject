using Perlink.Oi.Juridico.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoDocumentoResult
{
    public class TipoDocEstrategicoResultFactory : TipoDocumentoFactory
    {
        public override QueryCreator CreateQuery(IDatabaseContext context)
        {
            return new TipoDocEstrategicoQueryCreator(context);
        }
    }
}
