using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;


namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Api controller FornecedoresController
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/fornecedores")]
    public class FornecedoresController : ApiControllerBase
    {
        /// <summary>
        /// Obtem lista de forncedores
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Obter([FromServices] IFornecedorRepository repository)
        {
            return Result(repository.Obter());
        }
    }
}