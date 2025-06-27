using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.AgendaDeAudienciasDoCivelEstrategico {

    /// <summary>
    /// Controller de escritóriso para o context de agenda de audiências
    /// </summary>
    /// <returns></returns>
    [Route("agenda-de-audiencias-do-civel-estrategico/escritorios")]
    [ApiController]
    [Authorize]
    public class EscritoriosController : ApiControllerBase {

        /// <summary>
        /// Obtém escritórios cíveis estratégicos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Obter([FromServices] IEscritorioRepository repository, [FromQuery] bool considerarApenasDoUsuarioLogado = false) {
            if (considerarApenasDoUsuarioLogado) {
                return Result(repository.ObterAutorizadosAoUsuarioLogado());
            }

            return Result(repository.Obter());
        }
    }
}