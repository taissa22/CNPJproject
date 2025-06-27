using Perlink.Oi.Juridico.Application.Manutencao.Results.Tipo_Documento;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Factories.TipoDocumentoResult
{
    public abstract class QueryCreator
    {
        public QueryCreator(IDatabaseContext context)
        {
            Context = context;
        }

        protected IDatabaseContext Context { get; set; }
        public IQueryable<TipoDocumentoCommandResult> Query;
        public abstract IQueryable<TipoDocumentoCommandResult> GerarQuery(TipoDocumentoSort sort, bool ascending, string pesquisa = null);


        public IQueryable<TipoDocumentoCommandResult> Sort(TipoDocumentoSort sort, bool ascending, string pesquisa = null)
        {
            switch (sort)
            {
                case TipoDocumentoSort.Codigo:
                    Query = Query.SortBy(a => a.Id, ascending);
                    break;

                case TipoDocumentoSort.Ativo:
                    if (ascending)
                    {
                        Query = Query.OrderBy(a => a.Ativo).ThenBy(a => a.Id);
                    }
                    else
                    {
                        Query = Query.OrderByDescending(a => a.Ativo).ThenByDescending(a => a.Id);
                    }
                    break;

                case TipoDocumentoSort.DescricaoMigracao:
                    Query = Query.SortBy(x => x.DescricaoMigracao, ascending);
                    break;


                case TipoDocumentoSort.Descricao:
                default:
                    Query = Query.SortBy(a => a.Descricao, ascending);
                    break;
            }

            return Query.WhereIfNotNull(x => x.Descricao.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);

        }
    }
}
