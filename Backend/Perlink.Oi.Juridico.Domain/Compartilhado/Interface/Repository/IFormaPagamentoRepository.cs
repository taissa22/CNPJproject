using System.Collections.Generic;
using System.Threading.Tasks;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFormaPagamento;
using Shared.Domain.Interface.Repository;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IFormaPagamentoRepository : IBaseCrudRepository<FormaPagamento, long>
    {
        Task<IEnumerable<FormaPagamentoGridDTO>> GetFormaPagamentoGridManutencao(FormaPagamentoFiltroDTO filtros);
        Task<int> GetTotalFormaPagamentoGridManutencao(FormaPagamentoFiltroDTO filtros);
        Task<IEnumerable<FormaPagamentoGridExportarDTO>> GetFormaPagamentoGridExportarManutencao(FormaPagamentoFiltroDTO filtros);

        Task<FormaPagamento> CadastrarFormaPagamento(FormaPagamento formaPagamento);
    }
}
