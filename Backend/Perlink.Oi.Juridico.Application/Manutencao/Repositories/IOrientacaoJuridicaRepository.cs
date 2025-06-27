using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IOrientacaoJuridicaRepository
    {        
        CommandResult<IReadOnlyCollection<OrientacaoJuridica>> Obter(bool obterTrabalhista, OrientacaoJuridicaSort sort, bool ascending, string pesquisa = null);

        CommandResult<PaginatedQueryResult<OrientacaoJuridica>> ObterPaginado(bool obterTrabalhista, OrientacaoJuridicaSort sort, bool ascending, int pagina, int quantidade, string pesquisa = null);
    }
}
