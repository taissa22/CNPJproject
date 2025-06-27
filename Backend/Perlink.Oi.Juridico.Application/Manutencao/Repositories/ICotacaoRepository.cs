using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface ICotacaoRepository
    {
        CommandResult<PaginatedQueryResult<Cotacao>> ObterPaginado(int pagina, int quantidade, int codigoIndice,
          CotacaoSort sort, bool ascending, DateTime dataInicial, DateTime dataFinal, string pesquisa = null);

        CommandResult<IReadOnlyCollection<Cotacao>> Obter(int codigoIndice, CotacaoSort sort, bool ascending, DateTime dataInicial, DateTime dataFinal, string pesquisa = null);
    }
}