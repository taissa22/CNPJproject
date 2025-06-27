using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Controller que resolve as requisições de naturezas de ações do banco do brasil
    /// </summary>
    [Route("manutencao/naturezas")]
    [ApiController]
    [Authorize]
    public class NaturezaAcaoBBController : ApiControllerBase
    {
        /// <summary>
        /// Lista todas as ações do cível consumidor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult obter
            ([FromServices] INaturezaAcaoBBRepository repository,
            [FromQuery] int? id)
        {
            return Result(repository.ObterNaturezasAcoesBB(id));           

        }
    }
}