using Perlink.Oi.Juridico.Application.Factories.TipoAudienciaResult;
using Perlink.Oi.Juridico.Infra;

namespace Perlink.Oi.Juridico.Application.Factories.TipoAudienciaResult
{
    public class TipoAudienciaResultFactory : TipoAudienciaFactory
    {
        public override QueryCreator CreateQuery(IDatabaseContext contextAudiencia)
        {
            return new TipoAudienciaQueryCreator(contextAudiencia);
        }
    }
}