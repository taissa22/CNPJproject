using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Api controller RegionaisController
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/regionais")]
    public class RegionaisController : ApiControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Obter([FromServices] IRegionaisRepository repository)
        {
            return Result(repository.Obter());
        }
    }
}