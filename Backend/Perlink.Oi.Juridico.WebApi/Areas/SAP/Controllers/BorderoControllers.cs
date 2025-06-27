using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.CivelEstrategico.WebApi.Areas.SAP.Controllers
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]
   
    public class BorderoController : JuridicoControllerBase
    {
        private readonly IBorderoAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public BorderoController(IBorderoAppService appService)
        {
            this.appService = appService;
        }


        /// <summary>
        /// Recuperar Todos os parametros
        /// </summary>
        /// <returns>Lista de parametros</returns>
        [HttpGet("ObterBordero")]
        public async Task<IResultadoApplication<ICollection<BorderoViewModel>>> GetBordero(long CodigoLote)
        {
            return await appService.GetBordero(CodigoLote);

        }

        /// <summary>
        /// Recuperar Todos os parametros de um borderô e retorna um CSV
        /// </summary>
        /// <returns>Arquivo CSV em base64</returns>
        [HttpGet("ExportarBorderoDoLote")]
        public async Task<IResultadoApplication<byte[]>> ExportarBorderoDoLote(long codigoLote, long codigoTipoProcesso) {
            var result = await appService.ExportarBorderoDoLote(codigoLote, codigoTipoProcesso);
            return result;
        }
    }
}
