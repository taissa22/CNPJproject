using Perlink.Oi.Juridico.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.ManutencaoFactory.Eventos
{

    public class EventoEstrategicoResultFactory : EventoFactory
    {
        public override QueryCreator CreateQuery(IDatabaseContext context)
        {
            return new EventoEstrategicoQueryCreator(context);
        }
    }
}
