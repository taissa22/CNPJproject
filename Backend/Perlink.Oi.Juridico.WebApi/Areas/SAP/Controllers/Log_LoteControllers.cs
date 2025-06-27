using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;

using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.SAP.Controllers
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
        [ApiController]
       
        public class Log_LoteController : JuridicoControllerBase
    {
            private readonly ILog_LoteAppService appService;

            /// <summary>
            /// Injeção das dependencias da Api.
            /// </summary>
            /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
            public Log_LoteController(ILog_LoteAppService appService)
            {
                this.appService = appService;
            }


            /// <summary>
            /// Recuperar Todos os parametros
            /// </summary>
            /// <returns>Lista de parametros</returns>
            [HttpGet("ObterHistorico")]
        public async Task<IResultadoApplication<ICollection<Log_LoteHistoricoViewModel>>> ObterHistorico(long CodigoLote)
        {
            return await appService.ObterHistorico(CodigoLote);
        }

    }
}
