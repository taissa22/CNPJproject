using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCategoriaPagamento;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System.Threading.Tasks;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using System.Collections.Generic;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento;

namespace Perlink.Oi.Juridico.WebApi.Areas.SAP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CategoriaPagamentoController : JuridicoControllerBase
    {
        private readonly ICategoriaPagamentoAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public CategoriaPagamentoController(ICategoriaPagamentoAppService appService)
        {
            this.appService = appService;
        }
        
        /// <summary>
        /// Exclui uma categoria de pagamento com base no ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExcluirCategoriaPagamento")]
        public async Task<IResultadoApplication> ExcluirCategoriaPagamento(long codigoCategoriaPagamento)
        {
            var result = await appService.ExcluirCategoria(codigoCategoriaPagamento);
            return result;
        }

        ///<summary>
        ///Recupera as informações de tipo processo e tipo lançamento
        ///</summary>
        ///<returns>
        ///Retorna listas com as informações necessárias para o preenchimento das combobox de tipo processo e tipo lançamento.
        ///</returns>
        /// <param name="tipoProcesso">Tipo de Processo</param>
      

        ///<summary>
        ///Recupera as informações de categorias de pagamento
        ///</summary>
        ///<returns>
        ///Retorna listas com as informações necessárias para as categorias de pagaemnto
        ///</returns>
        /// <param name="categoriaPagamentoFiltroDTO">Filtro contendo informações necessárias p/ ordenação.</param>
        [HttpPost("BuscarCategoriasPagamento")]
        public async Task<IResultadoApplication<ICollection<CategoriaDePagamentoResultadoViewModel>>> BuscarCategoriasPagamento(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO)
        {
            var resultado = await appService.BuscarCategoriasPagamento(categoriaPagamentoFiltroDTO);
            return resultado;
        }

        ///<summary>
        ///Recupera as informações de FornecedoresPermitidos, ClassesGarantias e GrupoCorrecao
        ///</summary>
        ///<returns>
        ///Retorna listas com as informações necessárias para o preenchimento das popups
        ///</returns>
        /// <param name="tipoProcesso">Tipo de processo</param>
        /// <param name="tipoLancamento">Tipo de Lançamento</param>

        [HttpGet("RecuperarInformacoesComboboxPopup")]
        public async Task<IResultadoApplication<CategoriaPagamentoPopupComboboxViewModel>> RecuperarInformacoesComboboxPopup(long tipoProcesso, long tipoLancamento)
        {
            var resultado = await appService.RecuperarInformacoesComboboxPopup(tipoProcesso, tipoLancamento);
            return resultado;
        }

        ///<summary>
        ///Criar ou Editar Categoria de Pagamento
        ///</summary>
        ///<returns>
        ///Retorna sucesso = true ou false
        ///</returns>
        /// <param name="categoriaPagamentoDTO"></param>v
        [HttpPost("SalvarCategoriaPagamento")]
        public async Task<IResultadoApplication> SalvarCategoriaPagamento(CategoriaPagamentoInclusaoEdicaoDTO categoriaPagamentoDTO)
       {
            IResultadoApplication resultado;
            if (categoriaPagamentoDTO.Codigo == 0)
                resultado = await appService.CadastrarCategoriaPagamento(categoriaPagamentoDTO);
            else
                resultado = await appService.AlterarCategoriaPagamento(categoriaPagamentoDTO);

            return resultado;
        }

        /// <summary>
        /// Retorna um csv com o resultado.
        /// </summary>
        /// <returns>byte[]</returns>

        [HttpPost("ExportarCategoriaPagamento")]
        public async Task<IResultadoApplication<byte[]>> ExportarCategoriaPagamento(CategoriaPagamentoFiltroDTO filtroDTO)
        {
            var result = await appService.ExportarCategoriasPagamento(filtroDTO);
            return result;
        }

        ///<summary>
        ///Recupera as informações de categorias de pagamento
        ///</summary>
        ///<returns>
        ///Retorna listas com as informações necessárias para as categorias de pagaemnto
        ///</returns>        
        [HttpGet("RecuperarInformacoesComboboxPopupMigracaoEstrategico")]
        public async Task<IResultadoApplication<CategoriaMIgracaoEstrategicoPopupComboboxViewModel>> RecuperarInformacoesComboboxPopupEstrategico()
        {
            var resultado = await appService.RecuperarInformacoesComboboxPopupEstrategico();
            return resultado;
        }

        ///<summary>
        ///Recupera as informações de categorias de pagamento
        ///</summary>
        ///<returns>
        ///Retorna listas com as informações necessárias para as categorias de pagaemnto
        ///</returns>        
        [HttpGet("RecuperarInformacoesComboboxPopupMigracaoConsumidor")]
        public async Task<IResultadoApplication<CategoriaMIgracaoConsumidorPopupComboboxViewModel>> RecuperarInformacoesComboboxPopupConsumidor()
        {
            var resultado = await appService.RecuperarInformacoesComboboxPopupConsumidor();
            return resultado;
        }

    }
}