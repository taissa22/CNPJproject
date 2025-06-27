using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.WebApi.Helpers;

namespace Perlink.Oi.Juridico.WebApi.Areas.Compartilhado.Controllers
{
    /// <summary>
    /// Responsavel por obter o menu do sisjur.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MenuSisjurController : JuridicoControllerBase {
        ///<summary>
        /// Recupera o menu do sisjur
        /// </summary>
        /// <returns>xml</returns>
        [HttpGet("ObterBancoDropDown")]
        public JsonResult ObterMenuSisjur() {
            return null;
        }
    }
}