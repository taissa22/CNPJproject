using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;

namespace Perlink.Oi.Juridico.WebApi.Areas.SAP.Controllers.InterfaceBB
{
    /// <summary>
    /// Controller do BBOrgaos
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BBOrgaosController : JuridicoControllerBase {

        private readonly IBBOrgaosAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="AppService">Interface com os metodos que podem ser utilizados pela api.</param>
        public BBOrgaosController(IBBOrgaosAppService AppService) {
            this.appService = AppService;
        }

        /// <summary>
        /// Retorna Json com os filtros utilizados em tela
        /// </summary>
        /// <returns>filtrosOrgaosViewMOdel</returns>
        [HttpGet("CarregarFiltros")]
        public async Task<IResultadoApplication<BBOrgaosFiltrosViewModel>> CarregarFiltros() {
            var result = await appService.CarregarFiltros();
            return result;
        }

        ///<sumary>
        ///Retorna lista de BBOrgaos paginado para grid
        ///</sumary>
        ///<returns>BBOrgaosViewModel</returns>
        [HttpPost("ConsultarBBOrgaos")]
        public async Task<IPagingResultadoApplication<ICollection<BBOrgaosViewModel>>> ConsultarBBOrgaos([FromBody] ConsultaBBOrgaosDTO consultaBBOrgaosDTO) {
            var result = await appService.ConsultarBBOrgaos(consultaBBOrgaosDTO);
            return result;
        }

        ///<summary>
        ///Retorna o byte para exportação
        ///</summary>
        ///<returns></returns>
        [HttpPost("ExportarBBOrgaos")]
        public async Task<IResultadoApplication<byte[]>> ExportarBBOrgaos([FromBody] ConsultaBBOrgaosDTO consultaBBOrgaosDTO) {
            var result = await appService.ExportarBBOrgaos(consultaBBOrgaosDTO);
            return result;
        }

        #region CRUD
        ///<summary>
        ///Realiza a inclusão/edição de um orgão
        ///</summary>
        ///<returns>execução da solicitação</returns>
        [HttpPost("SalvarBBOrgaos")]
        public async Task<IResultadoApplication> SalvarBBOrgaos([FromBody]BBOrgaosViewModel viewModel) {
            var result = await appService.SalvarBBOrgaos(viewModel);
            return result;
        }

        ///<summary>
        ///Realiza a exclusão de um orgão
        ///</summary>
        ///<returns>execução da solicitação</returns>
        [HttpGet("ExcluirBBOrgaos")]
        public async Task<IResultadoApplication> ExcluirBBOrgaos(long codigo) {
            var result = await appService.ExcluirBBOrgaos(codigo);
            return result;
        }
        #endregion CRUD

    }
}