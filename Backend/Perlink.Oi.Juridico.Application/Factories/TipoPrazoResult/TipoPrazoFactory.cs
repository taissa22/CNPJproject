using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoPrazoResult
{
    public abstract class TipoPrazoFactory
    {
        public abstract QueryCreator CreateQuery(IDatabaseContext context);
        public static TipoPrazoFactory TipoPrazoCommandResult(TipoProcessoManutencao tipoProcesso)
        {
            switch (tipoProcesso.Id)
            {
                case 1:
                    return new TipoPrazoConsumidorResultFactory();
                case 9:
                    return new TipoPrazoEstrategicoResultFactory();
                default:
                    return new TipoPrazoResultFactory();
            }
        }
    }
}