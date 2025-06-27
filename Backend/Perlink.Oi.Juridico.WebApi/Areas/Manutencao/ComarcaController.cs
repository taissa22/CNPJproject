using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.WebApi.Areas;
using System;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Comarca.Manutencao
{
    /// <summary>
    /// Api controller Indice
    /// </summary>
    ///
    [AllowAnonymous]
    [Authorize]
    [ApiController]
    [Route("manutencao/comarca")]
    public class ComarcaController : ApiControllerBase
    {
        /// <summary>
        /// Lista todas as Comarcas
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <param name="codigoEstado"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <param name="pesquisa"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterPaginado(
            [FromServices] IComarcaRepository repository,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string codigoEstado,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc",
            [FromQuery] string pesquisa = null)
        {
            return Result(repository.ObterPaginado(pagina, quantidade, codigoEstado, EnumHelpers.ParseOrDefault(coluna, ComarcaSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa));
        }

        /// <summary>
        /// Cria um novo registro de Comarca
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] IComarcaService service,
            [FromBody] CriarComarcaCommand dados)
        {
            return Result(service.Criar(dados));
        }

        /// <summary>
        /// Atualiza registro de comarca
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar([FromServices] IComarcaService service,
            [FromBody] AtualizarComarcaCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        /// <summary>
        /// Exclui registro de comarca
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Remover(
            [FromServices] IComarcaService service,
            [FromRoute] int id)
        {
            return Result(service.Remover(id));
        }

        /// <summary>
        /// Exporta os Tipos de vara
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="repositoryVara"></param>
        /// /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// /// <param name="estadoId"></param>
        /// <param name="pesquisa"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IComarcaRepository repository,
            [FromServices] IVaraRepository repositoryVara,
            [FromQuery] string coluna = "descricao",
            [FromQuery] string direcao = "asc",
            [FromQuery] string estadoId = null,
            [FromQuery] string pesquisa = null)

        {
            var resultado = repository.Obter(estadoId, EnumHelpers.ParseOrDefault(coluna, ComarcaSort.Nome), string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), pesquisa);

            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Estado;Nome;Comarca BB;Código Vara;Tipo Vara;Endereço;Tribunal de Justiça BB;Vara BB");
                
                foreach (var a in resultado.Dados)
                {
                    var Varas = repositoryVara.ObterPorComarca(a.Id).Dados.Data;

                    if (Varas.Any())
                    {
                        foreach (var x in Varas)
                        {
                            csv.Append($"\"{a.Estado.Id}\";");
                            csv.Append($"\"{(a.Nome)}\";");
                            if (a.ComarcaBB != null) csv.Append($"\"{(a.ComarcaBB?.EstadoId)} - {(a.ComarcaBB?.Nome)} ({(a.ComarcaBB?.Id.ToString().PadLeft(9, '0'))} ) \";");
                            else csv.Append(";");
                            csv.Append($"\"{(x.VaraId)}\";");
                            csv.Append($"\"{(x.TipoVara.Nome)}\";");
                            csv.Append($"\"{(x.Endereco)}\";");
                            if (x.OrgaoBB != null) csv.Append($"\"{(x.OrgaoBB.TribunalBB.Nome)}\";");
                            else csv.Append(";");
                            if (x.OrgaoBB != null) csv.Append($"\"{(x.OrgaoBB?.Nome)}\";");
                            else csv.Append(";");
                            csv.AppendLine("");
                        }
                    }
                    else
                    {
                        csv.Append($"\"{a.Estado.Id}\";");
                        csv.Append($"\"{(a.Nome)}\";");
                        if (a.ComarcaBB != null) csv.Append($"\"{(a.ComarcaBB?.EstadoId)} - {(a.ComarcaBB?.Nome)} ({(a.ComarcaBB?.Id.ToString().PadLeft(9, '0'))} ) \";");
                        else csv.Append(";");
                        csv.AppendLine("");
                    }

                }

                string nomeArquivo = $"Comarca_Vara_{ DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}
