using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Api controller CentrosDeCustoController
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/centros-de-custo")]
    public class CentrosDeCustoController : ApiControllerBase
    {
        /// <summary>
        /// Obtem lista de Centros de Custo
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Obter([FromServices] ICentroDeCustoRepository repository)
        {
            return Result(repository.Obter());
        }
    }
}
