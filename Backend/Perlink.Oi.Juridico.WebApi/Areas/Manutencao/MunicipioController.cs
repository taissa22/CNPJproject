using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Api controller MunicipioController
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/municipio")]
    public class MunicipioController : ApiControllerBase
    {

        /// <summary>
        /// Lista os Municipios
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="estadoId"></param>
        /// <param name="municipioId"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>

        [HttpGet]
        public IActionResult ObterPaginado(
        [FromServices] IMunicipioRepository repository,
        [FromQuery] int pagina,
        [FromQuery] int quantidade,
        [FromQuery] string estadoId,
        [FromQuery] int? municipioId = null,
        [FromQuery] string coluna = "sigla",
        [FromQuery] string direcao = "asc")
        {
            return Result(repository.ObterPaginado(pagina, quantidade, estadoId, municipioId, EnumHelpers.ParseOrDefault(coluna, MunicipioSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc")));
        }

        /// <summary>
        /// Lista todos os Municipios
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="estadoId"></param>
        /// <returns></returns>
        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos(
        [FromServices] IMunicipioRepository repository,
        [FromQuery] string estadoId)
        {
            return Result(repository.ObterTodos(estadoId));
        }

        /// <summary>
        /// Atualiza registro de Municipio
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IMunicipioService service,
            [FromBody] AtualizarMunicipioCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Adiciona registro de Municipio
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] IMunicipioService service,
            [FromBody] CriarMunicipioCommand dados)
        {
            return Result(service.Criar(dados));
        }

        /// <summary>
        /// Exclui registro de Municipio
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IMunicipioService service,
            [FromRoute] int id)
        {
            return Result(service.Remover(id));
        }
    }
}
