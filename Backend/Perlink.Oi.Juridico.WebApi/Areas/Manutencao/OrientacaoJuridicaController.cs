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

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao {

    /// <summary>
    /// Controller para requisições para Orientacao Juridica.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/orientacao-juridica")]
    public class OrientacaoJuridicaController : ApiControllerBase {
        
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IOrientacaoJuridicaRepository repository, 
            [FromQuery] bool obterTrabalhista,
            [FromQuery] int pagina, 
            [FromQuery] int quantidade = 8,            
            [FromQuery] string coluna = "nome", 
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null) {
                
            return Result(repository.ObterPaginado(obterTrabalhista, EnumHelpers.ParseOrDefault(coluna, OrientacaoJuridicaSort.Nome),string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pagina, quantidade, pesquisa));
        }

        /// <summary>
        /// Cria uma nova Orientacao Juridica
        /// </summary>
        /// <param name="service">Instância do serviço de orientacao Juridica</param>
        /// <param name="command">Command que será utilizada para novo objeto.</param>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IOrientacaoJuridicaService service,
            [FromBody] CriarOrientacaoJuridicaCommand command) {
            return Result(service.Criar(command));
        }

        /// <summary>
        /// Atualiza uma nova Orientacao Juridica
        /// </summary>
        /// <param name="service"></param>
        /// <param name="command"></param>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IOrientacaoJuridicaService service,
            [FromBody] AtualizarOrientacaoJuridicaCommand command) {
            return Result(service.Atualizar(command));
        }

        /// <summary>
        /// Exclui um Orientacao Juridica
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IOrientacaoJuridicaService service,
            [FromRoute] int id) {
            return Result(service.Remover(id));
        }

        /// <summary>
        /// Trata da exportação dos registros de Orientacao Juridica
        /// </summary>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IOrientacaoJuridicaRepository repository,
            [FromQuery] bool obterTrabalhista,            
            [FromQuery] string coluna = "nome", 
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null) {           

            var resultado = repository.Obter(obterTrabalhista, EnumHelpers.ParseOrDefault(coluna, OrientacaoJuridicaSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),pesquisa);

            if (resultado.Tipo == ResultType.Valid) {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Código; Tipo Orientação; Nome;Situação ");

                foreach (var x in resultado.Dados) {
                    csv.Append($"\"{x.CodOrientacaoJuridica}\";");
                    csv.Append($"\"{x.TipoOrientacaoJuridica.Descricao}\";");
                    csv.Append($"\"{x.Nome}\";");
                    csv.Append($"\"{(x.Ativo ? "Ativo" : "Inativo")}\";");
                    csv.AppendLine("");
                    continue;                                    
                }

                string nomeArquivo = $"OrientacaoJuridicaTrabalhista_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}