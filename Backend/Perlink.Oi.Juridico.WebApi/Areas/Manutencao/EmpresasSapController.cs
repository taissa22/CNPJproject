using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Api controller EmpresasSapController
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/empresas-sap")]
    public class EmpresasSapController : ApiControllerBase
    {
        /// <summary>
        /// Obtem lista de Empresas Sap
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Obter([FromServices] IEmpresaSapRepository repository)
        {
            return Result(repository.Obter());
        }
    }
}