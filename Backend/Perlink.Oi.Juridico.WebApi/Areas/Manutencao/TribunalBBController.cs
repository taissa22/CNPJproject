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
    [Route("manutencao/tribunal-bb")]
    public class TribunalBBController : ApiControllerBase
    {



        /// <summary>
        /// Lista todas os Tipos de vara
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginado(
          [FromServices] ITribunalBBRepository repository)
        {
              return Result(repository.ObterTodos());
        }

    }
}
