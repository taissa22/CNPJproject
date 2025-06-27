using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao {

    /// <summary>
    /// Controller que resolve as requisições de ações do tributário judicial
    /// </summary>
  //  [Authorize]
    [Route("manutencao/acoes/tributario-judicial")]
    [ApiController]
    public class AcaoTributarioJudicialController : ApiControllerBase {

        /// <summary>
        /// Lista todas as ações do tributário judicial
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
            [FromServices] IAcaoRepository repository, 
            [FromQuery] int pagina, 
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "nome", 
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null) {
            return Result(repository.ObterPaginadoDoTributarioJudicial(pagina, quantidade,
                EnumHelpers.ParseOrDefault(coluna, AcaoTributarioJudicialSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa ));
        }

        /// <summary>
        /// Cria um novo registro de ação do tributário judicial
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] IAcaoService service,
            [FromBody] CriarAcaoDoTributarioJudicialCommand dados) {
            return Result(service.CriarDoTributarioJudicial(dados));
        }

        /// <summary>
        /// Atualiza registro de ação do tributário judicial
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IAcaoService service,
            [FromBody] AtualizarAcaoDoTributarioJudicialCommand dados) {
            return Result(service.AtualizarDoTributarioJudicial(dados));
        }

        /// <summary>
        /// Atualiza registro de ação do tributario judicial
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Remover([FromServices] IAcaoService service,
            [FromRoute] int id) {
            return Result(service.RemoverDoTributarioJudicial(id));
        }

        /// <summary>
        /// Exporta as ações do tributário judicial
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pesquisa"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IAcaoRepository repository,
            [FromQuery] string coluna = "nome", 
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null) {
            var resultado = repository.ObterDoTributarioJudicial(EnumHelpers.ParseOrDefault(coluna, AcaoTributarioJudicialSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid) {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("CÓDIGO;DESCRIÇÃO");

                foreach (var a in resultado.Dados) {
                    csv.Append($"\"{a.Id}\";");
                    csv.Append($"\"{a.Descricao}\";");
                   // csv.Append($"\"{(a.Ativo ? "Sim" : "Não")}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"acoes_tributario_judicial_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}