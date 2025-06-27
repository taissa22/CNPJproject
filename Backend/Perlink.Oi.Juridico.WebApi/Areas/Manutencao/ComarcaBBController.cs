using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{   
    
    /// <summary>
    /// Api controller Indice
    /// </summary>
    ///
    [AllowAnonymous]
    [Authorize]
    [ApiController]
    [Route("manutencao/comarcabb")]
    public class ComarcaBBController : ApiControllerBase
    {

        /// <summary>
        /// Lista todas as ComarcasBB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPorEstado(
            [FromServices] IComarcaBBRepository repository,
            string estadoId)
        {
            return Result(repository.ObterPorEstado(estadoId));
        }
    }
}
