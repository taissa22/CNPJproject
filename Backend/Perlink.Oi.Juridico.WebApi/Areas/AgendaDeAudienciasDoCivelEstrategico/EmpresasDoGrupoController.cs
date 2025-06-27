using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.AgendaDeAudienciasDoCivelEstrategico {

    /// <summary>
    /// Controller de empresas do grupo para o context de agenda de audiências
    /// </summary>
    /// <returns></returns>
    [Route("agenda-de-audiencias-do-civel-estrategico/empresas-do-grupo")]
    [ApiController]
    [Authorize]
    public class EmpresasDoGrupoController : ApiControllerBase {

        /// <summary>
        /// Recuperar todas as Empresas do Grupo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Obter([FromServices] IEmpresaDoGrupoRepository repository) {
            return Result(repository.Obter());
        }

        /// <summary>
        /// Obter paginado para dropdown
        /// </summary>
        [HttpGet("dropdown")]
        public IActionResult ObterParaDropdown([FromServices] IEmpresaDoGrupoRepository repository,  [FromQuery] int pagina = 1, [FromQuery] int quantidade = 50, [FromQuery] int empresaDoGrupoId = 0) {
            return Result(repository.ObterParaDropdown(pagina, quantidade, empresaDoGrupoId));
        }
    }
}