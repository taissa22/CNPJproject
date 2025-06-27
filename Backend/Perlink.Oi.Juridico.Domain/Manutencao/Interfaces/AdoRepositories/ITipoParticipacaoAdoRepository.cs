using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Manutencao.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.AdoRepositories
{
    public interface ITipoParticipacaoAdoRepository
    {
        IEnumerable<FiltroTipoParticipacaoResultadoDTO> ObterTodosPorFiltro(string descricao,
                                                                            int pageNumber, int pageSize,
                                                                            List<SortOrder> orders, bool IsExportMethod);

        int GetTotalCount(string descricao);
    }
}
