using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Api controller Indice
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("manutencao/indices")]
    public class IndiceController : ApiControllerBase
    {
        /// <summary>
        /// Lista todas os índices
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
            [FromServices] IIndiceRepository repository,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            if (quantidade == 0)
            {
                return Result(repository.ObterTodos());
            }

            return Result(repository.ObterPaginado(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, IndicesSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa));
        }

        /// <summary>
        /// verifica se o índice está sendo utilizado em alguma cotação
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="codIndice"></param>
        /// <returns></returns>
        [HttpGet("utilizado-em-cotacao/{codIndice}")]
        public IActionResult UtilizadoEmCotacao(
            [FromServices] IIndiceRepository repository,
            [FromRoute] int codIndice)
        {
            return Result(repository.UtilizadoEmCotacao(codIndice));
        }

        /// <summary>
        /// verifica se o índice está sendo utilizado em alguma cotação
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="codIndice"></param>
        /// <returns></returns>
        [HttpGet("obtertodos")]
        public IActionResult ObterTodos(
            [FromServices] IIndiceRepository repository)
        {
            return Result(repository.ObterTodos());
        }

        /// <summary>
        /// Cria um novo registro de índice
        /// </summary>

        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] IIndiceService service,
            [FromBody] CriarIndiceCommand dados)
        {
            return Result(service.Criar(dados));
        }

        /// <summary>
        /// Atualiza registro de índice
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IIndiceService service,
            [FromBody] AtualizarIndiceCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exclui registro de índice
        /// </summary>
        /// <param name="service"></param>
        /// <param name="codigoIndice"></param>
        /// <returns></returns>
        [HttpDelete("{codigoIndice}")]
        public IActionResult Remover(
            [FromServices] IIndiceService service,
            [FromRoute] int codigoIndice)
        {
            return Result(service.Remover(codigoIndice));
        }

        /// <summary>
        /// Exporta os índices
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pesquisa"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IIndiceRepository repository,
            [FromQuery] string coluna = "descricao", [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, IndicesSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Código;Nome;Mensal;Acumulado; Acumulado Automaticamente; Tipo de Valor");

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.Id}\";");
                    csv.Append($"\"{(a.Descricao)}\";");
                    csv.Append($"\"{(!string.IsNullOrEmpty(a.CodigoTipoIndice) ? (a.CodigoTipoIndice == "M" ? "Sim" : "Não" ) : string.Empty)  }\";");
                    csv.Append($"\"{(a.Acumulado.GetValueOrDefault() ? "Sim" : "Não")}\";");
                    csv.Append($"\"{(a.AcumuladoAutomatico.GetValueOrDefault() ? "Sim" : "Não")}\";");
                    csv.Append($"\"{(!string.IsNullOrEmpty(a.CodigoValorIndice) ? a.CodigoValorIndice == "F" ? "Fator" : a.CodigoValorIndice == "P" ? "Percentual" : "Valor" : string.Empty)  }\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"Indice_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}
