using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Processos.DTO;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Processos.Interface.AdoRepository
{
    public interface IAudienciaProcessoAdoRepository
    {
        IEnumerable<AudienciaProcessoResultadoDTO> ObterTodosPorFiltro(List<Filter> lstFilters, int pageNumber, int pageSize, List<SortOrder> orders, bool IsExportMethod);

        IEnumerable<AudienciaProcessoCompletoResultadoDTO> ObterTodosCompletoPorFiltro(List<Filter> lstFilters, int pageNumber, int pageSize, List<SortOrder> orders, bool IsExportMethod);

        string ObterQueryRelatorio(List<Filter> lstFilters, int pageNumber, int pageSize, List<SortOrder> orders, bool IsExportMethod);

        int GetTotalCount(List<Filter> lstFilters);
    }
}
