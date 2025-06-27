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
    /// Api controller ComplementoAreaEnvolvida
    /// </summary
    [Authorize]
    [ApiController]
    [Route("manutencao/complemento-area-envolvida")]
    public class ComplementoDeAreaEnvolvidaController : ApiControllerBase
    {
        /// /// <summary>
        /// Obtem lista de Complemento Area Envolvida
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="tipoProcesso"></param>
        /// <param name="pesquisa"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginado([FromServices] IComplementoDeAreaEnvolvidaRepository repository,
            [FromQuery] int tipoProcesso,
            [FromQuery] string pesquisa,
            [FromQuery] int pagina,
            [FromQuery] int quantidade = 8,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc")
        {
            return Result(repository.ObterPaginado(TipoProcesso.PorId(tipoProcesso), pesquisa, EnumHelpers.ParseOrDefault(coluna, ComplementoDeAreaEnvolvidaSort.Nome),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pagina, quantidade));
        }

        /// <summary>
        /// Cria um novo registro de Complemento Area Envolvida
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] IComplementoDeAreaEnvolvidaService service,
            [FromBody] CriarComplementoDeAreaEnvolvidaCommand dados)
        {
            return Result(service.Criar(dados));
        }

        /// <summary>
        /// Atualiza um registro de Complemento Area Envolvida
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IComplementoDeAreaEnvolvidaService service,
             [FromBody] AtualizarComplementoDeAreaEnvolvidaCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Atualiza um registro de Complemento Area Envolvida
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Remover([FromServices] IComplementoDeAreaEnvolvidaService service,
             [FromRoute] int id)
        {
            return Result(service.Remover(id));
        }

        /// <summary>
        /// Trata da exportação dos complementos de area envolvida
        /// </summary>
        [HttpGet("exportar")]
        public IActionResult Exportar([FromServices] IComplementoDeAreaEnvolvidaRepository repository,
            [FromQuery] int tipoProcesso,
            [FromQuery] string pesquisa,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc")
        {
            var resultado = repository.Obter(TipoProcesso.PorId(tipoProcesso), pesquisa, EnumHelpers.ParseOrDefault(coluna, ComplementoDeAreaEnvolvidaSort.Nome),
                    string.IsNullOrEmpty(direcao) || direcao.Equals("asc"));

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Código; Nome; Ativo; Tipo de Processo");

                foreach (var x in resultado.Dados)
                {
                    csv.Append($"\"{x.Id}\";");
                    csv.Append($"\"{x.Nome}\";");
                    csv.Append($"\"{(x.Ativo ? "Sim" : "Nâo")}\";");
                    csv.Append($"\"{TipoProcesso.PorId(tipoProcesso).Nome}\";");
                    csv.AppendLine("");
                }

                TextInfo txtInfo = new CultureInfo("pt-br", false).TextInfo;
                string tipoProcessoNome = txtInfo.ToTitleCase(TipoProcesso.PorId(tipoProcesso).Nome.WithoutAccents().Trim().ToLowerInvariant()).Replace(" ", "_");
                string nomeArquivo = $"Complemento_Area_Envolvida_{(tipoProcessoNome == "Tributario_Judicial" ? "Tributario" : tipoProcessoNome)}_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}