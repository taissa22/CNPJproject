using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
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
    public class BBModalidadeController : JuridicoControllerBase
    {
        private readonly IBBModalidadeAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="AppService">Interface com os metodos que podem ser utilizados pela api.</param>


        public BBModalidadeController(IBBModalidadeAppService AppService)
        {
            this.appService = AppService;
        }

        /// <summary>
        /// Retorna um csv com o resultado.
        /// </summary>
        /// <returns>byte[]</returns>

        [HttpPost("ExportarBBModalidade")]
        public async Task<IResultadoApplication<byte[]>> ExportarBBModalidade([FromBody] BBModalidadeFiltroDTO filtroDTO)
        {
            var result = await appService.ExportarBBModalidade(filtroDTO);
            return result;
        }

        /// <summary>
        /// Recuperar Todas as BBModalidade
        /// </summary>
        /// <returns>Lista de BBModalidade</returns>

        [HttpPost("ConsultarBBModalidade")]
        public async Task<IPagingResultadoApplication<ICollection<BBModalidadeViewModel>>> ConsultarBBModalidade(BBModalidadeFiltroDTO filtroDTO)
        {
            var resultado = await appService.ConsultarBBModalidade(filtroDTO);

            return resultado;
        }

        ///<summary>
        ///Cadastra ou Atualiza BBModalidade
        ///</summary>
        /// <param name="bbModalidade"></param>
        [HttpPost("SalvarBBModalidade")]
        public async Task<IResultadoApplication> SalvarBBModalidade([FromBody]BBModalidadeViewModel bbModalidade)
        {
            IResultadoApplication resultado;
            if (bbModalidade.Id == 0)
                resultado = await appService.CadastrarBBModalidade(bbModalidade);
            else
                resultado = await appService.AlterarBBModalidade(bbModalidade);

            return resultado;
        }

        /// <summary>
        /// Exclui uma BBModalidade com base no ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExcluirBBModalidade")]
        public async Task<IResultadoApplication> ExcluirBBModalidade(long codigo)
        {
            var result = await appService.ExcluirBBModalidade(codigo);
            return result;
        }

    }
}
