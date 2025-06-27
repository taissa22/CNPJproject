using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Commands.Dto;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao {

    /// <summary>
    /// Controller de profissionais
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [ApiController]
    [Route("manutencao/profissionais")]
    public class ProfissionaisController : ApiControllerBase {

        /// <summary>
        /// Lista todos os profissionais.
        /// </summary>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IProfissionalRepository repository,
            [FromQuery] int pagina, [FromQuery] int quantidade = 8,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc",
            [FromQuery] string nome = null, [FromQuery] string documento = null,
            [FromQuery] string tipoPessoa = null, [FromQuery] bool? advogadoDoAutor = null) {
            var resultado = repository.ObterPaginado(
                EnumHelpers.ParseOrDefault(coluna, ProfissionalSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),
                pagina, quantidade, TipoPessoa.PorValor(tipoPessoa), nome, documento, advogadoDoAutor);

            return Result(resultado);
        }

        /// <summary>
        /// Consulta se profissional existe pelo nome
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("existe")]
        public IActionResult Existe(
            [FromServices] IProfissionalRepository repository,
            [FromBody] NomeProfissionalDto command) {
            return Result(repository.Existe(DataString.FromString(command.Nome), command.Id));
        }

        /// <summary>
        /// Cria um profissional
        /// </summary>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IProfissionalService service,           
            [FromBody] CriarProfissionalCommand command) {
          
            return Result(service.Criar(command));
        }

        /// <summary>
        /// Atualiza um profissional
        /// </summary>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IProfissionalService service,
            [FromBody] AtualizarProfissionalCommand command) {
            return Result(service.Atualizar(command));
        }

        /// <summary>
        /// Deleta o profissional
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IProfissionalService service,
            [FromRoute] int id) {
            return Result(service.Remover(id));
        }

        /// <summary>
        /// Lista todos os profissionais.
        /// </summary>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IProfissionalRepository repository,
            [FromQuery] string coluna = "nome", [FromQuery] string direcao = "asc",
            [FromQuery] string nome = null, [FromQuery] string documento = null,
            [FromQuery] string tipoPessoa = null, [FromQuery] bool? advogadoAutor = null) {
            var resultado = repository.Obter(
                EnumHelpers.ParseOrDefault(coluna, ProfissionalSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),
                TipoPessoa.PorValor(tipoPessoa), DataString.FromNullableString(nome), DataString.FromNullableString(documento), advogadoAutor);

            if (resultado.Tipo == ResultType.Valid) {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("NOME;CPF/CNPJ;ESTADO;DDD;TELEFONE;ADVOGADO DO AUTOR;REGISTRO OAB;ESTADO OAB");

                foreach (var a in resultado.Dados) {
                    string documentoConvertido = string.Empty;

                    if (!string.IsNullOrEmpty(a.CPF)) {
                        if (CPF.IsValidForSisjur(a.CPF)) {
                            documentoConvertido = Convert.ToUInt64(a.CPF).ToString(@"\000\.000\.000\-00\");
                        }
                    } else if (!string.IsNullOrEmpty(a.CNPJ)) {
                        if (CNPJ.IsValidForSisjur(a.CNPJ)) {
                            documentoConvertido = Convert.ToUInt64(a.CNPJ).ToString(@"\00\.000\.000\/0000\-00\");
                        }
                    }

                    csv.Append($"\"{(!string.IsNullOrEmpty(a.Nome) ? a.Nome : string.Empty)}\";");
                    csv.Append($"\"{documentoConvertido}\";");
                    csv.Append($"\"{(a.Estado != null ? a.Estado.Id : string.Empty)}\";");
                    csv.Append($"\"{(!string.IsNullOrEmpty(a.TelefoneDDD) ? Convert.ToUInt64(a.TelefoneDDD).ToString() : string.Empty)}\";");

                    //Se tiver fora do padrão, vai dar erro ao tentar formatar.
                    try
                    {
                        csv.Append($"\"{(!string.IsNullOrEmpty(a.Telefone) ? Convert.ToUInt64(a.Telefone).ToString(@"0000-0000") : string.Empty)}\";");
                    }
                    catch (Exception)
                    {
                        csv.Append($"\"{(!string.IsNullOrEmpty(a.Telefone) ? a.Telefone : string.Empty)}\";");
                    }
                    
                    csv.Append($"\"{(a.EhAdvogado ? "Sim" : "Não")}\";");
                    csv.Append($"\"{(!string.IsNullOrEmpty(a.RegistroOAB) ? a.RegistroOAB : string.Empty)}\";");
                    csv.Append($"\"{(a.EstadoOAB != null ? a.EstadoOAB.Id : string.Empty)}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"Profissional_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}