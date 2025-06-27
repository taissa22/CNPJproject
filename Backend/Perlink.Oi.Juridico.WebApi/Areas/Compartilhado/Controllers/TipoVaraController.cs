using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.VariosContextos;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Compartilhado.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TipoVaraController : JuridicoControllerBase
    {
        private readonly ITipoVaraAppService appService;
        public TipoVaraController(ITipoVaraAppService appService)
        {
            this.appService = appService;
        }
        [HttpGet("RecuperarPorVaraEComarca")]
        public async Task<IResultadoApplication<IEnumerable<ComboboxViewModel<long>>>> RecuperarPorVaraEComarca(long codigoComarca, long codigoVara)
        {
            return await appService.RecuperarPorVaraEComarca(codigoComarca, codigoVara);
        }
    }
}