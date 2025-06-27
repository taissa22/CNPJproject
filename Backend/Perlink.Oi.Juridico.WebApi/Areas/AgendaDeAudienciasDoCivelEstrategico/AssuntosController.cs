using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.AgendaDeAudienciasDoCivelEstrategico
{
    /// <summary>
    /// Controller de assuntos
    /// </summary>
    /// <returns></returns>
    [Route("agenda-de-audiencias-do-civel-estrategico/assuntos")]
    [ApiController]
    [Authorize]
    public class AssuntosController : ApiControllerBase
    {
        /// <summary>
        /// Obter paginado para dropdown
        /// </summary>
        [HttpGet("dropdown")]
        public IActionResult ObterParaDropdown([FromServices] IAssuntoRepository repository, [FromQuery] int pagina = 1, [FromQuery] int quantidade = 50, [FromQuery] int assuntoId = 0)
        {
            return Result(repository.ObterParaDropdown(pagina, quantidade, assuntoId));
        }
    }
}