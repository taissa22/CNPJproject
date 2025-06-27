using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Shared.Application.Interface;

namespace Perlink.Oi.Juridico.WebApi.Areas.Compartilhado.Controllers
{
    /// <summary>
    /// Controle responsável por gerenciar as requisições ao recurso Estados.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EstadoController : ControllerBase
    {
        private readonly IEstadoAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public EstadoController(IEstadoAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Recuperar Todos os estados
        /// </summary>
        /// <returns>Lista de Estados</returns>
        [HttpGet("RecuperarEstados")]
        public async Task<IResultadoApplication<IEnumerable<EstadoDTO>>> RecuperarEstados()
        {
            var resultado = await appService.RecuperarEstados();
            return resultado;
        }
    }
}