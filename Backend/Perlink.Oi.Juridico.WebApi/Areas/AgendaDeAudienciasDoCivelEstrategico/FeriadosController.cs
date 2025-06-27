using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.AgendaDeAudienciasDoCivelEstrategico {

    /// <summary>
    /// Controller de feriados para o context de agenda de audiências
    /// </summary>
    /// <returns></returns>
    [Route("agenda-de-audiencias-do-civel-estrategico/feriados")]
    [ApiController]
    [Authorize]
    public class FeriadosController : ApiControllerBase {

        /// <summary>
        /// Recuperar lista de feriados
        /// </summary>
        /// <returns></returns>
        [HttpGet("futuros")]
        public IActionResult ObterFuturos([FromServices] IFeriadoRepository repository) {
            return Result(repository.ObterFuturos());
        }
    }
}