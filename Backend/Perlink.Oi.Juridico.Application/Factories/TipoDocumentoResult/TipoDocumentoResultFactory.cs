using Perlink.Oi.Juridico.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoDocumentoResult
{
    public class TipoDocumentoResultFactory : TipoDocumentoFactory
    {
        public override QueryCreator CreateQuery(IDatabaseContext context)
        {
            return new TipoDocumentoQueryCreator(context);
        }
    }
}
