
// Migrado para ApiV2

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using System;

namespace Perlink.Oi.Juridico.WebApi.Areas.Manutencao
{
    /// <summary>
    /// Api controller EscritorioEstadoController
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("manutencao/EscritorioEstado")]
    [Obsolete("Usar a ApiV2", true)]
    public class EscritorioEstadoController : ApiControllerBase
    {
        /// <summary>
        /// Lista todos os EscritorioEstados
        /// </summary>
        /// <param name="escritorioId"></param>
        /// <param name="tipoProcessoId"></param>
        /// <param name="repository"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObterTodos([FromQuery] int escritorioId,
                                        [FromQuery] int tipoProcessoId,
                                        [FromServices] IEscritorioEstadoRepository repository)
        {
            return Result(repository.Obter(escritorioId, tipoProcessoId));
        }

    }
}