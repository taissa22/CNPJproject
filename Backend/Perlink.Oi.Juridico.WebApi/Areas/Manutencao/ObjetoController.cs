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
    /// Controller para requisições para Objeto.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/objeto")]
    public class ObjetoController : ApiControllerBase
    {
        /// <summary>
        /// Recupera os Objeto pelo tipo de processo.
        /// </summary>
        /// <param name="repository">Repositorio</param>
        /// <param name="tipoProcesso">Tipo Processo</param>
        /// <param name="coluna">Coluna Ordenacao</param>
        /// <param name="direcao"> Ordenacao</param>
        /// <param name="pagina"> Pagina Atual.</param>
        /// <param name="quantidade"> Quantidade por pagina.</param>
        /// <param name="pesquisa"> Texto Pesquisado.</param>
        /// ObjetoSort sort, bool ascending, int pagina, int quantidade, DataString? descricao
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IObjetoRepository repository,
            [FromQuery] int tipoProcesso,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] int pagina = 1,
            [FromQuery] int quantidade = 8,
            [FromQuery] string pesquisa = null)
        {
            return Result(repository.ObterPaginado(tipoProcesso, EnumHelpers.ParseOrDefault(coluna, ObjetoSort.Descricao), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pagina, quantidade, pesquisa));         
        }

        /// <summary>
        /// Cria um novo Objeto
        /// </summary>
        /// <param name="service">Instância do serviço de Objeto</param>
        /// <param name="command">Command que será utilizada para novo objeto.</param>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IObjetoService service,
            [FromBody] CriarObjetoCommand command)
        {
            return Result(service.Criar(command));
        }

        /// <summary>
        /// Atualiza um novo Objeto
        /// </summary>
        /// <param name="service"></param>
        /// <param name="command"></param>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IObjetoService service,
            [FromBody] AtualizarObjetoCommand command)
        {
            return Result(service.Atualizar(command));
        }

        /// <summary>
        /// Exclui um Objeto
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IObjetoService service,
            [FromRoute] int id)
        {
            return Result(service.Remover(id));
        }


        /// <summary>
        /// Exporta os OBjeto
        /// </summary>
        /// <param name="repository">Repositorio</param>
        /// <param name="tipoProcesso">Tipo Processo</param>
        /// <param name="coluna">Coluna Ordenacao</param>
        /// <param name="direcao"> Ordenacao</param>
        /// <param name="pesquisa"> Texto Pesquisado.</param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IObjetoRepository repository,
            [FromQuery] int tipoProcesso,
            [FromQuery] string coluna = "descricao", 
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            var resultado = repository.Obter(tipoProcesso, EnumHelpers.ParseOrDefault(coluna, ObjetoSort.Id),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();

                if (TipoProcesso.PorId(tipoProcesso) == TipoProcesso.TRABALHISTA_ADMINISTRATIVO)
                    csv.AppendLine($"Código;Descrição;Tipo de Processo");
                else
                    csv.AppendLine($"Código;Descrição;Objeto Administrativo;Objeto Judicial;Grupo de Objeto;Ativo Administrativo;Ativo Judicial;Tipo de Processo");

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.Id}\";");
                    csv.Append($"\"{(a.Descricao)}\";");
                    if (TipoProcesso.PorId(tipoProcesso) == TipoProcesso.TRABALHISTA_ADMINISTRATIVO)
                        csv.Append($"\"{(TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Nome)}\";");
                    else
                    {
                        csv.Append($"\"{(a.EhTributarioAdministrativo ? "Sim" : "Não")}\";");
                        csv.Append($"\"{(a.EhTributarioJudicial ? "Sim" : "Não")}\";");
                        csv.Append($"\"{(a.GrupoPedidoDescricao)}\";");
                        csv.Append($"\"{(a.AtivoTributarioAdministrativo ? "Sim" : "Não")}\";");
                        csv.Append($"\"{(a.AtivoTributarioJudicial ? "Sim" : "Não")}\";");
                        csv.Append($"\"{("Tributário")}\";");
                    }
                    csv.AppendLine("");
                }
                string nomeArquivo;
                nomeArquivo =  (TipoProcesso.PorId(tipoProcesso) == TipoProcesso.TRABALHISTA_ADMINISTRATIVO ? $"Objeto_TrabalhistaAdm_" : $"Objeto_Tributario_");
                nomeArquivo = nomeArquivo + $"{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";

                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}