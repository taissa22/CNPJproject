using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Api controller InterfacesBBController
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/interfaces-bb")]
    public class InterfacesBBController : ApiControllerBase
    {
        /// <summary>
        /// Obtem lista de Interfaces BB
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Obter([FromServices] IInterfaceBBRepository repository)
        {
            return Result(repository.Obter());
        }
    }
}