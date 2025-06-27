using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.AgendaDeAudienciasDoCivelEstrategico {

    /// <summary>
    /// Controller de comarcas para o context de agenda de audiências
    /// </summary>
    /// <returns></returns>
    [Route("agenda-de-audiencias-do-civel-estrategico/comarcas")]
    [ApiController]
    [Authorize]
    public class ComarcasController : ApiControllerBase {

        /// <summary>
        /// Recuperar todos as comarcas
        /// </summary>
        /// <returns></returns>
        [HttpGet("estado/{estadoId}")]
        public IActionResult ObterPorEstado([FromServices] IComarcaRepository repository, [FromRoute] string estadoId) {
            return Result(repository.ObterPorEstado(estadoId));
        }
    }
}