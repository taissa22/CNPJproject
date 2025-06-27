using Perlink.Oi.Juridico.Application.Manutencao.Results.Tipo_Documento;
using Perlink.Oi.Juridico.Application.Manutencao.Results.TipoPrazo;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoPrazoResult
{
    public abstract class QueryCreator
    {
        public QueryCreator(IDatabaseContext contextPrazo)
        {
            ContextPrazo = contextPrazo;
        }

        protected IDatabaseContext ContextPrazo { get; set; }
        public IQueryable<TipoPrazoCommandResult> Query;
        public abstract IQueryable<TipoPrazoCommandResult> GerarQuery(TipoPrazoSort sort, bool ascending, string pesquisa = null);

        public IQueryable<TipoPrazoCommandResult> Sort(TipoPrazoSort sort, bool ascending, string pesquisa = null)
        {
            switch (sort)
            {
                case TipoPrazoSort.Codigo:
                    Query = Query.SortBy(a => a.Id, ascending);
                    break;

                case TipoPrazoSort.Ativo:
                    if (ascending)
                    {
                        Query = Query.OrderBy(a => a.Ativo).ThenBy(a => a.Id);
                    }
                    else
                    {
                        Query = Query.OrderByDescending(a => a.Ativo).ThenByDescending(a => a.Id);
                    }
                    break;

                case TipoPrazoSort.DescricaoMigracao:
                    Query = Query.SortBy(x => x.DescricaoMigracao, ascending);
                    break;


                case TipoPrazoSort.Descricao:
                default:
                    Query = Query.SortBy(a => a.Descricao, ascending);
                    break;
            }

            return Query.WhereIfNotNull(x => x.Descricao.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);

        }
    }
}
