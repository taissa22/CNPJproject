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
    [Authorize]
    [ApiController]
    [Route("manutencao/motivo-provavel-zero")]
    public class MotivoProvavelZeroController : ApiControllerBase
    {
        /// <summary>
        /// Lista todas as Comarcas
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
            [FromServices] IMotivoProvavelZeroRepository repository,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "descricao",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            return Result(repository.ObterPaginado(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, MotivoProvavelZeroSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa));
        }

        /// <summary>
        /// Cria um novo registro de Comarca
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IMotivoProvavelZeroService service,
            [FromBody] CriarMotivoProvavelZeroCommand dados)
        {
            return Result(service.Criar(dados));
        }

        /// <summary>
        /// Atualiza registro de MotivoProvavelZero
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IMotivoProvavelZeroService service,
            [FromBody] AtualizarMotivoProvavelZeroCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exclui registro de MotivoProvavelZero
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IMotivoProvavelZeroService service,
            [FromRoute] int id)
        {
            return Result(service.Remover(id));
        }

        /// <summary>
        /// Exporta os Tipos de vara
        /// </summary>
        /// <param name="repository"></param>
        /// /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="pesquisa"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IMotivoProvavelZeroRepository repository,
            [FromQuery] string coluna = "descricao",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)

        {
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, MotivoProvavelZeroSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Código;Descrição Motivo Provavel Zero");

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.Id}\";");
                    csv.Append($"\"{(a.Descricao)}\";");
                    csv.AppendLine();
                }

                string nomeArquivo = $"Motivo_Provavel_Zero_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}