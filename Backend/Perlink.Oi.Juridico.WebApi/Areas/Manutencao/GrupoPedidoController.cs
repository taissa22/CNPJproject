using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Controller que resolve as requisições de ações do cível consumidor
    /// </summary>
    [Authorize]
    [Route("manutencao/Grupo-Pedido")]
    [ApiController]
    public class GrupoPedidoController : ApiControllerBase
    {

        /// <summary>
        /// Lista todos grupos
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="tipoProcesso"></param>
        /// <param name="pesquisa"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IGrupoPedidoRepository repository,
            [FromQuery] int tipoProcesso,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] int pagina = 1,
            [FromQuery] int quantidade = 8,
            [FromQuery] string pesquisa = null)
        {
            return Result(repository.ObterPaginado(TipoProcesso.PorId(tipoProcesso), EnumHelpers.ParseOrDefault(coluna, GrupoPedidoSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pagina, quantidade, pesquisa));
        }

        /// <summary>
        /// Lista todos grupos para combo
        /// </summary>
        /// <param name="repository"></param>   
        /// <param name="tipoProcesso"></param>
        /// <returns></returns>
        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos(
            [FromServices] IGrupoPedidoRepository repository,
            [FromQuery] int tipoProcesso)
        {
            return Result(repository.ObterTodos(TipoProcesso.PorId(tipoProcesso)));
        }

        /// <summary>
        /// Cria um novo registro de grupo
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] IGrupoPedidoService service,
            [FromBody] CriarGrupoPedidoCommand dados)
        {

            return Result(service.Criar(dados));
        }

        /// <summary>
        /// Atualiza Grupo Pedido
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IGrupoPedidoService service,
            [FromBody] AtualizarGrupoPedidoCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exclui registro de Grupo Pedido
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IGrupoPedidoService service, 
            [FromRoute] int id)
        {
            return Result(service.Remover(id));
        }      
    }
}
