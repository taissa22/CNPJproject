using Perlink.Oi.Juridico.Application.Manutencao.Results.TiposAudiencias;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.External;
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Factories.TipoAudienciaResult
{
    public abstract class QueryCreator
    {
        public QueryCreator(IDatabaseContext contextAudiencia)
        {
            ContextAudiencia = contextAudiencia;
        }

        protected IDatabaseContext ContextAudiencia { get; set; }
        public IQueryable<TipoAudienciaCommandResult> Query;
        public abstract IQueryable<TipoAudienciaCommandResult> GerarQuery(TipoAudienciaSort sort, bool ascending, string pesquisa = null);

        public IQueryable<TipoAudienciaCommandResult> Sort(TipoAudienciaSort sort, bool ascending, string pesquisa = null)
        {
            switch (sort)
            {
                case TipoAudienciaSort.Codigo:
                    Query = Query.SortBy(a => a.CodigoTipoAudiencia, ascending);
                    break;

                case TipoAudienciaSort.Ativo:
                    if (ascending)
                    {
                        Query = Query.OrderBy(a => a.Ativo).ThenBy(a => a.CodigoTipoAudiencia);
                    }
                    else
                    {
                        Query = Query.OrderByDescending(a => a.Ativo).ThenByDescending(a => a.CodigoTipoAudiencia);
                    }
                    break;

                case TipoAudienciaSort.DescricaoMigracao:
                    Query = Query.SortBy(x => x.DescricaoMigracao, ascending);
                    break;


                case TipoAudienciaSort.Descricao:
                default:
                    Query = Query.SortBy(a => a.Descricao, ascending);
                    break;
            }

            return Query.WhereIfNotNull(x => x.Descricao.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);

        }
    }
}
