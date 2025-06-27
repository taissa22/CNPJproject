using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Compartilhado.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BancoController : JuridicoControllerBase
    {
        private readonly IBancoAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public BancoController(IBancoAppService appService)
        {
            this.appService = appService;
        }


        ///<summary>
        /// Recuperar Tipos processos que são utilizados no SAP
        /// </summary>
        /// <returns>Lista de TipoProcessoViewModel</returns>
        [HttpGet("ObterBancoDropDown")]
        public async Task<IResultadoApplication<ICollection<BancoViewModel>>> ObterTodosBanco()
        {
            var resultado = await appService.RecuperarNomeBanco();
            return resultado;
        }
    }

}