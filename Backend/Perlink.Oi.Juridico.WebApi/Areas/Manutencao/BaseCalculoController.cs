using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Handlers;
using Perlink.Oi.Juridico.Application.Manutencao.Inputs.BaseCalculos;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Result.BaseCalculos;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Controller da Manutencao Bases de Cálculo
    /// </summary>
    [Authorize]
    [Route("manutencao/bases-de-calculo")]
    [ApiController]
    public class BaseCalculoController : ApiControllerBase
    {
        /// <summary>
        /// Lista todas os tipos de pendências
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="pesquisa"></param>

        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IBaseDeCalculoRepository repository,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "codigo",
            [FromQuery] string direcao = "desc",
            [FromQuery] string pesquisa = null)
        {
            return Result(repository.ObterPaginado(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, BaseDeCalculoSort.Codigo), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa));
        }

        /// <summary>
        /// Cria um novo registro de tipo de pendência
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] IBaseDeCalculoService service,
            [FromBody] CriarBaseDeCalculoCommand dados)
        {
            return Result(service.Criar(dados));
        }

        /// <summary>
        /// Atualiza registro de tipo de pendência
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IBaseDeCalculoService service,
            [FromBody] AtualizarBaseDeCalculoCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exclui registro de tipo de pendência
        /// </summary>
        /// <param name="service"></param>
        /// <param name="codigoBaseDeCalculo"></param>
        /// <returns></returns>
        [HttpDelete("{codigoBaseDeCalculo}")]
        public IActionResult Remover(
            [FromServices] IBaseDeCalculoService service,
            [FromRoute] int codigoBaseDeCalculo)
        {
            return Result(service.Remover(codigoBaseDeCalculo));
        }

        /// <summary>
        /// Exporta os tipos de pendência
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pesquisa"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IBaseDeCalculoRepository repository,
            [FromQuery] string coluna = "codigo", [FromQuery] string direcao = "desc",
            [FromQuery] string pesquisa = null)
        {
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, BaseDeCalculoSort.Codigo),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {                
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Código;Descrição da Base de Cálculo;Cálculo Inicial");

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.Codigo}\";");
                    csv.Append($"\"{(a.Descricao)}\";");
                    csv.Append($"\"{(a.IndBaseInicial ? "Sim" : "Não") }\"");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"BaseCalculo_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}
