using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Controller da Manutencao Juros > Vigências Cíveis
    /// </summary>
    [Authorize]
    [Route("manutencao/juros-vigencias-civeis")]
    [ApiController]
    public class JurosVigenciasCiveisController : ApiControllerBase
    {
        /// <summary>
        /// Lista todos Juros > Vigências Cíveis
        /// </summary>
        /// <param name="tipoDeProcesso"></param>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        /// <param name="repository"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet("{tipoDeProcesso}")]
        public IActionResult ObterPaginado(
            [FromServices] IJurosVigenciasCiveisRepository repository,
            [FromRoute] int tipoDeProcesso, 
            [FromQuery] DateTime dataInicial,
            [FromQuery] DateTime dataFinal,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "inicioVigencia",
            [FromQuery] string direcao = "desc")
        {
            if (tipoDeProcesso == 0)
            {
                return Result(repository.obterParaComboboxTipoProcesso());
            }

            return Result(repository.ObterPaginado(TipoProcesso.PorId(tipoDeProcesso),dataInicial, dataFinal, pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, JurosVigenciasCiveisSort.InicioVigencia), string.IsNullOrEmpty(direcao) || direcao.Equals("asc")));
        }

        /// <summary>
        /// Cria um novo registro de juros vigências cíveis
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] IJurosVigenciasCiveisService service,
            [FromBody] CriarJurosVigenciasCiveisCommand dados)
        {
            return Result(service.Criar(dados));
        }

        /// <summary>
        /// Atualiza registro de juros vigências cívies
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IJurosVigenciasCiveisService service,
            [FromBody] AtualizarJurosVigenciasCiveisCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exclui registro juros vigências cíveis
        /// </summary>
        /// <param name="service"></param>
        /// <param name="codigoTipoProcesso"></param>
        /// <param name="dataVigencia"></param>
        /// <returns></returns>
        [HttpDelete("{codigoTipoProcesso}/{dataVigencia}")]
        public IActionResult Remover(
            [FromServices] IJurosVigenciasCiveisService service,
            [FromRoute] int codigoTipoProcesso,
            [FromRoute] DateTime dataVigencia)
        {
            return Result(service.Remover(codigoTipoProcesso, dataVigencia));
        }

        /// <summary>
        /// Exporta os juros vigências cívies
        /// </summary>
        /// <param name="tipoDeProcesso"></param>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        /// <param name="repository"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet("exportar/{tipoDeProcesso}")]
        public IActionResult Exportar(
            [FromServices] IJurosVigenciasCiveisRepository repository,
            [FromRoute] int tipoDeProcesso,
            [FromQuery] DateTime dataInicial,
            [FromQuery] DateTime dataFinal,
            [FromQuery] string coluna = "inicioVigencia", [FromQuery] string direcao = "desc")
        {
            var resultado = repository.Obter(TipoProcesso.PorId(tipoDeProcesso), dataInicial, dataFinal, EnumHelpers.ParseOrDefault(coluna, JurosVigenciasCiveisSort.InicioVigencia),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"));

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Início da Vigência;Valor da Taxa de Juros (%);Tipo de Processo");

                foreach (var item in resultado.Dados)
                {
                    csv.Append($"\"{item.DataVigencia:dd/MM/yyyy}\";");
                    csv.Append($"\"{(item.ValorJuros)}\";");
                    csv.Append($"\"{(item.TipoProcesso.Nome) }\"");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"Juros_Civeis_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}