using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.AgendaDeAudienciasDoCivelEstrategico {

    /// <summary>
    /// Controller de prepostos para o context de agenda de audiências
    /// </summary>
    /// <returns></returns>
    [Route("agenda-de-audiencias-do-civel-estrategico/prepostos")]
    [ApiController]
    [Authorize]
    public class PrepostosController : ApiControllerBase {

        /// <summary>
        /// Recuperar lista de prepostos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Obter([FromServices] IPrepostoRepository repository) {
            return Result(repository.Obter());
        }
    }
}