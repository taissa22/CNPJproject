using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Manutencao.DTO;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.AdoRepositories
{
    public interface ITipoAudienciaAdoRepository
    {
        IEnumerable<FiltroTipoAudienciaResultadoDTO> ObterTodosPorFiltro(long? codTipoProcesso, string descricao, 
                                                                         int pageNumber, int pageSize, 
                                                                         List<SortOrder> orders, bool IsExportMethod);

        int GetTotalCount(long? codTipoProcesso, string descricao);
    }
}
