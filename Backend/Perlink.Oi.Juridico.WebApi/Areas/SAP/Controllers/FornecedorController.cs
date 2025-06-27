using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Compartilhado.Controllers
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class FornecedorController : JuridicoControllerBase
    {
        private readonly IFornecedorAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="AppService">Interface com os metodos que podem ser utilizados pela api.</param>
        public FornecedorController(IFornecedorAppService AppService)
        {
            this.appService = AppService;
        }

        /// <summary>
        /// Recuperar Todos os Fornecedores de acordo com o filtro informado
        /// </summary>
        /// <returns>Lista paginada de FornecedorResultadoViewModel</returns>
        /// <param name="fornecedorFiltroDTO"></param>       
        [HttpPost("RecuperarFornecedorPorFiltro")]
        public async Task<IPagingResultadoApplication<ICollection<FornecedorResultadoViewModel>>> RecuperarFornecedorPorFiltro([FromBody] FornecedorFiltroDTO fornecedorFiltroDTO)
        {
            var resultado = await appService.RecuperarFornecedorPorFiltro(fornecedorFiltroDTO);
            if (resultado.Sucesso == true)
                if (fornecedorFiltroDTO.Total <= 0) {                    
                    resultado.Total = await appService.ObterQuantidadeTotalPorFiltro(fornecedorFiltroDTO);
                } else {
                    if (Math.Floor(Convert.ToDecimal(fornecedorFiltroDTO.Total / fornecedorFiltroDTO.Quantidade)) == fornecedorFiltroDTO.Pagina) {
                        resultado.Total = await appService.ObterQuantidadeTotalPorFiltro(fornecedorFiltroDTO);
                    } else
                        resultado.Total = fornecedorFiltroDTO.Total;
                }
            return resultado;
        }

        /// <summary>
        /// Retorna o csv dos Fornecedores de acordo com o filtro selecionado.
        /// </summary>
        /// <returns>Lista de byte[]</returns>
        /// <param name="fornecedorFiltroDTO"></param>   
        [HttpPost("ExportarFornecedores")]
        public async Task<IResultadoApplication<byte[]>> ExportarFornecedores(FornecedorFiltroDTO fornecedorFiltroDTO)
        {
            var result = await appService.ExportarFornecedores(fornecedorFiltroDTO);
            return result;
        }

        ///<summary>
        ///Criar o fornecedor
        ///</summary>
        ///<returns>
        ///Retorna se o vinculo foi criado com sucesso
        ///</returns>
        /// <param name="fornecedorCriacaoViewModel"></param>
        [HttpPost("CadastrarFornecedor")]
        public async Task<IResultadoApplication> CadastrarFornecedor([FromBody]FornecedorCriacaoViewModel fornecedorCriacaoViewModel)
        {
            var resultado = await appService.CadastrarFornecedor(fornecedorCriacaoViewModel);
            return resultado;
        }



        /// <summary>
        /// Atualiza um fornecedor cadastrado no sistema. 
        /// </summary>
        /// <param name="fornecedorAtualizar"></param>
        /// <returns></returns>
        [HttpPost("AtualizarFornecedor")]
        public async Task<IResultadoApplication> AtualizarFornecedor(FornecedorAtualizarViewModel fornecedorAtualizar)
        {
            var resultado = await appService.AtualizarFornecedor(fornecedorAtualizar);
            return resultado;
        }

        /// <summary>
        /// Exclui um fornecedor com base no ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExcluirFornecedor")]
        public async Task<IResultadoApplication> ExcluirFornecedor(long codigoFornecedor) {
            var result = await appService.ExcluirFornecedor(codigoFornecedor);
            return result;
        }
    }
}
