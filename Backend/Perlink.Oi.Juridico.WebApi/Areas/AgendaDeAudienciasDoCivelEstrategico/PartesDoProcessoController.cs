using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.AgendaDeAudienciasDoCivelEstrategico {

    /// <summary>
    /// Controller de advogados do escritório para o context de agenda de audiências
    /// </summary>
    /// <returns></returns>
    [Route("agenda-de-audiencias-do-civel-estrategico/partes-do-processo")]
    [ApiController]
    [Authorize]
    public class PartesDoProcessoController : ApiControllerBase {

        /// <summary>
        /// Recuperar lista de partes do processo
        /// </summary>
        /// <returns></returns>
        [HttpGet("{processoId}")]
        public IActionResult ObterPorProcesso([FromServices] IParteDoProcessoRepository repository, [FromRoute] int processoId) {
            return Result(repository.ObterPorProcesso(processoId));
        }
    }
}