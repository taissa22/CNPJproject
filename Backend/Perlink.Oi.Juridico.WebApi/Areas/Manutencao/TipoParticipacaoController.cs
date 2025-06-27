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
    /// Controller da Manutencao Cotação
    /// </summary>
    [Authorize]
    [Route("manutencao/tipos-de-participacao")]
    [ApiController]
    public class TipoDeParticipacaoController : ApiControllerBase
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
            [FromServices] ITipoDeParticipacaoRepository repository,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "descricao",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            return Result(repository.ObterPaginado(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, TipoDeParticipacaoSort.Codigo), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa));
        }

        /// <summary>
        /// Cria um novo registro de tipo de pendência
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] ITipoDeParticipacaoService service,
            [FromBody] CriarTipoDeParticipacaoCommand dados)
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
        public IActionResult Atualizar(
            [FromServices] ITipoDeParticipacaoService service,
            [FromBody] AtualizarTipoDeParticipacaoCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exclui registro de tipo de pendência
        /// </summary>
        /// <param name="service"></param>
        /// <param name="codigoTipoDeParticipacao"></param>
        /// <returns></returns>
        [HttpDelete("{codigoTipoDeParticipacao}")]
        public IActionResult Remover(
            [FromServices] ITipoDeParticipacaoService service,
            [FromRoute] int codigoTipoDeParticipacao)
        {
            return Result(service.Remover(codigoTipoDeParticipacao));
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
            [FromServices] ITipoDeParticipacaoRepository repository,
            [FromQuery] string coluna = "descricao", [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, TipoDeParticipacaoSort.Codigo),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Código;Descrição");

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.Codigo}\";");
                    csv.Append($"\"{(a.Descricao)}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"Tipo_Participacao_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}
