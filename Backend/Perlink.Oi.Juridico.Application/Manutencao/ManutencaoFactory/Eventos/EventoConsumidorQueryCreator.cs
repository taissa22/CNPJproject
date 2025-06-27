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
   public class EventoConsumidorQueryCreator : QueryCreator
        
    {
        public EventoConsumidorQueryCreator(IDatabaseContext context) : base(context)
        {
            Context = context;
        }
        public override IQueryable<Evento> GerarQuery(double TipoProcesso, EventoSort sort, IUsuarioAtualProvider UsuarioAtual, bool ascending, string pesquisa = null)
        { 

            var listaEstrategicoMigracao = Context.Eventos.Where(x => x.EhCivelEstrategico).Select(y => new { y.Id, y.Nome, y.Ativo }).AsNoTracking().ToArray();

            Query = from a in Context.Eventos.AsNoTracking()
                    join ma in Context.MigracaoEventosCivelConsumidor on a.Id equals ma.EventoCivelConsumidorId into LeftJoinMa
                    from ma in LeftJoinMa.DefaultIfEmpty()
                    where a.EhCivel.Value
                    select new Evento(
                        a.Id,
                        a.Nome,
                        a.PossuiDecisao,
                        a.EhCivel,
                        a.EhTrabalhista,
                        a.EhRegulatorio,
                        a.EhPrazo,
                        a.EhTributarioAdm,
                        a.EhTributarioJudicial,
                        a.FinalizacaoEscritorio,
                        a.TipoMulta,
                        a.EhTrabalhistaAdm,
                        a.NotificarViaEmail,
                        a.EhJuizado,
                        a.ReverCalculo,
                        a.AtualizaEscritorio,
                        a.EhCivelEstrategico,
                        a.InstanciaId,
                        a.PreencheMulta,
                        a.AlterarExcluir,
                        a.Ativo,
                        a.EhCriminalJudicial,
                        a.EhCriminalAdm,
                        a.EhCivelAdm,
                        a.ExigeComentario,
                        a.FinalizacaoContabil,
                        a.EhProcon,
                        a.EhPexJuizado,
                        a.EhPexCivelConsumidor,
                        a.SequencialDecisao,
                        (int?)ma.EventoCivelEstrategicoId,
                        (int?)ma.EventoCivelEstrategicoId == null ? null : listaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.EventoCivelEstrategicoId).Nome,
                        (int?)ma.EventoCivelEstrategicoId == null ? false : listaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.EventoCivelEstrategicoId) != null ? listaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.EventoCivelEstrategicoId).Ativo : false,
                        null,
                        null,
                        null

                    );

            return Sort(TipoProcesso, sort, UsuarioAtual, ascending, pesquisa);
        }
    }
}
