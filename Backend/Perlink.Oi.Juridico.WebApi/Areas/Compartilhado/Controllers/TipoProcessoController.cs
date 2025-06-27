using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
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
    public class TipoProcessoController : JuridicoControllerBase
    {
        private readonly ITipoProcessoAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public TipoProcessoController(ITipoProcessoAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Recuperar Todos os parametros de um CodigoTipoProcesso
        /// </summary>
        /// <returns>Lista de parametros</returns>
        [HttpGet("getByCodigoTipoProcesso")]
        public async Task<IResultadoApplication<TipoProcessoViewModel>> GetByCodigoTipoProcesso(long CodigoTipoProcesso)
        {
            var resultado = await appService.RecuperarPorId(CodigoTipoProcesso);
            return resultado;
        }

        /// <summary>
        /// Recuperar Todos os parametros
        /// </summary>
        /// <returns>Lista de parametros</returns>
        [HttpGet("obterTodosTiposProcesso")]
        public async Task<IResultadoApplication<ICollection<TipoProcessoViewModel>>> ObterTodosTiposProcesso()
        {
            var resultado = await appService.RecuperarTodos();
            return resultado;
        }

        ///<summary>
        /// Recuperar Tipos processos que são utilizados no SAP
        /// </summary>
        /// <returns>Lista de TipoProcessoViewModel</returns>
        [HttpGet("ObterTodosTipoProcessoSAP")]
        public async Task<IResultadoApplication<ICollection<TipoProcessoViewModel>>> ObterTodosTipoProcessoSAP(string tela) {
            var resultado = await appService.RecuperarTodosSAP(tela);
            return resultado;
        }
    }
}