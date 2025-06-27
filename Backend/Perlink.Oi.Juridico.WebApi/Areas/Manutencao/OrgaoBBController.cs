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
    [Route("manutencao/orgao-bb")]
    public class OrgaoBBController : ApiControllerBase
    {
        /// <summary>
        /// Lista todas os OrgaoBB
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="TribunalBBId"></param>
        /// <param name="ComarcaBBId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPorTribunalEComarcaBB(
            [FromServices] IOrgaoBBRepository repository,
            int TribunalBBId,
            int ComarcaBBId)
        {
            return Result(repository.ObterPorTribunalEComarcaBB(TribunalBBId,ComarcaBBId));
        }
    }
}
