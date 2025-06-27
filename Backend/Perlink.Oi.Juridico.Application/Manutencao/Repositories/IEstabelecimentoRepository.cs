using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IEstabelecimentoRepository
    {
        CommandResult<IReadOnlyCollection<Estabelecimento>> Obter(EstabelecimentoSort sort, bool ascending, string nome);
        CommandResult<PaginatedQueryResult<Estabelecimento>> ObterPaginado(int pagina, int quantidade,
            EstabelecimentoSort sort, bool ascending, string nome);        
    }
}
