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

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{

    /// <summary>
    /// Controller que resolve as requisições de ações do cível consumidor
    /// </summary>
    [Authorize]
    [Route("manutencao/acoes/civel-consumidor")]
    [ApiController]
    public class AcaoCivelConsumidorController : ApiControllerBase
    {

        /// <summary>
        /// Lista todas as ações do cível consumidor
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
            [FromQuery] string pesquisa = null)
        {

            return Result(repository.ObterPaginadoDoCivelConsumidor(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, AcaoCivelConsumidorSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa));
        }

        /// <summary>
        /// Cria um novo registro de ação do cível consumidor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] IAcaoService service,
            [FromBody] CriarAcaoDoCivelConsumidorCommand dados)
        {
            return Result(service.CriarDoCivelConsumidor(dados));
        }

        /// <summary>
        /// Atualiza registro de ação do cível consumidor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IAcaoService service,
            [FromBody] AtualizarAcaoDoCivelConsumidorCommand dados)
        {
            return Result(service.AtualizarDoCivelConsumidor(dados));
        }

        /// <summary>
        /// Exclui registro de ação do cível consumidor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Remover([FromServices] IAcaoService service, [FromRoute] int id)
        {
            return Result(service.RemoverDoCivelConsumidor(id));
        }

        /// <summary>
        /// Exporta as ações do cível consumidor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pesquisa"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IAcaoRepository repository,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            var resultado = repository.ObterDoCivelConsumidor(EnumHelpers.ParseOrDefault(coluna, AcaoCivelConsumidorSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("CÓDIGO;DESCRIÇÃO;NATUREZA DA AÇÃO BB; CORREPONDENTE CÍVEL ESTRATÉGICO (DE X PARA MIGRAÇÃO DE PROCESSO); CORRESPONDENTE CÍVEL ESTRATÉGICO ATIVO; ENVIAR PARA O APP PREPOSTO");

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.Id}\";");
                    csv.Append($"\"{a.Descricao}\";");
                    csv.Append($"\"{(a.NaturezaAcaoBB != null ? a.NaturezaAcaoBB.Nome : string.Empty)}\";");
                    csv.Append($"\"{(string.IsNullOrEmpty(a.DescricaoMigracao) ? string.Empty : a.DescricaoMigracao)}\";");
                    if (a.DescricaoMigracao == null)
                    {
                        csv.Append($"\"{(a.AtivoDePara ? "Sim" : "")}\";");
                    }
                    else
                    {
                        csv.Append($"\"{(a.AtivoDePara ? "Sim" : "Não")}\";");
                    }
                    csv.Append($"\"{(a.EnviarAppPreposto.GetValueOrDefault() ? "Sim" : "Não")}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"acoes_cc_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }

        [HttpGet("ObterDescricaoDeParaConsumidor")]
        public IActionResult ObterDescricaoDeParaConsumidor([FromServices] IAcaoRepository repository)
        {
            return Result(repository.ObterDescricaoDeParaConsumidor());
        }

    }
}