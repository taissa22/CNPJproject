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
using System.Globalization;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{

    /// <summary>
    /// Controller para requisições para Usuario Operacao Retroativa.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/usuario-operacao-retroativa")]
    public class UsuarioOperacaoRetroativaController : ApiControllerBase
    {

        /// <summary>
        /// Recupera os Usuario Configuracao Retroativa pelo tipo de processo.
        /// </summary>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IUsuarioOperacaoRetroativaRepository repository,            
            [FromQuery] int pagina,
            [FromQuery] int quantidade = 8,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null
            )
        {

            return Result(repository.ObterPaginado(EnumHelpers.ParseOrDefault(coluna, UsuarioOperacaoRetroativaSort.Usuario),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pagina, quantidade, pesquisa));
        }

        /// <summary>
        /// Cria um novo Usuario Configuracao Retroativa
        /// </summary>
        /// <param name="service">Instância do serviço de orgão</param>
        /// <param name="command">Command que será utilizada para novo objeto.</param>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IUsuarioOperacaoRetroativaService service,
            [FromBody] CriarUsuarioOperacaoRetroativaCommand command)
        {
            return Result(service.Criar(command));
        }

        /// <summary>
        /// Atualiza um novo Usuario Configuracao Retroativa
        /// </summary>
        /// <param name="service"></param>
        /// <param name="command"></param>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IUsuarioOperacaoRetroativaService service,
            [FromBody] AtualizarUsuarioOperacaoRetroativaCommand command)
        {
            return Result(service.Atualizar(command));
        }

        /// <summary>
        /// Exclui uma Configuração Retroativa
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IUsuarioOperacaoRetroativaService service,
            [FromRoute] string id)
        {
            return Result(service.Remover(id));
        }

        /// <summary>
        /// Trata da exportação dos registros de Usuario Configuracao Retroativa
        /// </summary>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IUsuarioOperacaoRetroativaRepository repository,           
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {


            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, UsuarioOperacaoRetroativaSort.Usuario),
            string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Usuário; Tipo Processo; Dia Limite");

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{(a.Usuario.Ativo ? a.Usuario.Nome : a.Usuario.Nome + " [Inativo]")}\";");
                    csv.Append($"\"{TipoProcesso.PorId(a.TipoProcesso).Nome}\";");
                    csv.Append($"\"{a.LimiteAlteracao}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"OperacoesRetroativas_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}