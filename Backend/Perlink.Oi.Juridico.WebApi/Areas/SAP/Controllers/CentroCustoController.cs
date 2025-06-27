using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCentroCusto;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCentroCusto;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
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
    public class CentroCustoController : JuridicoControllerBase
    {
        private readonly ICentroCustoAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="AppService">Interface com os metodos que podem ser utilizados pela api.</param>
        public CentroCustoController(ICentroCustoAppService AppService)
        {
            this.appService = AppService;
        }

        /// <summary>
        /// Recuperar Todos os CentroCustoes de acordo com o filtro informado
        /// </summary>
        /// <returns>Lista paginada de CentroCustoResultadoViewModel</returns>
        /// <param name="CentroCustoFiltroDTO"></param>
        [HttpPost("ConsultarCentroCusto")]
        public async Task<IPagingResultadoApplication<ICollection<CentroCustoViewModel>>> ConsultarCentrosCustos([FromBody] CentroCustoFiltroDTO CentroCustoFiltroDTO)
        {
            var resultado = await appService.ConsultarCentrosCustos(CentroCustoFiltroDTO);

            return resultado;
        }

        // <summary>
        // Retorna o csv dos CentroCustos de acordo com o filtro selecionado.
        //</summary>
        // <returns>Lista de byte[]</returns>
        /// <param name="CentroCustoFiltroDTO"></param>
        [HttpPost("ExportarCentroCusto")]
        public async Task<IResultadoApplication<byte[]>> ExportarCentroCusto(CentroCustoFiltroDTO CentroCustoFiltroDTO)
        {
            var result = await appService.ExportarCentroCusto(CentroCustoFiltroDTO);
            return result;
        }

        /// <summary>
        /// Cadastra/Editar centro de custo
        /// </summary>
        /// <returns>realizado com sucesso</returns>
        /// <param name="centroCustoViewModel"></param>
        [HttpPost("SalvarCentroCusto")]
        public async Task<IResultadoApplication> SalvarCentroCusto([FromBody] CentroCustoViewModel centroCustoViewModel)
        {
            var resultado = await appService.CriarAlterarCentroCusto(centroCustoViewModel);
            return resultado;
        }

        /// <summary>
        /// Exclui centro de custo com base no ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExcluirCentroCusto")]
        public async Task<IResultadoApplication> ExcluirCentroCusto(long codigo)
        {
            var result = await appService.ExcluirCentroCusto(codigo);
            return result;
        }
    }
}