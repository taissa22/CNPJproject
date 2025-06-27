using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.AgendaAudiencia;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.SAP.Controllers
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ComarcaController : JuridicoControllerBase
    {
        private readonly IComarcaAppService appService;
        public ComarcaController(IComarcaAppService appService)
        {
            this.appService = appService;
        }
        /// <summary>
        /// Traz uma lista de Comarca filtrada por Estado
        /// </summary>     
        /// <returns></returns>
        [HttpGet("RecuperarComarcaPorEstado")]
        public async Task<IResultadoApplication<ICollection<ComarcaComboViewModel>>> RecuperarComarcaPorEstado(string estado)
        {
            var resultado = await appService.RecuperarComarcaPorEstado(estado);
            return resultado;
        }

    }
}
