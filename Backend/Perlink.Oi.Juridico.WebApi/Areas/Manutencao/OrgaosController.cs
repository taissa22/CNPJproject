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
    /// Controller para requisições para orgão.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/orgaos")]
    public class OrgaosController : ApiControllerBase {

        /// <summary>
        /// Recupera os orgãos pelo tipo de processo.
        /// </summary>
        [HttpGet]
        public IActionResult ObterPaginado([FromServices] IOrgaoRepository repository, [FromQuery] string tipoOrgao,
            [FromQuery] int pagina, [FromQuery] int quantidade = 8,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc") {
            if (!TipoOrgao.IsValid(tipoOrgao)) {
                return BadRequest("Tipo orgão não encontrado");
            }

            return Result(repository.ObterPaginado(TipoOrgao.PorValor(tipoOrgao), EnumHelpers.ParseOrDefault(coluna, OrgaoSort.Nome),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pagina, quantidade));
        }

        /// <summary>
        /// Cria um novo orgão
        /// </summary>
        /// <param name="service">Instância do serviço de orgão</param>
        /// <param name="command">Command que será utilizada para novo objeto.</param>
        [HttpPost]
        public IActionResult Criar([FromServices] IOrgaoService service,
            [FromBody] CriarOrgaoCommand command) {
            return Result(service.Criar(command));
        }

        /// <summary>
        /// Atualiza um novo orgão
        /// </summary>
        /// <param name="service"></param>
        /// <param name="command"></param>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IOrgaoService service,
            [FromBody] AtualizarOrgaoCommand command) {
            return Result(service.Atualizar(command));
        }

        /// <summary>
        /// Exclui um orgão
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover([FromServices] IOrgaoService service,
            [FromRoute] int id) {
            return Result(service.Remover(id));
        }

        /// <summary>
        /// Trata da exportação dos registros de orgão
        /// </summary>
        [HttpGet("exportar")]
        public IActionResult Exportar([FromServices] IOrgaoRepository repository, [FromQuery] string tipoOrgao,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc") {
            if (!TipoOrgao.IsValid(tipoOrgao)) {
                return BadRequest("Tipo orgão não encontrado");
            }

            var resultado = repository.Obter(TipoOrgao.PorValor(tipoOrgao),
                EnumHelpers.ParseOrDefault(coluna, OrgaoSort.Nome),
            string.IsNullOrEmpty(direcao) || direcao.Equals("asc"));

            if (resultado.Tipo == ResultType.Valid) {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"NOME; DDD; TELEFONE; {(tipoOrgao == "1" ? "PROCURADORIA" : "COMPETENCIA")}");

                foreach (var x in resultado.Dados) {
                    if (x.Competencias is null || x.Competencias.Count() == 0) {
                        csv.Append($"\"{x.Nome}\";");
                        csv.Append($"\"{(!string.IsNullOrEmpty(x.TelefoneDDD) ? Convert.ToUInt64(x.TelefoneDDD).ToString() : string.Empty)}\";");
                        csv.Append($"\"{(!string.IsNullOrEmpty(x.Telefone) ? Convert.ToUInt64(x.Telefone).ToString(@"0 0000-0000") : string.Empty)}\";");
                        csv.AppendLine("");
                        continue;
                    }

                    foreach (var competencia in x.Competencias) {
                        csv.Append($"\"{x.Nome}\";");
                        csv.Append($"\"{(!string.IsNullOrEmpty(x.TelefoneDDD) ? Convert.ToUInt64(x.TelefoneDDD).ToString() : string.Empty)}\";");
                        csv.Append($"\"{(!string.IsNullOrEmpty(x.Telefone) ? Convert.ToUInt64(x.Telefone).ToString(@"0 0000-0000") : string.Empty)}\";");
                        csv.Append($"\"{(competencia != null ? competencia.Nome : string.Empty)}\";");
                        csv.AppendLine("");
                    }
                }

                string nomeArquivo = $"Orgao_{(tipoOrgao == "2" ? "Criminal_Adm" : tipoOrgao == "1" ? "Civel_Adm" : "Demais_Tipos")}_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}