using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Enums;

namespace Perlink.Oi.Juridico.Application.Factories.TipoAudienciaResult
{
    public abstract class TipoAudienciaFactory
    {
        public abstract QueryCreator CreateQuery(IDatabaseContext context);
        public static TipoAudienciaFactory TipoAudienciaCommandResult(TipoProcesso tipoProcesso)
        {
            switch (tipoProcesso.Id)
            {
                case 1:
                    return new TipoAudienciaConsumidorResultFactory();
                case 9:
                    return new TipoAudienciaEstrategicoResultFactory();
                default:
                    return new TipoAudienciaResultFactory();
            }
        }
    }
}