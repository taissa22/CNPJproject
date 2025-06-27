using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Extensions;
using System;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    ///<summary>
    /// Api controller "Esfera"
    ///</summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/indice-correcao-esfera")]
    public class IndiceCorrecaoEsferaController : ApiControllerBase
    {
        ///<summary>
        ///Lista todas as esferas
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="esferaId"></param>
        ///<param name="pagina"></param>
        ///<param name="quantidade"></param>
        ///<param name="coluna"></param>
        ///<param name="direcao"></param>        
        ///<returns></returns>
        [HttpGet]
        public IActionResult Obterpaginado(
            [FromServices] IIndiceCorrecaoEsferaRepository repository,
            [FromQuery] int esferaId,
            [FromQuery] int pagina,
            [FromQuery] int quantidade,
            [FromQuery] string coluna = "nome",
            [FromQuery] string direcao = "asc")
        {
            return Result(repository.ObterPaginado(esferaId, pagina, quantidade, EnumHelpers.ParseOrDefault(coluna, IndiceCorrecaoEsferaSort.DataVigencia), string.IsNullOrEmpty(direcao) || direcao.Equals("asc")));
        }


        /// <summary>
        /// Cria uma Indice Correcao Esfera
        /// </summary>
        [HttpPost]
        public IActionResult Criar(
            [FromServices] IIndiceCorrecaoEsferaService service,
            [FromBody] CriarIndiceCorrecaoEsferaCommand command)
        {
            return Result(service.Criar(command));
        }

        /// <summary>
        /// Exclui registro de Indice Correcao Esfera
        /// </summary>
        /// <param name="service"></param>
        /// <param name="codigoEsfera"></param>
        /// /// <param name="dataVigencia"></param>        
        /// <param name="codigoIndice"></param>
        /// <returns></returns>
        [HttpDelete("{codigoEsfera}/{dataVigencia}/{codigoIndice}")]
        public IActionResult Remover(
            [FromServices] IIndiceCorrecaoEsferaService service,
            [FromRoute] int codigoEsfera,
            [FromRoute] DateTime dataVigencia,
            [FromRoute] int codigoIndice)
        {
            return Result(service.Remover(codigoEsfera, dataVigencia,codigoIndice));
        }


    }
}





