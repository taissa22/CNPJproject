using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.CivelEstrategico.WebApi.Areas.Compartilhado.Controllers
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProcessoController : JuridicoControllerBase
    {
        private readonly IProcessoAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public ProcessoController(IProcessoAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Recupera um Lote por Processo.
        /// </summary>
        /// <returns>Lote correspondente ao código.</returns>
        [HttpGet("recuperarProcessoPeloCodigoTipoProcesso")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IResultadoApplication<ICollection<ProcessoFiltroViewModel>>> recuperarProcessoPeloCodigoTipoProcesso(string numeroProcesso, long codigoTipoProcesso, string rota="defaultValue")
        {
            var resultado = await appService.RecuperarProcessoPeloCodigoTipoProcesso(numeroProcesso, codigoTipoProcesso, rota);
            return resultado;
        }

        [HttpGet("recuperarProcessoPeloCodigoInterno")]
        public async Task<IResultadoApplication<ICollection<ProcessoFiltroViewModel>>> recuperarProcessoPeloCodigoInterno(long codigoInterno, long codigoTipoProcesso, string rota = "defaultValue")
        {
            var resultado = await appService.RecuperarProcessoPeloCodigoInterno(codigoInterno, codigoTipoProcesso, rota);
            return resultado;
        }
    }
}