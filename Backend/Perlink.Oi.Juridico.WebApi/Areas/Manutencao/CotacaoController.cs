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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{

    /// <summary>
    /// Controller da Manutencao Cotação
    /// </summary>
    [Authorize]
    [Route("manutencao/cotacao")]
    [ApiController]
    public class CotacaoController : ApiControllerBase
    {
        /// <summary>
        /// Lista todas os tipos de pendências
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="codigoIndice"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="pesquisa"></param>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        /// <returns></returns>
        [HttpGet("{codigoIndice}")]
        public IActionResult ObterPaginado(
            [FromServices] ICotacaoRepository repository,
            [FromRoute] int codigoIndice,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] DateTime dataInicial,
            [FromQuery] DateTime dataFinal,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            return Result(repository.ObterPaginado(pagina, quantidade, codigoIndice, EnumHelpers.ParseOrDefault(coluna, CotacaoSort.DataCotacao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),
               dataInicial, dataFinal, pesquisa));
        }

        /// <summary>
        /// Cria um novo registro de tipo de pendência
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] ICotacaoService service,
            [FromBody] CriarCotacaoCommand dados)
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
        public IActionResult Atualizar([FromServices] ICotacaoService service,
            [FromBody] AtualizarCotacaoCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exclui registro de tipo de pendência
        /// </summary>
        /// <param name="service"></param>
        /// <param name="codigoIndice"></param>
        /// /// <param name="dataCotacao"></param>
        /// <returns></returns>
        [HttpDelete("{codigoIndice}/{dataCotacao}")]
        public IActionResult Remover(
            [FromServices] ICotacaoService service,
            [FromRoute] int codigoIndice,
            [FromRoute] DateTime dataCotacao)
        {
            return Result(service.Remover(codigoIndice, dataCotacao));
        }

        /// <summary>
        /// Exporta os tipos de pendência
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pesquisa"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="codigoIndice"></param>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        /// <returns></returns>
        [HttpGet("exportar/{codigoIndice}")]
        public IActionResult Exportar(
            [FromServices] ICotacaoRepository repository,
            [FromRoute] int codigoIndice,
            [FromQuery] DateTime dataInicial,
            [FromQuery] DateTime dataFinal,
            [FromQuery] string coluna = "descricao", [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            var resultado = repository.Obter(codigoIndice, EnumHelpers.ParseOrDefault(coluna, CotacaoSort.DataCotacao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), dataInicial, dataFinal, DataString.FromNullableString(pesquisa));

            if (resultado.Tipo == ResultType.Valid)
            {
                string codigoValorIndice = resultado.Dados.FirstOrDefault().Indice.CodigoValorIndice;
                var indiceAcumulado = resultado.Dados.FirstOrDefault().Indice.Acumulado;
                var tipoIndice = codigoValorIndice == "F" ? "Fator" : codigoValorIndice == "P" ? "Percentual" : "Valor";               

                StringBuilder csv = new StringBuilder();

                var colunas = $"Mês/Ano;{tipoIndice};";

                if (indiceAcumulado.GetValueOrDefault())
                {
                    colunas += $"Valor Acumulado;";
                }

                colunas += $"Índice;";

                csv.AppendLine(colunas);

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.DataCotacao:MM/yyyy}\";");
                    csv.Append($"\"{(a.Valor)}\";");
                    if (indiceAcumulado.GetValueOrDefault())
                    {
                        csv.Append($"\"{a.ValorAcumulado.GetValueOrDefault().ToString("#,###.0000")}\";");
                    }
                    csv.Append($"\"{a.Indice.Descricao}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"Cotacao_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }


       


    }
}
