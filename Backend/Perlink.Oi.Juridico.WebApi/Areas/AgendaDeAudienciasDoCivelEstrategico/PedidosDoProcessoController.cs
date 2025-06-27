using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.AgendaDeAudienciasDoCivelEstrategico {

    /// <summary>
    /// Controller de pedidos para o context de agenda de audiências
    /// </summary>
    /// <returns></returns>
    [Route("agenda-de-audiencias-do-civel-estrategico/pedidos-do-processo")]
    [ApiController]
    [Authorize]
    public class PedidosDoProcessoController : ApiControllerBase {

        /// <summary>
        /// Recuperar lista de pedidos
        /// </summary>
        /// <returns></returns>
        [HttpGet("{processoId}")]
        public IActionResult Obter([FromServices] IPedidoDoProcessoRepository repository, [FromRoute] int processoId) {
            return Result(repository.ObterPorProcesso(processoId));
        }
    }
}