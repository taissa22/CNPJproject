using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.VariosContextos;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;

namespace Perlink.Oi.Juridico.WebApi.Areas.Compartilhado.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class VaraController : JuridicoControllerBase
    {
        private readonly IVaraAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public VaraController(IVaraAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Recuperar Todos os parametros
        /// </summary>
        /// <returns>Lista de parametros</returns>
        [HttpGet("recuperarVaraPorComarca")]
        public async Task<IResultadoApplication<IEnumerable<ComboboxViewModel<long>>>> RecuperarVaraPorComarca(long codigoComarca)
        {
            var resultado = await appService.RecuperarVaraPorComarca(codigoComarca);
            return resultado;
        }
    }
}