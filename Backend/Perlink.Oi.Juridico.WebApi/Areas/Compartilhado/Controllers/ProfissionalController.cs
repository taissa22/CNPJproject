using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.CivelEstrategico.WebApi.Areas.Compartilhado.Controllers
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProfissionalController : JuridicoControllerBase
    {
        private readonly IProfissionalAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public ProfissionalController(IProfissionalAppService appService)
        {
            this.appService = appService;
        }

        ///<summary>
        /// Recuperar todos os escritórios pelo campo ind_escritorio = 'N' e ind_advogado = 'N'
        /// Ordena pelo nome
        /// </summary>
        /// <returns>Lista de ProfissionalDropDownViewModel</returns>
        [HttpGet("obterProfissionaisDropDown")]
        public async Task<IResultadoApplication<ICollection<ProfissionalDropDownViewModel>>> ObterProfissionaisDropDown()
        {
            var resultado = await appService.RecuperarTodosProfissionais();
            return resultado;
        }

        ///<summary>
        /// Recuperar todos os escritórios pelo campo ind_escritorio = 'S'
        /// Ordena pelo nome
        /// </summary>
        /// <returns>Lista de ProfissionalDropDownViewModel</returns>
        [HttpGet("obterEscritoriosDropDown")]
        public async Task<IResultadoApplication<ICollection<ProfissionalDropDownViewModel>>> ObterEscritoriosDropDown() {
            var resultado = await appService.RecuperarTodosEscritorios();
            return resultado;
        }
    }
}