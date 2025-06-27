using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Controller para requisições para orgão.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/preposto")]
    public class PrepostoController : ApiControllerBase
    {
        /// <summary>
        /// Recupera os Prepostos
        /// </summary>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IPrepostosRepository repository,
            [FromQuery] int pagina,
            [FromQuery] int quantidade = 8,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            return Result(repository.ObterPaginado(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, PrepostoSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa));
        }

        /// <summary>
        /// Recupera Alocacao por Preposto
        /// </summary>
        [HttpGet("Alocacoes")]
        public IActionResult ObterAlocacao(
           [FromServices] IPrepostosRepository repository,
           [FromQuery] int pagina,
           [FromQuery] int quantidade = 8,
           [FromQuery] string coluna = "nome",
           [FromQuery] string direcao = "asc",           
           [FromQuery] int prepostoId = 0,
           [FromQuery] string tiposProcessos = null)
        {
            return Result(repository.ObterAlocacoesFuturas(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, AlocacaoPrepostoSort.Data), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), prepostoId, tiposProcessos));
        }

        /// <summary>
        /// Recupera Staus Alocação
        /// </summary>
        [HttpGet("EstaAlocado")]
        public IActionResult EstaAlocado(
          [FromServices] IPrepostosRepository repository,
          [FromQuery] string tiposProcessos,
          [FromQuery] int prepostoId)
        {            
            return Result(repository.EstaAlocado(tiposProcessos, prepostoId));
        }
        
        /// <summary>
        /// Cria um novo Preposto
        /// </summary>
        /// <param name="service">Instância do serviço de Preposto</param>
        /// <param name="command">Command que será utilizada para novo Preposto.</param>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IPrepostosService service,
            [FromBody] CriarPrepostoCommand command)
        {
            return Result(service.Criar(command));
        }

        /// <summary>
        /// Atualiza um novo Preposto
        /// </summary>
        /// <param name="service"></param>
        /// <param name="command"></param>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IPrepostosService service,
            [FromBody] AtualizarPrepostoCommand command)
        {
            return Result(service.Atualizar(command));
        }

        /// <summary>
        /// Exclui um Preposto
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IPrepostosService service,
            [FromRoute] int id)
        {
            return Result(service.Remover(id));
        }

        /// <summary>
        /// Trata da exportação dos registros de Preposto
        /// </summary>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IPrepostosRepository repository,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = "")
        {
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, PrepostoSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Código;Nome;Matrícula;Cível Consumidor;Cível Estratégico;Juizado;Trabalhista;Procon;Pex;Ativo;É de Escritório;Usuário");

                foreach (var x in resultado.Dados)
                {
                    csv.Append($"\"{x.Id}\";");
                    csv.Append($"\"{x.Nome}\";");
                    csv.Append($@"{(String.IsNullOrEmpty(x.Matricula) ? "" : x.Matricula)};");
                    //csv.Append($"\"{x.EstadoId}\";");                    
                    csv.Append($@"{(x.EhCivel ? "Sim" :"Não" )};");
                    csv.Append($@"{(x.EhCivelEstrategico ? "Sim" : "Não")};");                    
                    csv.Append($@"{(x.EhJuizado ? "Sim" : "Não")};");
                    csv.Append($@"{(x.EhTrabalhista ? "Sim" : "Não")};");
                    csv.Append($@"{(x.EhProcon ? "Sim" : "Não")};");
                    csv.Append($@"{(x.EhPex ? "Sim" : "Não")};");
                    csv.Append($@"{(x.Ativo ? "Sim" : "Não")};");
                    csv.Append($@"{(x.EhEscritorio ? "Sim" : "Não")};");
                    csv.Append($@"{(x.NomeUsuario)};");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"Preposto_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }

        /// <summary>
        /// Trata da exportação dos registros de Preposto
        /// </summary>
        [HttpGet("exportarAlocacao")]
        public IActionResult ExportarAlocacao(
            [FromServices] IPrepostosRepository repository,
            [FromQuery] int prepostoId,
            [FromQuery] string tiposProcessos,
            [FromQuery] string coluna = "data",
            [FromQuery] string direcao = "asc")
        {
            var resultado = repository.ObterAlocacao(EnumHelpers.ParseOrDefault(coluna, AlocacaoPrepostoSort.Data), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), prepostoId, tiposProcessos);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Tipo de Alocação;Data;UF;Comarca;Vara;Tipo Vara;Nº do Processo;Tipo de Processo;Código Interno Processo");

                foreach (var x in resultado.Dados)
                {
                    csv.Append($"\"{x.Tipo}\";");
                    csv.Append($"\"{x.Data}\";");
                    csv.Append($"\"{x.UF}\";");
                    csv.Append($"\"{x.Comarca}\";");
                    csv.Append($@"{x.VaraId};");
                    csv.Append($@"{(x.VaraNome)};");
                    csv.Append($@"{(x.NumeroProcesso)};");
                    csv.Append($@"{(x.TipoProcesso)};");
                    csv.Append($@"{(x.CodProcessoInterno)};");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"Alocacoes_Futuras_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }

        /// <summary>
        /// verifica se já existe um preposto com o mesmo nome informado na tela
        /// </summary>
        /// <returns></returns>       
        [HttpGet("ValidarDuplicidadeDeNomePreposto")]
        public IActionResult ValidarDuplicidadeDeNomePreposto(
        [FromServices] IPrepostosRepository repository,
        [FromQuery] string nomePreposto,
        [FromQuery] int prepostoId)
        {
            return Result(repository.ValidarDuplicidadeDeNomePreposto(nomePreposto, prepostoId));
        }
    }
}