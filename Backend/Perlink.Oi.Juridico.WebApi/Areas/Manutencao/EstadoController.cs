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

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Api controller EstadoController
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/estado")]
    public class EstadoController : ApiControllerBase
    {

        /// <summary>
        /// Lista  Estados
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="estadoId"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IEstadoRepository repository,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string estadoId,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc")
        {
            return Result(repository.ObterPaginado(pagina, quantidade, estadoId, EnumHelpers.ParseOrDefault(coluna, EstadoSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc")));
        }

        /// <summary>
        /// Lista todos os Estados
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos(
            [FromServices] IEstadoRepository repository)
        {
            return Result(repository.ObterTodos());
        }

        /// <summary>
        /// Atualiza registro de Estado
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IEstadoService service,
            [FromBody] AtualizarEstadoCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exporta os Estados
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="estadoId"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IEstadoRepository repository,
            [FromQuery] string estadoId,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc")
        {
            {
                var resultado = repository.Obter(estadoId, EnumHelpers.ParseOrDefault(coluna, EstadoSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"));

                if (resultado.Tipo == ResultType.Valid)
                {
                    StringBuilder csv = new StringBuilder();
                    csv.AppendLine($"Sigla;Nome;Taxa de Juros Mês;Código do Município;Nome do Município");

                    foreach (var a in resultado.Dados)
                    {
                        if (a.Municipios.Any())
                        {
                            foreach (var x in a.Municipios.OrderBy(x => x.Nome))
                            {
                                csv.Append($"\"{a.Id}\";");
                                csv.Append($"\"{(a.Nome)}\";");
                                csv.Append($"\"{ a.ValorJuros.ToString("000.00000")}\";");
                                csv.Append($"\"{x.Id}\";");
                                csv.Append($"\"{x.Nome}\";");
                                csv.AppendLine("");
                            }
                        }
                        else
                        {
                            csv.Append($"\"{a.Id}\";");
                            csv.Append($"\"{(a.Nome)}\";");
                            csv.Append($"\"{(string.Format(a.ValorJuros.ToString(), "000,00000"))}\";");
                            csv.AppendLine("");
                        }

                    }

                    string nomeArquivo = $"EstadoMunicipio_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                    byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                    bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                    return File(bytes, "text/csv", nomeArquivo);
                }

                return Result(resultado);
            }

        }

        /// <summary>
        /// Exclui registro de estado
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IEstadoService service,
            [FromRoute] string id)
        {
            return Result(service.Remover(id));
        }
    }
}
