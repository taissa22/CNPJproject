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
    /// Api controller Indice
    /// </summary>
    ///
    [AllowAnonymous]
    [Authorize]
    [ApiController]
    [Route("manutencao/tipos-de-vara")]
    public class TipoVaraController : ApiControllerBase
    {
        /// <summary>
        /// Lista todas os Tipos de vara
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
            [FromServices] ITipoVaraRepository repository,
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

            return Result(repository.ObterPaginado(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, TipoVaraSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa));
        }

        /// <summary>
        /// verifica se o tipo de vara está sendo utilizado em alguma cotação
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="codTipoVara"></param>
        /// <param name="codTipoProcesso"></param>
        /// <returns></returns>
        [HttpGet("utilizado-em-processo/{codTipoVara}/{codTipoProcesso}")]
        public IActionResult UtilizadoEmCotacao(
            [FromServices] ITipoVaraRepository repository,
            [FromRoute] int codTipoVara,
            [FromRoute] int codTipoProcesso)
        {
            return Result(repository.UtilizadoEmProcesso(codTipoVara, codTipoProcesso));
        }

        /// <summary>
        /// Cria um novo registro de tipos de vara
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] ITipoVaraService service,
            [FromBody] CriarTipoVaraCommand dados)
        {
            return Result(service.Criar(dados));
        }

        /// <summary>
        /// Atualiza registro de tipos de vara
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] ITipoVaraService service,
            [FromBody] AtualizarTipoVaraCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exclui registro de tipos de vara
        /// </summary>
        /// <param name="service"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpDelete("{codigo}")]
        public IActionResult Remover(
            [FromServices] ITipoVaraService service,
            [FromRoute] int codigo)
        {
            return Result(service.Remover(codigo));
        }

        /// <summary>
        /// Exporta os Tipos de vara
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pesquisa"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] ITipoVaraRepository repository,
            [FromQuery] string coluna = "descricao",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, TipoVaraSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Código;Nome;Cível Consumidor;Cível Estratégico;Trabalhista;Tributária;Juizado;Criminal Judicial;Procon;");

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.Id}\";");
                    csv.Append($"\"{(a.Nome)}\";");
                    csv.Append($"\"{(a.Eh_CivelConsumidor ? "Sim" : "Não")}\";");
                    csv.Append($"\"{(a.Eh_CivelEstrategico ? "Sim" : "Não")}\";");
                    csv.Append($"\"{(a.Eh_Trabalhista ? "Sim" : "Não")}\";");
                    csv.Append($"\"{(a.Eh_Tributaria ? "Sim" : "Não")}\";");
                    csv.Append($"\"{(a.Eh_Juizado ? "Sim" : "Não")}\";");
                    csv.Append($"\"{(a.Eh_CriminalJudicial ? "Sim" : "Não")}\";");
                    csv.Append($"\"{(a.Eh_Procon ? "Sim" : "Não")}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"Tipo_Vara_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}