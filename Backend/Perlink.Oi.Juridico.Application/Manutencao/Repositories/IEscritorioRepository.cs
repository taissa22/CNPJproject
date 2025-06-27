using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IEscritorioRepository
    {
        CommandResult<PaginatedQueryResult<Escritorio>> ObterPaginado(string estado, int areaAtuacao, int pagina, int quantidade, EscritorioSort sort, bool ascending, string pesquisa);

        CommandResult<IReadOnlyCollection<Escritorio>> Obter(string estado, int areaAtuacao, EscritorioSort sort, bool ascending, string pesquisa);
    }
}
