using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using System;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Controller das requisições de Empresas do Grupo.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/empresas-do-grupo")]
    public class EmpresasDosGruposController : ApiControllerBase
    {
        /// <summary>
        /// Recupera as empresas de acordo com o filtro passado.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ObterPaginado(
            [FromServices] IEmpresaDoGrupoRepository repository,
            [FromQuery] int pagina, [FromQuery] int quantidade = 8,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc",
            [FromQuery] string nome = null, [FromQuery] string cnpj = null, [FromQuery] string centroSap = null)
        {
            try {
                return Result(repository.ObterPaginado(
                    EnumHelpers.ParseOrDefault(coluna, EmpresaDoGrupoSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),
                    pagina, quantidade, DataString.FromNullableString(nome), CNPJ.FromNullableString(cnpj), DataString.FromNullableString(centroSap)));

            } catch (Exception ex) {

                return Result(CommandResult.Invalid(ex.Message));
            }
        }

        /// <summary>
        /// Exporta as empresa do grupo
        /// </summary>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IEmpresaDoGrupoRepository repository,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc",
            [FromQuery] string nome = null, [FromQuery] string cnpj = null, [FromQuery] string centroSap = null)
        {
            var resultado = repository.Obter(
                EnumHelpers.ParseOrDefault(coluna, EmpresaDoGrupoSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),
                DataString.FromNullableString(nome), CNPJ.FromNullableString(cnpj), DataString.FromNullableString(centroSap));

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("CNPJ; RAZÃO SOCIAL; EMPRESA CENTRALIZADORA; ESTADO; CENTRO SAP; EMP RECUPERANDA; EMP TRIO");

                foreach (var x in resultado.Dados)
                {
                    csv.Append($"\"{(x.CNPJ != null ? Convert.ToUInt64(x.CNPJ).ToString(@"00\.000\.000\/0000\-00") : string.Empty)}\";");
                    csv.Append($"\"{DataString.FromString(x.Nome)}\";");
                    csv.Append($"\"{DataString.FromNullableString(x.EmpresaCentralizadora?.Nome).ToString() ?? string.Empty}\";");
                    csv.Append($"\"{x.Estado.Nome}\";");
                    csv.Append($"\"{x.CodCentroSap ?? string.Empty}\";");
                    csv.Append($"\"{(!x.EmpRecuperanda.HasValue || x.EmpRecuperanda.Value == false ? "Não" : "Sim")}\";");
                    csv.Append($"\"{(String.IsNullOrEmpty(x.EmpTrio) || x.EmpTrio == "N" ? "Não" : "Sim")}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"Empresa_do_Grupo_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }

        /// <summary>
        /// obtem nomes de acordo com o cnpj informado
        /// </summary>
        [HttpGet("nomes-por-cnpj/{cnpj}")]
        public IActionResult ObterNomesPorCNPJ(
            [FromServices] IEmpresaDoGrupoRepository repository,
            [FromRoute] string cnpj)
        {
            return Result(repository.ObterNomesPorCNPJ(CNPJ.FromString(cnpj)));
        }

        /// <summary>
        /// Verifica se existe uma empresa do grupo com o mesmo nome
        /// </summary>
        [HttpGet("{nome}")]
        public IActionResult Existe(
            [FromServices] IEmpresaDoGrupoRepository repository,
            [FromRoute] string nome, [FromQuery] int? id)
        {
            return Result(repository.Existe(DataString.FromString(nome), id));
        }

        /// <summary>
        /// Cria uma nova empresa do grupo.
        /// </summary>
        /// <returns>Sem retorno</returns>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IEmpresaDoGrupoService service,
            [FromBody] CriarEmpresaDoGrupoCommand command)
        {
            return Result(service.Criar(command));
        }

        /// <summary>
        /// Cria uma nova empresa do grupo.
        /// </summary>
        /// <returns>Sem retorno</returns>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IEmpresaDoGrupoService service,
            [FromBody] AtualizarEmpresaDoGrupoCommand command)
        {
            return Result(service.Atualizar(command));
        }

        /// <summary>
        /// Deleta uma empresa do grupo caso ela atenda os critérios.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IEmpresaDoGrupoService service,
            [FromRoute] int id)
        {
            return Result(service.Remover(id));
        }
    }
}