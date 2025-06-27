using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoDocumentoResult
{
    public abstract class TipoDocumentoFactory
    {
        public abstract QueryCreator CreateQuery(IDatabaseContext context);

        public static TipoDocumentoFactory TipoDocumentoCommandResult(TipoProcesso tipoProcesso)
        {            
            switch (tipoProcesso.Id)
            {
                case 1 :
                    return new TipoDocConsumidorResultFactory();
                case 9: 
                    return new TipoDocEstrategicoResultFactory();
                default: 
                    return new TipoDocumentoResultFactory();                    
            }            
        }
    }
}
