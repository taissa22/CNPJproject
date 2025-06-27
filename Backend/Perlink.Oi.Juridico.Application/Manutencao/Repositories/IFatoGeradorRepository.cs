using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Application.Manutencao.ViewModel;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IFatoGeradorRepository
    {
        CommandResult<PaginatedQueryResult<FatoGerador>> ObterPaginado(FatoGeradorSort sort, bool ascending, int pagina, int quantidade, string? pesquisa);
        CommandResult<IReadOnlyCollection<FatoGerador>> Obter(FatoGeradorSort sort, bool ascending, string? pesquisa);

    }
}