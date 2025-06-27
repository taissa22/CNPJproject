using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.AgendaDeAudienciasDoCivelEstrategico {

    /// <summary>
    /// Controller de advogados do escritório para o context de agenda de audiências
    /// </summary>
    /// <returns></returns>
    [Route("agenda-de-audiencias-do-civel-estrategico/advogados-do-escritorio")]
    [ApiController]
    [Authorize]
    public class AdvogadosDoEscritorioController : ApiControllerBase {

        /// <summary>
        /// Recuperar lista de advogados
        /// </summary>
        /// <returns></returns>
        [HttpGet("{escritorioId}")]
        public IActionResult ObterPorEscritorio([FromServices] IAdvogadoDoEscritorioRepository repository, [FromRoute] int escritorioId) {
            return Result(repository.ObterPorEscritorio(escritorioId));
        }
    }
}