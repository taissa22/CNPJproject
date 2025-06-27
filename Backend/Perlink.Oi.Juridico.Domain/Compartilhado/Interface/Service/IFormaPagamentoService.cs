using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFormaPagamento;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface IFormaPagamentoService : IBaseCrudService<FormaPagamento, long>
    {
        Task<IEnumerable<FormaPagamentoGridDTO>> GetFormaPagamentoGridManutencao(FormaPagamentoFiltroDTO filtros);
        Task<int> GetTotalFormaPagamentoGridManutencao(FormaPagamentoFiltroDTO filtros);
        Task<IEnumerable<FormaPagamentoGridExportarDTO>> GetFormaPagamentoGridExportarManutencao(FormaPagamentoFiltroDTO filtroDTO);

        Task<FormaPagamento> CadastrarFormaPagamento(FormaPagamento formaPagamento);
    }
}
