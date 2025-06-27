using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
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
    [Route("manutencao/esfera")]
    public class EsferaController : ApiControllerBase
    {
        ///<summary>
        ///Lista todas as esferas
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="pagina"></param>
        ///<param name="quantidade"></param>
        ///<param name="coluna"></param>
        ///<param name="direcao"></param>        
        ///<returns></returns>
        [HttpGet]
        public IActionResult Obterpaginado(
            [FromServices] IEsferaRepository repository,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc")
        {
            return Result(repository.ObterPaginado(pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, EsferasSort.Id), string.IsNullOrEmpty(direcao) || direcao.Equals("asc")));
        }

        //        ///<summary>
        //        /// verifica se a esfera está sendo utilizada em algum lugar
        //        ///</summary>
        //        ///<param name = "repository"></param>
        //        ///<param name = "codEsfera"></param>
        //        ///<returns></returns>
        //        [HttpGet] ("utilizado-em-esfera/{codEsfera}")]

        //        public IActionResult UtilizadoEm(
        //            [FromServices] IEsferaRepository repository,
        //            [FromRoute] int codEsfera)
        //        {
        //            return Result(repository.Utilizadoem...(codEsfera));
        //        }

        ///<sumary>
        /// Cria registro de nova esfera
        ///</sumary>

        ///<param name="dados"></param>
        ///<returns></returns>
        [HttpPost]
        public IActionResult Criar([FromServices] IEsferaService service, [FromBody] CriarEsferaCommand dados)
        {
            return Result(service.Criar(dados));
        }

        ///<summary>
        /// Atualiza registro de esfera
        ///</summary>
        ///<param name="service"></param>
        ///<param name="dados"></param>
        ///<returns></returns>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IEsferaService service,
            [FromBody] AtualizarEsferaCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        ///<summary>
        /// Exclui registro de esfera
        ///</summary>
        ///<param name="service"></param>
        ///  ///<param name="codigoEsfera"></param>
        ///  <returns></returns>
        [HttpDelete("{codigoEsfera}")]
        public IActionResult Remover(
            [FromServices] IEsferaService service,
            [FromRoute] int codigoEsfera)
        {
            return Result(service.Remover(codigoEsfera));
        }

        ///<sumary>
        /// Exporta as esferas
        ///</sumary>
        /// <param name="repository"></param>        
        /// <param name="repositoryIndiceCorrecao"></param> 
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IEsferaRepository repository,
            [FromServices] IIndiceCorrecaoEsferaRepository repositoryIndiceCorrecao,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc")
        {
            var resultado = repository.Obter(EnumHelpers.ParseOrDefault(coluna, EsferasSort.Nome),string.IsNullOrEmpty(direcao) || direcao.Equals("asc"));


            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Código;Nome da Esfera;Corrige Principal;Corrige Multas;Corrige Juros;Índice de Correção da Esfera;Data de Vigência") ;

                foreach (var a in resultado.Dados)
                {
                    var resultadoIndiceCorrecao = repositoryIndiceCorrecao.Obter(a.Id, IndiceCorrecaoEsferaSort.EsferaId, true);

                    if (resultadoIndiceCorrecao.Dados.Count == 0)
                    {
                        csv.Append($"\"{a.Id}\";");
                        csv.Append($"\"{(a.Nome)}\";");
                        csv.Append($"\"{(a.CorrigePrincipal ? "Sim" : "Não") }\";");
                        csv.Append($"\"{(a.CorrigeMultas ? "Sim" : "Não") }\";");
                        csv.Append($"\"{(a.CorrigeJuros ? "Sim" : "Não") }\";");
                        csv.Append($"\"{string.Empty}\";");
                        csv.Append($"\"{string.Empty}\";");
                        csv.AppendLine("");

                        continue;
                    }

                    foreach (var b in resultadoIndiceCorrecao.Dados)
                    {
                        csv.Append($"\"{a.Id}\";");
                        csv.Append($"\"{(a.Nome)}\";");
                        csv.Append($"\"{(a.CorrigePrincipal ? "Sim" : "Não") }\";");
                        csv.Append($"\"{(a.CorrigeMultas ? "Sim" : "Não") }\";");
                        csv.Append($"\"{(a.CorrigeJuros ? "Sim" : "Não") }\";");
                        csv.Append($"\"{b.Indice.Descricao }\";");
                        csv.Append($"\"{b.DataVigencia.ToString("dd/MM/yyyy") }\";");
                        csv.AppendLine("");
                    }                    
                }

                string nomeArquivo = $"Esfera_{DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}





