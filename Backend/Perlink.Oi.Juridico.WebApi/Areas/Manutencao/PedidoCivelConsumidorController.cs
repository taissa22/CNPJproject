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

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao {

    /// <summary>
    /// Resolve requisições de pedidos
    /// </summary>
    [Authorize]
    [Route("manutencao/pedidos/civel-consumidor")]
    [ApiController]
    public class PedidoCivelConsunmidoroController : ApiControllerBase {

        /// <summary>
        /// Recuperar pedidos civel estratégico
        /// </summary>
        [HttpGet]
        public IActionResult ObterPaginadoDoCivelConsumidor([FromServices] IPedidoRepository repository, [FromQuery] int pagina,
            [FromQuery] int quantidade = 8, 
            [FromQuery] string coluna = "descricao", 
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            return Result(repository.ObterPaginadoDoCivelConsumidor(EnumHelpers.ParseOrDefault(coluna, PedidoCivelConsumidorSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pagina,
                quantidade, pesquisa));
        }


        /// <summary>
        /// Insere um novo pedido cível estratégico
        /// </summary>
        [HttpPost]
        public IActionResult Criar([FromServices] IPedidoService service,
            [FromBody] CriarPedidoDoCivelConsumidorCommand command) {
            return Result(service.CriarDoCivelConsumidor(command));
        }

        /// <summary>
        /// Atualiza um assunto civel estratégico
        /// <param name="service"></param>
        /// <param name="command"></param>
        /// </summary>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IPedidoService service,
            [FromBody] AtualizarPedidoDoCivelConsumidorCommand command) {
            return Result(service.AtualizarDoCivelConsumidor(command));
        }

        /// <summary>
        /// Exclui um assunto cível estratégico
        /// </summary>
        [HttpDelete("{pedidoId}")]
        public IActionResult Remover([FromServices] IPedidoService service,
            [FromRoute] int pedidoId) {
            return Result(service.RemoverDoCivelConsumidor(pedidoId));
        }

        /// <summary>
        /// Exporta os pedidos do módulo cível estratégico
        /// </summary>
        [HttpGet("exportar")]
        public IActionResult Exportar([FromServices] IPedidoRepository repository,
            [FromQuery] string coluna = "descricao", [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null) {
            var resultado = repository.ObterDoCivelConsumidor(EnumHelpers.ParseOrDefault(coluna, PedidoCivelConsumidorSort.Descricao),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid) {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("CÓDIGO;DESCRIÇÃO;NECESSITA ATUALIZAÇÃO DE DÉBITO PRÓXIMO À AUDIÊNCIA;ATIVO;CORRESPONDENTE CÍVEL ESTRATÉGICO (DE X PARA MIGRAÇÃO DE PROCESSO);CORRESPONDENTE CÍVEL ESTRATÉGICO ATIVO");

                foreach (var x in resultado.Dados) {
                    csv.Append($"\"{x.Id}\";");
                    csv.Append($"\"{x.Descricao}\";");
                    csv.Append($@"{(x.Audiencia ? "Sim" : "Não")};");
                    csv.Append($@"{(x.Ativo ? "Sim" : "Não")};");
                    csv.Append($"\"{(string.IsNullOrEmpty(x.DescricaoPara) ? string.Empty : x.DescricaoPara)}\";");
                    if (x.DescricaoPara == null)
                    {
                        csv.Append($"\"{(x.AtivoPara ? "Sim" : "")}\";");
                    }
                    else
                    {
                        csv.Append($"\"{(x.AtivoPara ? "Sim" : "Não")}\";");
                    }
                    csv.AppendLine("");
                }

                string nomeArquivo = $"pedidos_cc_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }

        /// <summary>
        /// Retorna dados para lista DE/PARA Ce
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        [HttpGet("ObterDescricaoDeParaCivelConsumidor")]
        public IActionResult ObterDescricaoDeParaCivelConsumidor([FromServices] IPedidoRepository repository)
        {
            return Result(repository.ObterDescricaoDeParaCivelConsumidor());
        }
    }
}