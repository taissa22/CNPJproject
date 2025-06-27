using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.ManutencaoFactory.Eventos
{
    public abstract class EventoFactory
    {
        public abstract QueryCreator CreateQuery(IDatabaseContext context);

        public static EventoFactory EventoResult(TipoProcessoManutencao tipoProcesso)
        {
            switch (tipoProcesso.Id)
            {
                case 1:
                    return new EventoConsumidorResultFactory();
                case 9:
                    return new EventoEstrategicoResultFactory();//Aqui tem que adicionar o novo arquivo pra nova query            
                default:
                    return new EventoResultFactory();
            }
        }
    }
}
