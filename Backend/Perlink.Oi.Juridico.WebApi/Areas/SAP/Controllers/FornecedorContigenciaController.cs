using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutençãoFornecedorContigencia;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFornecedores;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.SAP.Controllers
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class FornecedorContigenciaController : JuridicoControllerBase
    {
        private readonly IFornecedorContigenciaAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="AppService">Interface com os metodos que podem ser utilizados pela api.</param>
        public FornecedorContigenciaController(IFornecedorContigenciaAppService AppService)
        {
            this.appService = AppService;
        }

        ///<sumary>
        ///Retorna lista de FornecedorContigencia paginado para grid
        ///</sumary>
        ///<returns>FornecedorContigenciaResultadoViewModel</returns>
        [HttpPost("ConsultarFornecedorContigencia")]
        public async Task<IPagingResultadoApplication<ICollection<FornecedorContigenciaResultadoViewModel>>> ConsultarFornecedorContigencia([FromBody] FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO)
        {
            var result =  await appService.ConsultarFornecedorContigencia(fornecedorContigenciaConsultaDTO);
            return result;
        }

        /// <summary>
        /// Atualiza um fornecedor Contigencia cadastrado no sistema. 
        /// </summary>
        /// <param name="fornecedorAtualizar"></param>
        /// <returns></returns>
        [HttpPost("SalvarFornecedorContigencia")]
        public async Task<IResultadoApplication> AtualizarFornecedorContigencia(FornecedorContigenciaAtualizaViewModel fornecedorAtualizar)
        {
            var resultado = await appService.AtualizarFornecedorContigencia(fornecedorAtualizar);
            return resultado;
        }

        ///<sumary>
        ///Cria um arquivo CSV de acordo com os dados informados.
        ///</sumary>
        ///<returns>Retorna arquivo csv de fornecedor contigencia</returns>
        [HttpPost("ExportarFornecedorContigencia")]
        public Task<IResultadoApplication<byte[]>> ExportarFornecedorContingencia([FromBody] FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO)
        {
            var result = appService.ExportarFornecedorContingencia(fornecedorContigenciaConsultaDTO);
            return result;
        }
    }
}