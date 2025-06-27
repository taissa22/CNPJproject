using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Relatorios
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class RelatorioGenericoController : JuridicoControllerBase
    {
        /// <summary>
        /// Serviço associado ao empresa do grupo
        /// </summary>
        public readonly IEmpresaDoGrupoAppService appService;
        /// <summary>
        /// Serviço asssociado ao escritórios
        /// </summary>
        public readonly IEscritorioAppService appEscritorioService;

        /// <summary>
        /// Controler de Relatório Genérico
        /// </summary>
        /// <param name="appService"></param>
        /// <param name="appEscritorioService"></param>
        public RelatorioGenericoController(IEmpresaDoGrupoAppService appService, IEscritorioAppService appEscritorioService)
        {
            this.appService = appService;
            this.appEscritorioService = appEscritorioService;
        }
        /// <summary>
        /// Recupera as Empresas associadas ao grupo da Oi
        /// </summary>
        /// <returns>Lista da empresa do grupo</returns>

        [HttpGet("recuperaEmpresadoGrupo")]
        public async Task<IResultadoApplication<IEnumerable<EmpresaDoGrupoListaViewModel>>> RecuperaEmpresadoGrupo()
        {
            return await appService.RecuperarEmpresaDoGrupo();
        }
        /// <summary>
        /// Retorna uma lista dos escritórios associados a área civel consumidor
        /// </summary>
        /// <returns>Lista dos escritórios da área civel consumidor</returns>
        [HttpGet("recuperaEscritorioCivelConsumidor")]
        public async Task<IResultadoApplication<IEnumerable<EscritorioListaViewModel>>> RecuperaEscritorioAreaCivelConsumidor()
        {
            return await appEscritorioService.RecuperarAreaCivelConsumidor();
        }

        /// <summary>
        /// Retorna uma lista dos escritórios associados a área civel Estratégico
        /// </summary>
        /// <returns>Lista dos escritórios da área civel estratégico</returns>
        [HttpGet("recuperaEscritorioAreaCivelEstrategico")]
        public async Task<IResultadoApplication<IEnumerable<EscritorioListaViewModel>>> RecuperaEscritorioAreaCivelEstrategico()
        {
            return await appEscritorioService.RecuperarAreaCivelEstrategico();
        }

        /// <summary>
        /// Retorna uma lista dos escritórios associados a área procon
        /// </summary>
        /// <returns>Lista dos escritórios da área procon</returns>
        [HttpGet("recuperaEscritorioProcon")]
        public async Task<IResultadoApplication<IEnumerable<EscritorioListaViewModel>>> RecuperaEscritorioProcon()
        {
            return await appEscritorioService.RecuperarAreaProcon();
        }

    }
}