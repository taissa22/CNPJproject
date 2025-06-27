using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{

    /// <summary>
    /// Controller da Manutencao Cotação Indice Trabalhista
    /// </summary>
    [Authorize]
    [Route("manutencao/cotacao-indice-trabalhista")]
    [ApiController]
    public class CotacaoIndiceTrabalhistaController : ApiControllerBase
    {
        /// <summary>
        /// Lista todas os tipos de pendências
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataCorrecao"></param>
        /// <param name="dataBaseInicial"></param>
        /// <param name="dataBaseFinal"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] ICotacaoIndiceTrabalhistaRepository repository,
            [FromQuery] DateTime dataCorrecao,
            [FromQuery] DateTime dataBaseInicial,
            [FromQuery] DateTime dataBaseFinal,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc")
        {
            return Result(repository.ObterPaginado(dataCorrecao, dataBaseInicial, dataBaseFinal, EnumHelpers.ParseOrDefault(coluna, CotacaoIndiceTrabalhistaSort.DataBase),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pagina, quantidade));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>

        [HttpGet("ObterPaginadoTemp")]
        public IActionResult ObterPaginadoTemp(
           [FromServices] ICotacaoIndiceTrabalhistaRepository repository,
           [FromQuery] int pagina,
           [FromQuery] int quantidade,
           [FromQuery] string coluna = "nome",
           [FromQuery] string direcao = "asc")
        {
            return Result(repository.ObterPaginadoTemp(EnumHelpers.ParseOrDefault(coluna, CotacaoIndiceTrabalhistaSort.DataBase),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pagina, quantidade));

        }

        /// <summary>
        /// Exclui registro de tipo de pendência
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dataCorrecao"></param>
        /// /// <param name="dataBase"></param>
        /// <returns></returns>
        [HttpDelete("{dataCorrecao}/{dataBase}")]
        public IActionResult Remover(
            [FromServices] ICotacaoIndiceTrabalhistaService service,
            [FromRoute] DateTime dataCorrecao,
            [FromRoute] DateTime dataBase)
        {
            return Result(service.Remover(dataCorrecao, dataBase));
        }

        /// <summary>
        /// Exporta os tipos de pendência
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataCorrecao"></param>
        /// <param name="dataBaseInicial"></param>
        /// <param name="dataBaseFinal"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] ICotacaoIndiceTrabalhistaRepository repository,
            [FromQuery] DateTime dataCorrecao,
            [FromQuery] DateTime dataBaseInicial,
            [FromQuery] DateTime dataBaseFinal,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc")
        {
            var resultado = repository.Obter(dataCorrecao, dataBaseInicial, dataBaseFinal, EnumHelpers.ParseOrDefault(coluna, CotacaoIndiceTrabalhistaSort.DataBase),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"));

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                var colunas = $"Mês/Ano de Correção;Mês/Ano de Distribuição;Valor";

                csv.AppendLine(colunas);

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.DataBase:MM/yyyy}\";");
                    csv.Append($"\"{a.DataCorrecao:MM/yyyy}\";");
                    csv.Append($"\"{a.ValorCotacao}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"CotacaoIndiceTrabalhista_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpGet("AplicarImportacao")]
        public IActionResult AplicarImportacao(
           [FromServices] ICotacaoIndiceTrabalhistaService service)
        {
            return Ok(service.AplicarImportacao());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arquivo"></param>
        /// <param name="dataCotacao"></param>
        /// <param name="repository"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public IActionResult upload([FromForm] IFormFile arquivo, [FromForm] DateTime dataCotacao, [FromServices] ICotacaoIndiceTrabalhistaRepository repository, [FromServices] ICotacaoIndiceTrabalhistaService service)
        {
            List<CotacaoIndiceTrabalhista> lst = new List<CotacaoIndiceTrabalhista>();

            var resultado = repository.Validar(arquivo, dataCotacao, ref lst);
            if (resultado.Dados == string.Empty)
            {
                service.InserirTemporaria(lst);
            }
            return Ok(resultado);
         
        }


    }
}

