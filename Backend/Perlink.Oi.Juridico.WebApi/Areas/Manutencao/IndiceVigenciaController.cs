using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Api controller Indices Vigências Civeis
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("manutencao/indice-vigencia")]
    public class IndiceVigenciaController : ApiControllerBase
    {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="pagina"></param>
    /// <param name="quantidade"></param>
    /// <param name="datainicio"></param>
    /// <param name="datafim"></param>
    /// <param name="tipoprocesso"></param>
    /// <param name="coluna"></param>
    /// <param name="direcao"></param>
    /// <param name="vigencia"></param>
    /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginado(
             [FromServices] IIndicesVigenciasRepository repository,
             [FromQuery] int pagina,
             [FromQuery] int quantidade,
             [FromQuery] string datainicio,
             [FromQuery] string datafim,
             [FromQuery] int tipoprocesso,
             [FromQuery] string coluna = "nome",
             [FromQuery] string direcao = "asc",
             [FromQuery] int vigencia = 0

            )
        {
            if (quantidade == 0)
            {
                return Result(repository.ObterTodos());
            }
            if (datainicio.Contains("Invalid"))
            {
                return Result(repository.ObterPaginado(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, IndicesVigenciaSort.Indice), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"),tipoprocesso, null, vigencia));

            }

            return Result(repository.ObterPaginado(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, IndicesVigenciaSort.Indice), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), Convert.ToDateTime(datainicio), Convert.ToDateTime(datafim), tipoprocesso,null, vigencia));
        }


      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        [HttpGet("obtertodos")]
        public IActionResult ObterTodos(
          [FromServices] IIndicesVigenciasRepository repository)
        {
            return Result(repository.ObterTodos());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] IIndicesVigenciasService service,
          [FromBody] CriarIndiceVigenciaCommand dados)
        {
            return Result(service.Criar(dados));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="tipoprocesso"></param>
        /// <returns></returns>
        [HttpGet("obterindices/{tipoprocesso}")]
        public IActionResult ObterIndices(
          [FromServices] IIndicesVigenciasRepository repository,
          [FromRoute] int tipoprocesso
            )
        {
            return Result(repository.ObterInces(tipoprocesso));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="datainicio"></param>
        /// <param name="datafim"></param>
        /// <param name="tipoprocesso"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="vigencia"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IIndicesVigenciasRepository repository,
             [FromQuery] int pagina,
             [FromQuery] int quantidade,
             [FromQuery] string datainicio,
             [FromQuery] string datafim,
             [FromQuery] int tipoprocesso,
             [FromQuery] string coluna = "nome",
             [FromQuery] string direcao = "asc",
             [FromQuery] int vigencia = 0
            )
        {

            var resultado = datainicio.Contains("Invalid") ?
                            repository.ObterBase(EnumHelpers.ParseOrDefault(coluna, IndicesVigenciaSort.Indice), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), tipoprocesso, null, vigencia)
                            :
                            repository.ObterBase(EnumHelpers.ParseOrDefault(coluna, IndicesVigenciaSort.Indice), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), Convert.ToDateTime(datainicio), Convert.ToDateTime(datafim), tipoprocesso, null, vigencia);


            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Início de Vigência;Índice de Correção");

                foreach (var a in resultado.Dados)
                {
                    csv.Append($"\"{a.DataVigencia.ToShortDateString()}\";");
                    csv.Append($"\"{(a.Indice.Descricao)}\";");
                    csv.AppendLine("");
                }

                string nomeArquivo = string.Empty;

                if (tipoprocesso == 1)
                {
                   nomeArquivo = $"IndivcesVigencias_CivelConsumidor_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                }
                else
                {
                    nomeArquivo = $"IndivcesVigencias_CivelEstrategico_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                }

                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }


     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="codigoIndice"></param>
        /// <param name="dataVigencia"></param>
        /// <returns></returns>
        [HttpDelete("{codigoIndice}/{dataVigencia}")]
        public IActionResult Remover(
            [FromServices] IIndicesVigenciasService service,
            [FromRoute] int codigoIndice,
            [FromRoute] DateTime dataVigencia
            )
        {
            return Result(service.Remover(codigoIndice, dataVigencia));
        }
    }
}
