using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoFormaPagamento;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFormaPagamento;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Interface
{
    public interface IFormaPagamentoAppService : IBaseCrudAppService<FormaPagamentoViewModel, FormaPagamento, long>
    {
        Task<IPagingResultadoApplication<IEnumerable<FormaPagamentoGridViewModel>>> GetFormaPagamentoGridManutencao(FormaPagamentoFiltroDTO filtros);
        Task<int> GetTotalFormaPagamentoGridManutencao(FormaPagamentoFiltroDTO filtros);
        Task<IResultadoApplication> ExcluirFormaPagamentoComAssociacao(long id);
        Task<IResultadoApplication<byte[]>> ExportarFormasPagamento(FormaPagamentoFiltroDTO filtroDTO);
        Task<IResultadoApplication> SalvarFormaPagamento(FormaPagamentoInclusaoEdicaoDTO inclusaoEdicaoDTO);
    }
}