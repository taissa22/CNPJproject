using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.DocumentoCredor {

    /// <summary>
    /// Controller que resolve as requisições de naturezas de ações do banco do brasil
    /// </summary>
    [Route("documento-credor/parametro")]
    [ApiController]
    [AllowAnonymous]
    public class ParametroJuridicoController : ApiControllerBase {
        /// <summary>
        /// Recuperar um parametro por id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{parametroId}")]
        public IActionResult ObterParametroPorId([FromServices] IParametroJuridicoRepository parametroRepository, [FromRoute] string parametroId) {
            return Result(parametroRepository.Obter(parametroId));
        }
    }
}
