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
    /// EndPoint que resolve requisições de ações
    /// </summary>
    [Authorize]
    [Route("manutencao/acoes/civel-estrategico")]
    [ApiController]
    public class AcaoCivelEstrategicoController : ApiControllerBase {

        /// <summary>
        /// Recuperar todas as ações
        /// </summary>
        /// <param name="repository">repositório de ações</param>
        /// <param name="pagina">pagina</param>
        /// <param name="quantidade">quantidade</param>
        /// <param name="coluna">coluna para ordenação</param>
        /// <param name="direcao">direção para ordenação</param>
        /// <param name="pesquisa">pesquisa</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IAcaoRepository repository, [FromQuery] int pagina, [FromQuery] int quantidade,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null) {
            return Result(repository.ObterPaginadoDoCivelEstrategico(pagina, quantidade,
                EnumHelpers.ParseOrDefault(coluna, AcaoCivelEstrategicoSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa));
        }

        /// <summary>
        /// Insere uma nova ação do módulo cível estratégico
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] IAcaoService service,
            [FromBody] CriarAcaoDoCivelEstrategicoCommand dados) {
            return Result(service.CriarDoCivelEstrategico(dados));
        }

        /// <summary>
        /// Atualiza uma ação do módulo cível estratégico
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IAcaoService service,
            [FromBody] AtualizarAcaoDoCivelEstrategicoCommand dados) {
            return Result(service.AtualizarDoCivelEstrategico(dados));
        }

        /// <summary>
        /// Exclui uma ação do módulo cível estratégico
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Remover([FromServices] IAcaoService service, [FromRoute] int id) {
            return Result(service.RemoverDoCivelEstrategico(id));
        }

        /// <summary>
        /// Exporta as ações do módulo cível estratégico
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pesquisa"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar([FromServices] IAcaoRepository repository,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null) {
            var resultado = repository.ObterDoCivelEstrategico(EnumHelpers.ParseOrDefault(coluna, AcaoCivelEstrategicoSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid) {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("CÓDIGO;DESCRIÇÃO;ATIVO;CORREPONDENTE CÍVEL CONSUMIDOR (DE X PARA MIGRAÇÃO DO PROCESSO); CORRESPONDENTE CÍVEL CONSUMIDOR ATIVO");

                foreach (var a in resultado.Dados) {
                    csv.Append($"\"{a.Id}\";");
                    csv.Append($"\"{a.Descricao}\";");
                    csv.Append($@"{(a.Ativo ? "Sim" : "Não")};");
                    csv.Append($"\"{(string.IsNullOrEmpty(a.DescricaoMigracao) ? string.Empty : a.DescricaoMigracao)}\";");
                    if (a.DescricaoMigracao == null)
                    {
                        csv.Append($"\"{(a.AtivoDePara ? "Sim" : "")}\";");
                    }
                    else
                    {
                        csv.Append($"\"{(a.AtivoDePara ? "Sim" : "Não")}\";");
                    }
                    csv.AppendLine("");
                }

                string nomeArquivo = $"acoes_ce_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }

        [HttpGet("ObterDescricaoDeParaCivelEstrategico")]
        public IActionResult ObterDescricaoDeParaCivelEstrategico([FromServices] IAcaoRepository repository)
        {
            return Result(repository.ObterDescricaoDeParaCivelEstrategico());
        }
    }
}