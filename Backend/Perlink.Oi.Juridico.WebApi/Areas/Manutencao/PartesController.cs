using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Controller da reuisição de partes
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/partes")]
    public class PartesController : ApiControllerBase
    {/// <summary>
    /// Ordenação de partes
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="pagina"></param>
    /// <param name="quantidade"></param>
    /// <param name="coluna"></param>
    /// <param name="direcao"></param>
    /// <param name="nome"></param>
    /// <param name="tipoParte"></param>
    /// <param name="documento"></param>
    /// <param name="carteiraDeTrabalho"></param>
    /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IParteRepository repository,
            [FromQuery] int pagina, [FromQuery] int quantidade = 8,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc",
            [FromQuery] string nome = null, [FromQuery] string tipoParte = null, [FromQuery] string documento = null, [FromQuery] int? carteiraDeTrabalho = null)
        {
            return Result(repository.ObterPaginado(
                EnumHelpers.ParseOrDefault(coluna, ParteSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),
                pagina, quantidade, TipoParte.PorValor(tipoParte), nome, documento, carteiraDeTrabalho));
        }

        /// <summary>
        /// Cria uma parte
        /// </summary>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IParteService service,
            [FromBody] CriarParteCommand command)
        {
            return Result(service.Criar(command));
        }

        /// <summary>
        /// Atualiza uma parte
        /// </summary>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IParteService service,
            [FromBody] AtualizarParteCommand command)
        {
            return Result(service.Atualizar(command));
        }

        /// <summary>
        /// Deleta a parte
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IParteService service,
            [FromRoute] int id)
        {
            return Result(service.Remover(id));
        }

        /// <summary>
        /// Lista todos os partes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IParteRepository repository,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc",
            [FromQuery] string nome = null, [FromQuery] string tipoParte = null, [FromQuery] string documento = null, [FromQuery] int? carteiraDeTrabalho = null)
        {
            var resultado = repository.Obter(
                EnumHelpers.ParseOrDefault(coluna, ParteSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),
                TipoParte.PorValor(tipoParte), nome, documento, carteiraDeTrabalho);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();

                csv.AppendLine("TIPO;NOME;ESTADO;CPF/CNPJ;CARTEIRA DE TRABALHO");

                foreach (var a in resultado.Dados)
                {
                    string documentoFormatado = string.Empty;

                    if (!string.IsNullOrEmpty(a.CPF) && a.CPF.Length == 11)
                    {
                        documentoFormatado = Convert.ToUInt64(a.CPF).ToString(@"000\.000\.000\-00");
                    }
                    else if (!string.IsNullOrEmpty(a.CPF))
                    {
                        documentoFormatado = a.CPF.ToString();
                    }

                    if (!string.IsNullOrEmpty(a.CNPJ) && a.CNPJ.Length == 14)
                    {
                        documentoFormatado = Convert.ToUInt64(a.CNPJ).ToString(@"00\.000\.000\/0000\-00");
                    }
                    else if (!string.IsNullOrEmpty(a.CNPJ))
                    {
                        documentoFormatado = a.CNPJ.ToString();
                    }

                    csv.Append($"\"{(!string.IsNullOrEmpty(a.TipoParte.Descricao) ? a.TipoParte.Descricao : string.Empty)}\";");
                    csv.Append($"\"{(!string.IsNullOrEmpty(a.Nome) ? a.Nome : string.Empty)}\";");                    
                    csv.Append($"\"{(a.Estado != null ? a.Estado.Id : "")}\";");
                    csv.Append($"\"{documentoFormatado}\";");
                    csv.Append($"\"{(a.CarteiraDeTrabalho != null ? a.CarteiraDeTrabalho.ToString() : "")}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"Parte_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
            return Result(resultado);
        }
    }
}