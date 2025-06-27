using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Manutencao.Sorts;
using System;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{

    /// <summary>
    /// Controller que resolve as requisições de fiel depositario
    /// </summary>
    [Authorize]
    [Route("manutencao/fiel-depositario")]
    [ApiController]   
    public class FielDepositarioController : ApiControllerBase
    {

        /// <summary>
        /// Recuperar fieis depositarios
        /// </summary>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IFielDepositarioRepository repository, 
            [FromQuery] int pagina,
            [FromQuery] int quantidade = 8, 
            [FromQuery] string coluna = "descricao", 
            [FromQuery] string direcao = "asc")

        {
            return Result(repository.ObterPaginado(pagina, quantidade,
                EnumHelpers.ParseOrDefault(coluna, FielDepositarioSort.Nome),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc")));
        }

        /// <summary>
        /// Insere um novo fiel depositário
        /// </summary>
        [HttpPost]
        public IActionResult Criar([FromServices] IFielDepositarioService service,
            [FromBody] CriarFielDepositarioCommand command)
        {
            return Result(service.CriarFielDepositario(command));
        }

        /// <summary>
        /// Atualiza um fiel depositário       
        /// </summary>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IFielDepositarioService service,
            [FromBody] AtualizarFielDepositarioCommand command)
        {
            return Result(service.AtualizarFielDepositario(command));
        }

        /// <summary>
        /// Exclui um fiel depositário
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover([FromServices] IFielDepositarioService service,
            [FromRoute] int id)
        {
            return Result(service.ExcluirFielDepositario(id));
        }


        /// <summary>
        /// Exporta os fieis depositários
        /// </summary>        
        [HttpGet("exportar")]
        public IActionResult Exportar([FromServices] IFielDepositarioRepository repository,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc")
            
        {
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, FielDepositarioSort.Nome),
            string.IsNullOrEmpty(direcao) || direcao.Equals("asc"));

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("ID;CPF;NOME");

                foreach (var x in resultado.Dados)
                {
                    csv.Append($"\"{x.Id}\";");
                    csv.Append($"\"{(!string.IsNullOrEmpty(x.Cpf) ? Convert.ToUInt64(x.Cpf).ToString(@"000\.000\.000\-00") : string.Empty)}\";");
                    csv.Append($"\"{x.Nome}\";");                 
                    csv.AppendLine("");
                }

                string nomeArquivo = $"fiel_depositario_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}