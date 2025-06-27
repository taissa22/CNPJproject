using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.General;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
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
  
    public class BBNaturezasAcoesController : JuridicoControllerBase
    {
        private readonly IBBNaturezasAcoesAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="AppService">Interface com os metodos que podem ser utilizados pela api.</param>

        public BBNaturezasAcoesController(IBBNaturezasAcoesAppService AppService)
        {
            this.appService = AppService;
        }

        /// <summary>
        /// Retorna um csv com o resultado.
        /// </summary>
        /// <returns>byte[]</returns>

        [HttpPost("ExportarBBNaturezasAcoes")]
        public async Task<IResultadoApplication<byte[]>> ExportarBBNaturezasAcoes(DescriptionFilterViewModel filtroDTO)
        {
            var result = await appService.ExportarBBNaturezasAcoes(filtroDTO);

            return result;
        }

        /// <summary>
        /// Recuperar Todas as BB Naturezas Acoes
        /// </summary>
        /// <returns>Lista de BB Naturezas Acoes</returns>
        /// <param name="filtroDTO"></param>

        [HttpPost("ConsultarBBNaturezasAcoes")]
        public async Task<IPagingResultadoApplication<ICollection<BBNaturezasAcoesViewModel>>> ConsultarBBNaturezasAcoes([FromBody] DescriptionFilterViewModel filtroDTO)
        {
            var resultado = await appService.ConsultarBBNaturezasAcoes(filtroDTO);            

            return resultado;
        }

        ///<summary>
        ///Cadastra ou Atualiza BB Comarca
        ///</summary>
        ///<returns>
        ///Retorna o Id caso o cadastro ocorra com sucesso
        ///</returns>
        /// <param name="bbNaturezasAcoes"></param>
        [HttpPost("SalvarBBNaturezasAcoes")]
        public async Task<IResultadoApplication> SalvarBBNaturezasAcoes([FromBody]BBNaturezasAcoesViewModel bbNaturezasAcoes)
        {
            IResultadoApplication resultado;
            if (bbNaturezasAcoes.Id == 0)
                resultado = await appService.CadastrarBBNaturezasAcoes(bbNaturezasAcoes);
            else
                resultado = await appService.AlterarBBNaturezasAcoes(bbNaturezasAcoes);

            return resultado;
        }

        /// <summary>
        /// Exclui uma BBNaturezasAcoes com base no ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExcluirBBNaturezasAcoes")]
        public async Task<IResultadoApplication> ExcluirBBNaturezasAcoes(long codigo)
        {
            var result = await appService.ExcluirBBNaturezasAcoes(codigo);

            return result;
        }
    }
}