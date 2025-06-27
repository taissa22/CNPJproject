using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Application.Manutencao.ViewModel;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IObjetoRepository
    {
        CommandResult<PaginatedQueryResult<ObjetoViewModel>> ObterPaginado(int tipoProcesso, ObjetoSort sort, bool ascending, int pagina, int quantidade, string descricao);
        CommandResult<IReadOnlyCollection<ObjetoViewModel>> Obter(int tipoProcesso, ObjetoSort sort, bool ascending, string descricao);

    }
}