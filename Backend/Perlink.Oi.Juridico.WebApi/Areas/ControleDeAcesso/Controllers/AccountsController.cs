using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel;
using Perlink.Oi.Juridico.WebApi.Helpers;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.CivelEstrategico.WebApi.Areas.CivelEstrategico.Controllers
{
    /// <summary>
    /// Api Account Resposável pelo controle de acessos.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : JuridicoControllerBase
    {
        private readonly IAutenticacaoAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public AccountsController(IAutenticacaoAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Metodo Teste Login
        /// </summary>
        /// <returns>Teste</returns>
        [HttpPost, AllowAnonymous, Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            var response = await appService.Autenticar(viewModel);
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        /// <summary>
        /// Obtém os dados do usuário logado
        /// </summary>
        /// <returns>Dados do usuário logado</returns>
        [HttpGet, Route("User")]
        public async Task<IActionResult> Get()
        {
            var response = await appService.ObterDadosUsuarioLogado();
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }
    }
}