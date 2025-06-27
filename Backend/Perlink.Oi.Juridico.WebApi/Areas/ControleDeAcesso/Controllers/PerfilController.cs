using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO;
using Perlink.Oi.Juridico.WebApi.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Perlink.Oi.Juridico.WebApi.Areas.ControleDeAcesso.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class PerfilController : JuridicoControllerBase
    {
        private readonly IPerfilAppService appService;
        /// <summary>
        /// Construtor para injeção de dependência
        /// </summary>
        /// <param name="appService"></param>
        public PerfilController(IPerfilAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Obtém as informações do perfil
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ObterDetalhePerfil()
        {
            async Task<string> GetBodyAsync()
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }

            var codigoPerfil = await GetBodyAsync();
            PerfilDTO perfilDTO;

            if (codigoPerfil.Equals("createPerfilPath"))
            {
                perfilDTO = await appService.Criar();
            }
            else
            {
                perfilDTO = await appService.ObterDetalhePerfil(codigoPerfil);
            }


            if (perfilDTO == null)
            {
                return BadRequest();
            }

            return Ok(perfilDTO);
        }

        /// <summary>
        /// Obtém Gestores ativos  
        /// </summary>
        /// <returns></returns>
        [HttpGet("gestor")]
        public async Task<IEnumerable<KeyValuePair<string, string>>> ObterListaGestoresDefault()
        {
            var retorno = await appService.ListarGestoresDefault();
            var jsonGestor = retorno.Select(x => new KeyValuePair<string, string>(x.NomeId, x.Descricao));

            return jsonGestor;
        }

        /// <summary>
        /// Salva as modificações no perfil
        /// </summary>
        /// <param name="perfil">DTO com as informações para persistentencia</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Atualizar(PerfilDTO perfil)
        {
            return Ok(appService.Atualizar(perfil));
        }

        /// <summary>
        /// Exporta as informações do perfil
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ehPermissao"></param>
        /// <returns></returns>
        [HttpGet, Route("exportar")]
        public async Task<IActionResult> Exportar([FromQuery] string id, bool ehPermissao)
        {
            var bytes = await appService.Exportar(id, ehPermissao);
            var nome = ehPermissao ? "permissao" : "usuario";
            string nomeArquivo = String.Format("relatorio_juridico_{0}_perfil_{1}_{2}h{3}m{4}s_{5}{6}",
                              nome, DateTime.Now.ToString("yyyy_MM_dd"), DateTime.Now.Hour, DateTime.Now.Minute,
                              DateTime.Now.Second, String.Format("{0}{1}{2}{3}", new Random(DateTime.Now.Millisecond).Next(1, 9), new Random(DateTime.Now.Millisecond).Next(1, 9), new Random(DateTime.Now.Millisecond).Next(1, 9),
                              new Random(DateTime.Now.Millisecond).Next(1, 9)), ".csv");

            return File(bytes, "text/csv", nomeArquivo);
        }

        [HttpGet, Route("Criar"), AllowAnonymous]
        public async Task<PerfilDTO> NovoUsuarioPerfil()
        {
            return await appService.Criar();
        }
    }
}
