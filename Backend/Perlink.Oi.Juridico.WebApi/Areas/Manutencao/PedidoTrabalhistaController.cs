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
    /// Resolve requisições de pedidos
    /// </summary>
    [Route("manutencao/pedidos/trabalhista")]
    [ApiController]
    [Authorize]
    public class PedidoTrabalhistaController : ApiControllerBase
    {
        /// <summary>
        /// Recuperar pedidos trabalhistas
        /// </summary>        
        [HttpGet]
        public IActionResult ObterPaginadoDoCivelEstrategico([FromServices] IPedidoRepository repository, [FromQuery] int pagina,
            [FromQuery] int quantidade = 8, [FromQuery]string coluna = "descricao", [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            return Result(repository.ObterPaginadoDoTrabalhista(EnumHelpers.ParseOrDefault(coluna, PedidoTrabalhistaSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pagina,
                quantidade, pesquisa));
        }

        /// <summary>
        /// Insere um novo pedido trabalhista
        /// </summary>
        [HttpPost]
        public IActionResult Criar([FromServices] IPedidoService service,
            [FromBody] CriarPedidoDoTrabalhistaCommand command)
        {
            return Result(service.CriarDoTrabalhista(command));
        }

        /// <summary>
        /// Atualiza um assunto civel estratégico
        /// <param name="service"></param>
        /// <param name="command"></param>
        /// </summary>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IPedidoService service,
            [FromBody] AtualizarPedidoDoTrabalhistaCommand command)
        {
            return Result(service.AtualizarDoTrabalhista(command));
        }

        /// <summary>
        /// Exclui um assunto cível estratégico       
        /// </summary>
        [HttpDelete("{pedidoId}")]
        public IActionResult Remover([FromServices] IPedidoService service,
            [FromRoute] int pedidoId)
        {
            return Result(service.RemoverDoTrabalhista(pedidoId));
        }

        /// <summary>
        /// Exporta os pedidos do módulo cível estratégico
        /// </summary>       
        [HttpGet("exportar")]
        public IActionResult Exportar([FromServices] IPedidoRepository repository, 
            [FromQuery] string coluna = "descricao", [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            var resultado = repository.ObterDoTrabalhista(EnumHelpers.ParseOrDefault(coluna, PedidoTrabalhistaSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid) {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("Código;Descrição;Risco de Perda Potencial Inicial (Trabalhista);Provável Zero;Próprio/Terceiro;Ativo");

                foreach (var x in resultado.Dados) {
                    csv.Append($"\"{x.Id}\";");
                    csv.Append($"\"{x.Descricao}\";");
                    csv.Append($"\"{x.RiscoPerda.Descricao}\";");
                    csv.Append($"\"{(x.ProvavelZero ? "Sim" : "Não")}\";");
                    csv.Append($"\"{x.ProprioTerceiro.Valor}\";");
                    csv.Append($"\"{(x.Ativo ? "Sim" : "Não")}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"pedidos_trabalhista_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}