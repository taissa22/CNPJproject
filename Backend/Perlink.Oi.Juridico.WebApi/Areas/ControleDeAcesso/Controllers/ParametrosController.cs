using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel;
using Perlink.Oi.Juridico.WebApi.Helpers;

namespace Perlink.Oi.Juridico.CivelEstrategico.WebApi.Areas.CivelEstrategico.Controllers
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ParametrosController : JuridicoControllerBase
    {
        private readonly IParametroAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public ParametrosController(IParametroAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Recuperar Todos os parametros
        /// </summary>
        /// <returns>Lista de parametros</returns>
        [HttpGet("{id}")]
        public ParametroViewModel GetById(string id)
        {
            var resultado = appService.RecuperarPorNome(id).Data;
            return resultado;

        }
    }
}