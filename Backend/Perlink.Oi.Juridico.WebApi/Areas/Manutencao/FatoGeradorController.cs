using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Controller para requisições para Fato Gerador.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/fato-gerador")]
    public class FatoGeradorController : ApiControllerBase
    {
        /// <summary>
        /// Recupera os Fatos Geradores
        /// </summary>
        /// <param name="repository">Repositorio</param>
        /// <param name="coluna">Coluna Ordenacao</param>
        /// <param name="direcao"> Ordenacao</param>
        /// <param name="pagina"> Pagina Atual.</param>
        /// <param name="quantidade"> Quantidade por pagina.</param>
        /// <param name="pesquisa"> Texto Pesquisado.</param>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IFatoGeradorRepository repository,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] int pagina = 1,
            [FromQuery] int quantidade = 8,
            [FromQuery] string pesquisa = null)
        {
            return Result(repository.ObterPaginado(EnumHelpers.ParseOrDefault(coluna, FatoGeradorSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pagina, quantidade, pesquisa));         
        }

        /// <summary>
        /// Cria um novo Fato Gerador
        /// </summary>
        /// <param name="service">Instância do serviço de Fato Gerador</param>
        /// <param name="command">Command que será utilizada para novo Fato Gerador.</param>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IFatoGeradorService service,
            [FromBody] CriarFatoGeradorCommand command)
        {
            return Result(service.Criar(command));
        }

        /// <summary>
        /// Atualiza um novo Fato Gerador
        /// </summary>
        /// <param name="service"></param>
        /// <param name="command"></param>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IFatoGeradorService service,
            [FromBody] AtualizarFatoGeradorCommand command)
        {
            return Result(service.Atualizar(command));
        }

        /// <summary>
        /// Exclui um Fato Gerador
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IFatoGeradorService service,
            [FromRoute] int id)
        {
            return Result(service.Remover(id));
        }


        /// <summary>
        /// Exporta os Fato Geradores
        /// </summary>
        /// <param name="repository">Repositorio</param>
        /// <param name="tipoProcesso">Tipo Processo</param>
        /// <param name="coluna">Coluna Ordenacao</param>
        /// <param name="direcao"> Ordenacao</param>
        /// <param name="pesquisa"> Texto Pesquisado.</param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IFatoGeradorRepository repository,
            [FromQuery] string coluna = "nome", 
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, FatoGeradorSort.Id),string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Código;Nome;Ativo");

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.Id}\";");
                    csv.Append($"\"{(a.Nome)}\";");
                    csv.Append($"\"{(a.Ativo == true ? "Sim" : "Não")}\";");
                    csv.AppendLine("");
                }

                string  nomeArquivo = "FatoGerador_" + $"{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}