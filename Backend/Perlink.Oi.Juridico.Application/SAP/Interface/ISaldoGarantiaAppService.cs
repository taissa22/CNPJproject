using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.SaldoGarantia;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface
{
    public interface ISaldoGarantiaAppService : IBaseCrudAppService<AgendamentoSaldoGarantiaViewModel, AgendamentoSaldoGarantia, long>
    {
        Task<IResultadoApplication<SaldoGarantiaFiltrosViewModel>> CarregarFiltros(long codigoTipoProcesso);
        Task ExecutarAgendamentoSaldoGarantia(ILogger logger);
        Task Expurgar(ILogger logger);
        Task<IPagingResultadoApplication<ICollection<AgendamentoResultadoViewModel>>> ConsultarAgendamento(OrdernacaoPaginacaoDTO ordernacaoPaginacaoDTO);
        Task<IResultadoApplication<ICollection<KeyValuePair<string, string>>>> ConsultarCriteriosPesquisa(long codigoAgendamento);
        Task<IResultadoApplication> SalvarAgendamento(SaldoGarantiaAgendamentoDTO filtroDTO);
        Task<IResultadoApplication> ExcluirAgendamento(long codigoAgendamento);
        Task<Stream> DownloadSaldoGarantia(string filePath);
    }
}
