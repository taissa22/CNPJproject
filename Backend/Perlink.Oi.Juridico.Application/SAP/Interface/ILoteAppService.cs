using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface
{
    public interface ILoteAppService : IBaseCrudAppService<LoteViewModel, Lote, long>
    {
        Task<IResultadoApplication<string>> RecuperarPorCodigoSAP(long codigo);

        Task<IResultadoApplication<string>> RecuperarPorNumeroSAPCodigoTipoProcesso(long NumeroPedidoSAP, long CodigoTipoProcesso);

        Task<IResultadoApplication<ICollection<LoteViewModel>>> RecuperarPorDataCriacaoLote(DateTime DataCriacaoMin, DateTime DataCriacaoMax);

        Task<IResultadoApplication<LoteDetalhesViewModel>> RecuperaDetalhes(long CodigoParte);
        Task<IResultadoApplication<CompromissoLoteViewModel>> VerificarCompromisso(long CodigoParte);

        Task<IResultadoApplication<long?>> AtualizarNumeroLoteBBAsync(long CodigoParte);
        Task<IResultadoApplication<ICollection<LoteViewModel>>> RecuperarPorDataPedidoCriacaoLote(DateTime DataCriacaoPedidoMin, DateTime DataCriacaoPedidoMax);

        Task<IResultadoApplication<ICollection<LoteViewModel>>> RecuperarPorParteCodigoSAP(long CodigoSapParte);

        Task<IPagingResultadoLoteApplication<ICollection<LoteResultadoViewModel>>> RecuperarPorFiltro(LoteFiltroDTO loteFiltroDTO);
        Task<IPagingResultadoLoteApplication<ICollection<LoteResultadoViewModel>>> ObterQuantidadeTotalPorFiltro(LoteFiltroDTO loteFiltroDTO);
        Task<IResultadoApplication<byte[]>> Exportar(LoteFiltroDTO loteFiltroDTO, long codigoTipoProcesso);
        Task<IResultadoApplication<LoteCriteriosFiltrosViewModel>> CarregarFiltros(long codigoTipoProcesso);
        Task<IResultadoApplication<LoteCanceladoViewModel>> CancelamentoLote(LoteCancelamentoDTO loteCancelamento);
        Task<IResultadoApplication> CriarLote(LoteCriacaoViewModel loteCriacaoViewModel);
        IResultadoApplication<dynamic> RegerarArquivoBB(ArquivoBBDTO regerarArquivoBBDTO);
        Task<IResultadoApplication<long>> ValidarNumeroLote(long numeroLote, long codigoTipoProcesso);
    }
}