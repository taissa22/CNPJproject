using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Manutencao.DTO;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.AdoRepositories
{
    public interface IBaseCalculoAdoRepository
    {
        IEnumerable<FiltroBaseCalculoResultadoDTO> ObterTodosPorFiltro(string descricao,
                                                                       int pageNumber, int pageSize,
                                                                       List<SortOrder> orders, bool IsExportMethod);

        int GetTotalCount(string descricao);
    }
}
