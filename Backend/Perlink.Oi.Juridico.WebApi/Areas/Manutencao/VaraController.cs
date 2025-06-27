using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.WebApi.Areas;
using System;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Comarca.Manutencao
{
    /// <summary>
    /// Api controller Indice
    /// </summary>
    ///
    [AllowAnonymous]
    [Authorize]
    [ApiController]
    [Route("manutencao/vara")]
    public class VaraController : ApiControllerBase
    {
        /// <summary>
        /// Lista todas as Varas
        /// </summary>
        /// <param name="comarcaId"></param>
        /// <param name="repository"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="pesquisa"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IVaraRepository repository,
            [FromQuery] int comarcaId,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            return Result(repository.ObterPaginado(comarcaId, pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, VaraSort.Id), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa));
        }

        /// <summary>
        ///  Cria um novo registro de Vara
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult Criar([FromServices] IVaraService service,
           [FromBody] CriarVaraCommand dados)
        {
            return Result(service.Criar(dados));
        }
        /// <summary>
        /// Atualiza registro de Vara
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IVaraService service,
           [FromBody] AtualizarVaraCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exclui registro de comarca
        /// </summary>
        /// <param name="service"></param>
        /// <param name="VaraId"></param>
        /// <param name="ComarcaId"></param>
        /// <param name="TipoVaraId"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult Remover(
          [FromServices] IVaraService service,
          [FromQuery] int VaraId,
          [FromQuery] int ComarcaId,
          [FromQuery] int TipoVaraId)
        {
            return Result(service.Remover(VaraId,ComarcaId,TipoVaraId));
        }

    }
}
