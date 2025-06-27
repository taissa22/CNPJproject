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
    /// Controller responsável pelas requisições de estabelecimento.
    /// </summary>
    [Authorize]
    [Route("manutencao/estabelecimentos")]
    [ApiController]
    public class EstabelecimentosController : ApiControllerBase
    {
        /// <summary>
        /// Lista todos estabelecimentos cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IEstabelecimentoRepository repository,           
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "nome", 
            [FromQuery] string direcao = "asc", 
            [FromQuery] string nome = null
            )
        {
            return Result(repository.ObterPaginado(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, EstabelecimentoSort.Nome),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), nome));
        }

        /// <summary>
        /// Cria um novo estabelecimento
        /// </summary>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IEstabelecimentoService service,
            [FromBody] CriarEstabelecimentoCommand command)
        {
            return Result(service.Criar(command));
        }

        /// <summary>
        /// Atualiza um novo orgão
        /// </summary>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IEstabelecimentoService service,
            [FromBody] AtualizarEstabelecimentoCommand command)
        {
            return Result(service.Atualizar(command));
        }

        /// <summary>
        /// Exclui um estabelecimento
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IEstabelecimentoService service,
            [FromRoute] int id)
        {
            return Result(service.Remover(id));
        }

        /// <summary>
        /// Exporta as empresa do grupo
        /// </summary>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IEstabelecimentoRepository repository,
            [FromQuery] string coluna = "nome", 
            [FromQuery] string direcao = "asc", 
            [FromQuery] string nome = null)
        {
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, EstabelecimentoSort.Nome),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), nome);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("CNPJ; NOME; ENDERECO; BAIRRO; CIDADE; ESTADO; CEP; TELEFONE; CELULAR");

                foreach (var x in resultado.Dados)
                {
                    csv.Append($"\"{(x.CNPJ != null ? Convert.ToUInt64(x.CNPJ).ToString(@"00\.000\.000\/0000\-00") : string.Empty)}\";");
                    csv.Append($"\"{(!string.IsNullOrEmpty(x.Nome) ? x.Nome : string.Empty)}\";");
                    csv.Append($"\"{(!string.IsNullOrEmpty(x.Endereco) ? x.Endereco : string.Empty)}\";");
                    csv.Append($"\"{(!string.IsNullOrEmpty(x.Bairro) ? x.Bairro : string.Empty)}\";");
                    csv.Append($"\"{(!string.IsNullOrEmpty(x.Cidade) ? x.Cidade : string.Empty)}\";");
                    csv.Append($"\"{(x.Estado != null ? x.Estado.Id : string.Empty)}\";");
                    csv.Append($"\"{(!string.IsNullOrEmpty(x.CEP) ? x.CEP : string.Empty)}\";");
                    csv.Append($"\"{(!string.IsNullOrEmpty(x.Telefone) ? Convert.ToUInt64(string.Concat(x.Telefone.Where(c => !char.IsWhiteSpace(c)))).ToString(@"0000-0000") : string.Empty)}\";");
                    csv.Append($"\"{x.Celular}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"Estabelecimentos_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}