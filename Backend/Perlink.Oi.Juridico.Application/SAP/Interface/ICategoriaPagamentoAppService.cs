using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCategoriaPagamento;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface
{
    public interface ICategoriaPagamentoAppService : IBaseCrudAppService<CategoriaPagamentoViewModel, CategoriaPagamento, long>
    {
        Task<IResultadoApplication> ExcluirCategoria(long codigoCategoriaPagamento);
        Task<IResultadoApplication<ICollection<CategoriaDePagamentoResultadoViewModel>>> BuscarCategoriasPagamento(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO);
        Task<IResultadoApplication> CadastrarCategoriaPagamento(CategoriaPagamentoInclusaoEdicaoDTO categoriaPagamentoViewModel);
        Task<IResultadoApplication> AlterarCategoriaPagamento(CategoriaPagamentoInclusaoEdicaoDTO categoriaPagamentoViewModel);
        Task<IResultadoApplication<byte[]>> ExportarCategoriasPagamento(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO);
        Task<IResultadoApplication<CategoriaPagamentoPopupComboboxViewModel>> RecuperarInformacoesComboboxPopup(long tipoProcesso, long tipoLancamento);
        Task<IResultadoApplication<CategoriaMIgracaoEstrategicoPopupComboboxViewModel>> RecuperarInformacoesComboboxPopupEstrategico();

        Task<IResultadoApplication<CategoriaMIgracaoConsumidorPopupComboboxViewModel>> RecuperarInformacoesComboboxPopupConsumidor();
        


    }
}