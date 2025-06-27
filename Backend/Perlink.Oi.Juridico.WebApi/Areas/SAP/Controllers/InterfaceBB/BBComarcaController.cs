using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.VariosContextos;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Compartilhado.Controllers.InterfaceBB
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BBComarcaController : JuridicoControllerBase
    {
        private readonly IBBComarcaAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="AppService">Interface com os metodos que podem ser utilizados pela api.</param>


        public BBComarcaController(IBBComarcaAppService AppService)
        {
            this.appService = AppService;
        }

        /// <summary>
        /// Retorna um csv com o resultado.
        /// </summary>
        /// <returns>byte[]</returns>

        [HttpPost("ExportarBBComarca")]
        public async Task<IResultadoApplication<byte[]>> ExportarBBComarca(FiltrosDTO filtroDTO)
        {
            var result = await appService.ExportarBBComarca(filtroDTO);
            return result;
        }

        /// <summary>
        /// Recuperar Todas as BB Comarcas
        /// </summary>
        /// <returns>Lista de BB Comarca</returns>
        /// <param name="filtroDTO"></param>

        [HttpPost("ConsultarBBComarca")]
        public Task<IPagingResultadoApplication<ICollection<BBComarcaViewModel>>> ConsultarBBComarca([FromBody] FiltrosDTO filtroDTO)
        {
            var resultado = appService.ConsultarBBComarca(filtroDTO);

            return resultado;
        }

        /// <summary>
        /// Recuperar Todas as BB Comarcas
        /// </summary>
        /// <returns>Lista de BB Comarca</returns>
        /// <param name="filtroDTO"></param>

        [HttpGet("ConsultarComarcaPorEstado")]
        public Task<IResultadoApplication<ICollection<ComboboxViewModel<int>>>> ConsultarComarcaPorEstado(string codigoEstado)
        {
            var resultado = appService.ConsultarComarcaPorEstado(codigoEstado);

            return resultado;
        }
        ///<summary>
        ///Cadastra ou Atualiza BB Comarca 
        ///</summary>
        ///<returns>
        ///Retorna o Id caso o cadastro ocorra com sucesso
        ///</returns>
        /// <param name="viewModel"></param>
        [HttpPost("SalvarBBComarca")]
        public async Task<IResultadoApplication> SalvarBBComarca([FromBody]BBComarcaViewModel viewModel)
        {
            IResultadoApplication resultado;
            if (viewModel.Id == 0)
                resultado = await appService.CadastrarBBComarca(viewModel);
            else
                resultado = await appService.AlterarBBComarca(viewModel);

            return resultado;
        }

        /// <summary>
        /// Exclui uma BBComarca com base no ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExcluirBBComarca")]
        public async Task<IResultadoApplication> ExcluirBBComarca(long codigo)
        {
            var result = await appService.ExcluirBBComarca(codigo);
            return result;
        }

    }
}
