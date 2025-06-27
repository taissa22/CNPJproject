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
    /// EndPoint que resolve requisições das empresas centralizadoras
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/empresas-centralizadoras")]
    public class EmpresasCentralizadorasController : ApiControllerBase {

        /// <summary>
        /// Lista todas as empresas centralizadoras para dropdown
        /// </summary>
        [HttpGet]
        public IActionResult obter([FromServices] IEmpresaCentralizadoraRepository repository) {
            return Result(repository.Obter());
        }

        /// <summary>
        /// Recuperar todos as empresas centralizadoras
        /// </summary>
        /// <returns></returns>
        [HttpGet("obterPaginado")]
        public IActionResult ObterPaginado(
            [FromServices] IEmpresaCentralizadoraRepository repository,
            [FromQuery] int pagina, [FromQuery] int quantidade = 8,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc",
            [FromQuery] string nome = null, [FromQuery] int? ordem = null, [FromQuery] int? codigo = null) {
            return Result(repository.ObterPaginado(
                EnumHelpers.ParseOrDefault(coluna, EmpresaCentralizadoraSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),
                pagina, quantidade, DataString.FromNullableString(nome), ordem, codigo));
        }

        /// <summary>
        /// Exporta os assuntos do módulo cível consumidor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="nome"></param>
        /// <param name="ordem"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>      
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IEmpresaCentralizadoraRepository repository,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc",
            [FromQuery] string nome = null, [FromQuery] int? ordem = null, [FromQuery] int? codigo = null) {
            var resultado = repository.Obter(
                EnumHelpers.ParseOrDefault(coluna, EmpresaCentralizadoraSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),
                DataString.FromNullableString(nome), ordem, codigo);

            if (resultado.Tipo == ResultType.Valid) {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("ORDEM;CÓDIGO;EMPRESA CENTRALIZADORA;ESTADO;CÓDIGO CONVÊNIO;CNPJ CONVÊNIO;BANCO DÉBITO;AGÊNCIA DÉBITO;DÍGITO AGÊNCIA DÉBITO;CONTA DÉBITO;MCI;AGÊNCIA DEPOSITÁRIA;DÍGITO AGÊNCIA DEPOSITÁRIA");

                foreach (var x in resultado.Dados) { 

                    if (x.Convenios.Count != 0)
                    {
                        foreach(var convenio in x.Convenios)
                        {
                            csv.Append($"\"{ x.Ordem }\";");
                            csv.Append($"\"{ x.Codigo }\";");
                            csv.Append($"\"{ x.Nome }\";");
                            csv.Append($"\"{convenio.Estado.Id}\";");
                            csv.Append($"\"{convenio.Codigo}\";");
                            csv.Append($"\"{CNPJ.FormatCNPJ(convenio.CNPJ)}\";");
                            csv.Append($"\"{convenio.BancoDebito}\";");
                            csv.Append($"\"{convenio.AgenciaDebito}\";");
                            csv.Append($"\"{convenio.DigitoAgenciaDebito}\";");
                            csv.Append($"\"{convenio.ContaDebito}\";");
                            csv.Append($"\"{convenio.MCI}\";");
                            csv.Append($"\"{convenio.AgenciaDepositaria}\";");
                            csv.Append($"\"{convenio.DigitoAgenciaDebito}\";");
                            csv.AppendLine("");
                        } 
                    }
                    else
                    {
                        csv.Append($"\"{ x.Ordem }\";");
                        csv.Append($"\"{ x.Codigo }\";");
                        csv.Append($"\"{ x.Nome }\";");
                        csv.AppendLine("");
                    }

                  
                }
                string nomeArquivo = $"Empresa_Centralizadora_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }

        /// <summary>
        /// Verifica se existe uma empresa centralizadora com o mesmo nome
        /// </summary>
        [HttpGet("existe/{nome}")]
        public IActionResult Existe(
            [FromServices] IEmpresaCentralizadoraRepository repository,
            [FromRoute] string nome, [FromQuery] int? codigo) {
            return Result(repository.Existe(DataString.FromString(nome), codigo));
        }

        /// <summary>
        /// Insere uma nova empresa centralizadora
        /// <param name="service"></param>
        /// <param name="command"></param>
        /// </summary>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IEmpresaCentralizadoraService service,
            [FromBody] CriarEmpresaCentralizadoraCommand command) {
            return Result(service.Criar(command));
        }

        /// <summary>
        /// Atualiza uma empresa centralizadora
        /// <param name="service"></param>
        /// <param name="command"></param>
        /// </summary>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IEmpresaCentralizadoraService service,
            [FromBody] AtualizarEmpresaCentralizadoraCommand command) {
            return Result(service.Atualizar(command));
        }

        /// <summary>
        /// Exclui uma empresa centralizadora
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IEmpresaCentralizadoraService service,
            [FromRoute] int id) {
            return Result(service.Remover(id));
        }
    }
}