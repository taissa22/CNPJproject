using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// EndPoint que resolve requisições de % ATM
    /// </summary>
    [Route("manutencao/percentual-atm")]
    [Authorize]
    [ApiController]
    public class PercentualATMController : ApiControllerBase
    {
        /// <summary>
        /// Lista as vigências
        /// </summary>
        [HttpGet]
        public IActionResult ObterPaginadoPercentualATM([FromServices] IPercentualATMRepository repository, [FromQuery] DateTime dataVigencia, [FromQuery] int codTipoProcesso, [FromQuery] int pagina = 1,
            [FromQuery] int quantidade = 8, [FromQuery] string coluna = "uf", [FromQuery] string direcao = "asc")

        {
            return Result(repository.ObterPaginadoPercentualATM(pagina, quantidade,
                EnumHelpers.ParseOrDefault(coluna, PercentualATMSort.Uf),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), dataVigencia, codTipoProcesso));
        }

        /// <summary>
        /// Importa os percentuais para a vigência
        /// </summary>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public IActionResult Importar([FromForm] IFormFile arquivo, [FromForm] DateTime dataVigencia, [FromQuery] int codTipoProcesso, [FromServices] IPercentualAtmService service)
        {
            return Result(service.AtualizarPercentualAtmCC(arquivo, dataVigencia.Date, codTipoProcesso));
        }

        /// <summary>
        /// Valida se a vigência já existe
        /// </summary>
        [HttpGet("existeVigencia")]
        public IActionResult ExisteVigencia([FromServices] IPercentualATMRepository repository, [FromQuery] DateTime dataVigencia, [FromQuery] int codTipoProcesso)
        {
            return Result(repository.ExistePercentualParaVigencia(dataVigencia, codTipoProcesso));
        }

        /// <summary>
        /// Busca Combobox 
        /// </summary>
        [HttpGet("ObterComboVigencias")]
        public IActionResult ObterComboVigencias([FromServices] IPercentualATMRepository repository,[FromQuery] int codTipoProcesso)
        {
            return Result(repository.ObterComboVigencias(codTipoProcesso));
        }

        /// <summary>
        /// Exporta os assuntos do módulo cível consumidor
        /// </summary>
        [HttpGet("exportar")]
        public IActionResult Exportar([FromServices] IPercentualATMRepository repository, [FromQuery] DateTime dataVigencia, 
            [FromQuery] int codTipoProcesso, [FromQuery] string nomeTipoProcesso, [FromQuery] string coluna = "uf", [FromQuery] string direcao="desc")
        {
            if(direcao == "undefined")
            {
                direcao = "asc";
            }
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, PercentualATMSort.Uf),
                string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), dataVigencia,codTipoProcesso);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("UF;% ATM");

                foreach (var x in resultado.Dados)
                {
                    csv.Append($"\"{x.EstadoId}\";");
                    csv.Append($"\"{x.Percentual}\";");

                    csv.AppendLine("");
                }

                string nomeArquivo = $"PercentualATM_{nomeTipoProcesso}_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }

       

    }
}