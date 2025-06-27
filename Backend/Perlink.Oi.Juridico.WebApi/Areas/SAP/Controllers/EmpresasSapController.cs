using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Compartilhado.Controllers
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EmpresasSapController : JuridicoControllerBase
    {
        private readonly IEmpresas_SapAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="AppService">Interface com os metodos que podem ser utilizados pela api.</param>

        public EmpresasSapController(IEmpresas_SapAppService AppService)
        {
            this.appService = AppService;
        }

        /// <summary>
        /// Retorna um csv com o resultado.
        /// </summary>
        /// <returns>byte[]</returns>
   
        [HttpPost("ExportarEmpresasSap")]
        public async Task<IResultadoApplication<byte[]>> ExportarEmpresas([FromBody] Empresas_SapFiltroDTO filtroDTO)
        {
            var result = await appService.ExportarEmpresasSap(filtroDTO);
            return result;
        }

        /// <summary>
        /// Recuperar Todas as empresas
        /// </summary>
        /// <returns>Lista de Empresas_SapResultadoViewModel</returns>
        /// <param name="filtroDTO"></param>

        [HttpPost("ConsultarEmpresasSap")]
        public async Task<IPagingResultadoApplication<ICollection<Empresas_SapResultadoViewModel>>> RecuperarEmpresasOrdenadas([FromBody] Empresas_SapFiltroDTO filtroDTO)
        {
            var resultado = await appService.RecuperarEmpresasPorFiltro(filtroDTO);

            return resultado;
        }

        ///<summary>
        ///Criar empresa
        ///</summary>
        ///<returns>
        ///Retorna o Id caso o cadastro ocorra com sucesso
        ///</returns>
        /// <param name="empresa_SapViewModel"></param>
        [HttpPost("SalvarEmpresasSap")]
        public async Task<IResultadoApplication> CadastrarEmpresaSap([FromBody]Empresas_SapViewModel empresa_SapViewModel)
        {

            IResultadoApplication resultado;
            if (empresa_SapViewModel.Id == 0)
                resultado = await appService.CadastrarEmpresa(empresa_SapViewModel);
            else
                resultado = await appService.AtualizarEmpresa(empresa_SapViewModel);

            return resultado;
        }

        /// <summary>
        /// Exclui uma empresa com base no ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExcluirEmpresasSap")]
        public async Task<IResultadoApplication> ExcluirEmpresasSap(long codigo)
        {
            var result = await appService.ExcluirEmpresasSap(codigo);
            return result;
        }
    }
}