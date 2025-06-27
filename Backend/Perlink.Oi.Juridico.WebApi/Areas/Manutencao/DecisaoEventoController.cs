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

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    ///<summary>
    /// Api controller "Decisão Evento"
    ///</summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/decisaoevento")]
    public class DecisaoEventoController : ApiControllerBase
    {
        ///<summary>
        ///Lista todas as Decisão Evento
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="eventoId"></param>
        ///<param name="pagina"></param>
        ///<param name="quantidade"></param>
        ///<param name="coluna"></param>
        ///<param name="direcao"></param>      
        ///<returns></returns>
        [HttpGet]
        public IActionResult Obterpaginado(
            [FromServices] IDecisaoEventoRepository repository,
            [FromQuery] int eventoId,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "id",
            [FromQuery] string direcao = "asc")
        {
            return Result(repository.ObterPaginado(eventoId, pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, DecisaoEventoSort.Id), string.IsNullOrEmpty(direcao) || direcao.Equals("asc")));
        }


        ///<sumary>
        /// Cria registro de nova Decisão Evento
        ///</sumary>

        ///<param name="dados"></param>
        ///<returns></returns>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IDecisaoEventoService service,
            [FromBody] CriarDecisaoEventoCommand dados)
        {
            return Result(service.Criar(dados));
        }

        ///<summary>
        /// Atualiza registro de Decisao Evento
        ///</summary>
        ///<param name="service"></param>
        ///<param name="dados"></param>
        ///<returns></returns>
        [HttpPut]
        public IActionResult Atualizar(
            [FromServices] IDecisaoEventoService service,
            [FromBody] AtualizarDecisaoEventoCommand dados)
        {
            return Result(service.Atualizar(dados));
        }

        ///<summary>
        /// Exclui registro de Decisão Evento
        ///</summary>
        ///<param name="service"></param>
        ///<param name="decisaoEvento"></param>        
        ///<param name="eventoId"></param>
        ///  <returns></returns>
        [HttpDelete("{decisaoEvento}/{eventoId}")]
        public IActionResult Remover(
            [FromServices] IDecisaoEventoService service,
            [FromRoute] int decisaoEvento,
            [FromRoute] int eventoId)
        {
            return Result(service.Remover(decisaoEvento,eventoId));
        }

        ///<sumary>
        /// Exporta as Evento
        ///</sumary>
        /// <param name="repository"></param>        
        /// <param name="eventoid"></param>
        /// <param name="coluna"></param>
        /// <param name="direcao"></param>
        /// <returns></returns>
        [HttpGet("exportar")]
        public IActionResult Exportar(
            [FromServices] IDecisaoEventoRepository repository,
            [FromQuery] int eventoid,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc")
        {
            var resultado = repository.Obter(eventoid, EnumHelpers.ParseOrDefault(coluna, DecisaoEventoSort.Descricao),string.IsNullOrEmpty(direcao) || direcao.Equals("asc"));


            if (resultado.Tipo == ResultType.Valid)
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine($"Código;Nome da Esfera;Corrige Principal;Corrige Multas;Corrige Juros;Índice de Correção da Esfera;Data de Vigência") ;

                foreach (var a in resultado.Dados)
                {
                    
                    csv.Append($"\"{a.Id}\";");
                    csv.Append($"\"{(a.Descricao)}\";");
                    //csv.Append($"\"{(a.CorrigePrincipal ? "Sim" : "Não") }\";");
                    //csv.Append($"\"{(a.CorrigeMultas ? "Sim" : "Não") }\";");
                    //csv.Append($"\"{(a.CorrigeJuros ? "Sim" : "Não") }\";");
                    //csv.Append($"\"{b.Indice.Descricao }\";");
                    //csv.Append($"\"{b.DataVigencia.ToString("dd/MM/yyyy") }\";");
                    csv.AppendLine("");
                                        
                }

                string nomeArquivo = $"Evento_{DateTime.Now.ToString("yyyyMMdd_HHmmss") }.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }

            return Result(resultado);
        }
    }
}





