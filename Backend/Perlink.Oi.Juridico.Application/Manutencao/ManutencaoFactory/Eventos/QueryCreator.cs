using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.ManutencaoFactory.Eventos
{
    public abstract class QueryCreator
    {
        public QueryCreator(IDatabaseContext context)
        {
            Context = context;
        }

        protected IDatabaseContext Context { get; set; }
        public IQueryable<Evento> Query;
        public abstract IQueryable<Evento> GerarQuery(double TipoProcesso, EventoSort sort, IUsuarioAtualProvider UsuarioAtual, bool ascending, string pesquisa = null);
        public IQueryable<Evento> Sort(double TipoProcesso, EventoSort sort, IUsuarioAtualProvider UsuarioAtual, bool ascending, string pesquisa = null)
        {
            var query = Query;

            switch (sort)
            {
                case EventoSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case EventoSort.Descricao:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;

                case EventoSort.CumprimentoPrazo:
                    query = query.SortBy(a => a.EhPrazo, ascending);
                    break;

                case EventoSort.PossuiDecisao:
                    query = query.SortBy(a => a.PossuiDecisao, ascending);
                    break;

                case EventoSort.Notificar:
                    query = query.SortBy(a => a.NotificarViaEmail, ascending);
                    break;

                case EventoSort.Ativo:
                    query = query.SortBy(a => a.Ativo, ascending);
                    break;

                case EventoSort.PreencheMulta:
                    query = query.SortBy(a => a.PreencheMulta, ascending);
                    break;

                case EventoSort.Instancia:
                    query = query.SortBy(a => a.InstanciaId, ascending);
                    break;

                case EventoSort.FinalizacaoContabil:
                    query = query.SortBy(a => a.FinalizacaoContabil, ascending);
                    break;

                case EventoSort.EscritorioAtualiza:
                    query = query.SortBy(a => a.AtualizaEscritorio, ascending);
                    break;

                case EventoSort.AlteraExclui:
                    query = query.SortBy(a => a.AlterarExcluir, ascending);
                    break;

                case EventoSort.ReverCalculo:
                    if (ascending)
                    {
                        query = query.OrderBy(a => a.ReverCalculo == null ? 0 : 1).ThenBy(a => a.ReverCalculo);
                    }
                    else
                    {
                        query = query.OrderByDescending(a => a.ReverCalculo == null ? 0 : 1).ThenByDescending(a => a.ReverCalculo);
                    }
                    break;

                case EventoSort.FinalizacaoEscritorio:
                    if (ascending)
                    {
                        query = query.OrderBy(a => a.FinalizacaoEscritorio == null ? 0 : 1).ThenBy(a => a.FinalizacaoEscritorio);
                    }
                    else
                    {
                        query = query.OrderByDescending(a => a.FinalizacaoEscritorio == null ? 0 : 1).ThenByDescending(a => a.FinalizacaoEscritorio);
                    }
                    break;
            }          
           

            switch (TipoProcesso)
            {
                case 1:
                    query = query.Where(x => x.EhCivel.GetValueOrDefault() && UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EVENTO_CIVEL_CONSUMIDOR));
                    break;

                case 2:
                    query = query.Where(x => x.EhTrabalhista.GetValueOrDefault() && UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EVENTO_TRABALHISTA));
                    break;

                case 3:
                    query = query.Where(x => x.EhRegulatorio.GetValueOrDefault() && UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EVENTO_ADMINSTRATIVO));
                    break;


                case 4:
                    query = query.Where(x => x.EhTributarioAdm.GetValueOrDefault() && UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EVENTO_TRIBUTARIO_ADMINISTRATIVO));
                    break;

                case 5:
                    query = query.Where(x => x.EhTributarioJudicial.GetValueOrDefault() && UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EVENTO_TRIBUTARIO_JUDICIAL));
                    break;

                case 6:
                    query = query.Where(x => x.EhTrabalhistaAdm.GetValueOrDefault() && UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EVENTO_TRABALHISTA_ADMINISTRATIVO));
                    break;

                case 9:
                    query = query.Where(x => x.EhCivelEstrategico && UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EVENTO_CIVEL_ESTRATEGICO));
                    break;
            }

            query = query.WhereIfNotNull(x => x.Nome.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);

            return query;
        }


    }
}
