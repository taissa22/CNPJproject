using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IIndicesVigenciasRepository
    {
        CommandResult<PaginatedQueryResult<IndiceCorrecaoProcesso>> ObterPaginado(int pagina, int quantidade,
          IndicesVigenciaSort sort, bool ascending, int tipoProcesso, string pesquisa = null, int vigencia = 0, bool exportar = false);

        CommandResult<PaginatedQueryResult<IndiceCorrecaoProcesso>> ObterPaginado(int pagina, int quantidade,
          IndicesVigenciaSort sort, bool ascending, DateTime DataInicio, DateTime DataFim,int tipoProcesso, string pesquisa = null, int vigencia = 0, bool exportar = false);

        CommandResult<bool> InserirIndiceVivencia(IndiceCorrecaoProcesso indiceVigencia);

        CommandResult<IReadOnlyCollection<IndiceCorrecaoProcesso>> ObterTodos();

        CommandResult<IReadOnlyCollection<Indice>> ObterInces(int tipoProcesso);

        CommandResult<IQueryable<IndiceCorrecaoProcesso>> ObterBase(IndicesVigenciaSort sort, bool ascending, DateTime inicio, DateTime fim, int tipoProcesso, string pesquisa = null, int vigencia = 0);

        CommandResult<IQueryable<IndiceCorrecaoProcesso>> ObterBase(IndicesVigenciaSort sort, bool ascending, int tipoProcesso, string pesquisa = null, int vigencia = 0);


    }
}
