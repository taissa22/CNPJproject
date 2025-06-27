using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.DTO.EstornoLancamentoPago;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Microsoft.Extensions.Logging;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Interface
{
    public interface ILancamentoProcessoAppService : IBaseCrudAppService<LancamentoProcessoViewModel, LancamentoProcesso, long>
    {
        Task<IResultadoApplication<ICollection<LancamentoProcessoViewModel>>> RecuperarPorParteNumeroGuia(long numeroGuia);
        Task<IResultadoApplication<long>> RecuperarLancamentoProcesso(long NumeroGuia, long CodigoTipoProcesso);
        Task<IResultadoApplication<ICollection<LoteLancamentoViewModel>>> ObterLancamentoDoLote(long codigoLote);
        Task<IResultadoApplication<byte[]>> ExportarLancamentoDoLote(long codigoLote, long codigoTipoProcesso);
        Task<IResultadoApplication<ICollection<LoteCriacaoResultadoLancamentoDTO>>>ObterLancamentoDoCriacao(long codigoLote);
        Task<IResultadoApplication<ICollection<LoteCriacaoResultadoEmpresaCentralizadoraViewModel>>> ObterLotesAgrupadoPorEmpresaCentralizadora(LoteCriacaoFiltroEmpresaCentralizadoraDTO filtros);
        Task<IPagingResultadoApplication<ICollection<LoteCriacaoResultadoEmpresaGrupoViewModel>>> ObterLotesAgrupadoPorEmpresadoGrupo(LoteCriacaoFiltroEmpresaGrupoDTO filtros);
        Task<int> ObterTotalLotesAgrupadoPorEmpresadoGrupo(LoteCriacaoFiltroEmpresaGrupoDTO filtros);
        Task<IResultadoApplication<ICollection<LoteCriacaoResultadoLancamentoViewModel>>> ObterLancamentosLoteCriacao(LoteCriacaoFiltroLancamentoDTO filtros, ILogger logger);
        Task<IResultadoApplication> AlterarDataEnvioEscritorio(List<LoteLancamentoViewModel> loteLancamentos);
        Task<IResultadoApplication<long?>> RecuperarNumeroContaJudicial(long NumeroContaJudicial);
        Task<IResultadoApplication<ICollection<DadosProcessoEstornoViewModel>>> ConsultaLancamentoEstorno( LancamentoEstornoFiltroDTO lancamentoEstornoFiltroViewModel);
        Task<IResultadoApplication> EstornaLancamento(DadosLancamentoEstornoDTO dadosLancamentoEstornoDTO);
    }
}