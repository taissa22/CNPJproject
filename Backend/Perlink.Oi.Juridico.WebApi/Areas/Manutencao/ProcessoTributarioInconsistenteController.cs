using Microsoft.AspNetCore.Authorization;
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
    ///<summary>
    /// Api controller "Esfera"
    ///</summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/processotributarioinconsistente")]
    public class ProcessoTributarioInconsistenteController : ApiControllerBase
    {
        ///<summary>
        ///Lista todas os ProcessoTributarioInconsistenteController
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="pagina"></param>
        ///<param name="quantidade"></param>
        ///<param name="coluna"></param>
        ///<param name="direcao"></param>
        ///<returns></returns>
        [HttpGet]
        public IActionResult Obterpaginado(
            [FromServices] IProcessoTributarioInconsistenteRepository repository,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc")
        {
            return Result(repository.ObterPaginado(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, ProcessoTributarioInconsistenteSort.TipoProcesso), string.IsNullOrEmpty(direcao) || direcao.Equals("asc")));
        }

        ///<summary>
        /// Exclui TODOS os registros de ProcessoTributarioInconsistente
        ///</summary>
        ///<param name="service"></param>
        ///  <returns></returns>
        [HttpDelete()]
        public IActionResult Remover(
            [FromServices] IProcessoTributarioInconsistenteService service)
        {
            return Result(service.Remover());
        }

        ///<sumary>
        /// Exporta as ProcessoTributarioInconsistente
        ///</sumary>
        /// <param name="repository"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IProcessoTributarioInconsistenteRepository repository,
            [FromQuery] string coluna = "TipoProcesso",
            [FromQuery] string direcao = "asc")
        {
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, ProcessoTributarioInconsistenteSort.TipoProcesso), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"));

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Tipo de processo; Código do processo;Número do processo; Estado; Comarca / Órgão; Vara / Município; Empresa do Grupo;Escritório; Objeto / Período;Valor Total Corrigido;Valor Total Pago");
                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.TipoProcesso}\";");
                    csv.Append($"\"{(a.CodigoProcesso)}\";");
                    csv.Append($"\"{(a.NumeroProcesso)}\";");
                    csv.Append($"\"{(a.Estado)}\";");
                    csv.Append($"\"{(a.ComarcaOrgao)}\";");
                    csv.Append($"\"{(a.VaraMunicipio)}\";");
                    csv.Append($"\"{(a.EmpresadoGrupo)}\";");
                    csv.Append($"\"{(a.Escritorio)}\";");
                    csv.Append($"\"{(a.Objeto)}\";");
                    csv.Append($"\"{(a.ValorTotalCorrigido)}\";");
                    csv.Append($"\"{(a.ValorTotalPago)}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = $"ProcessosComInconsistências_{DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}