using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.ManutencaoFactory.Eventos
{
    public class EventoQueryCreator : QueryCreator
    {
        public EventoQueryCreator(IDatabaseContext context) : base(context)
        {
            Context = context;
        }
        public override IQueryable<Evento> GerarQuery(double TipoProcesso, EventoSort sort, IUsuarioAtualProvider UsuarioAtual, bool ascending, string pesquisa = null)
        {
            Query = Context.Eventos.AsNoTracking();
            return Sort(TipoProcesso, sort, UsuarioAtual, ascending, pesquisa);
        }

    }
}
