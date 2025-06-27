using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.General;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Compartilhado.Controllers.InterfaceBB
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class BBStatusParcelasController : JuridicoControllerBase
    {
        private readonly IBBStatusParcelasAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="AppService">Interface com os metodos que podem ser utilizados pela api.</param>

        public BBStatusParcelasController(IBBStatusParcelasAppService AppService)
        {
            this.appService = AppService;
        }

        /// <summary>
        /// Retorna um csv com o resultado.
        /// </summary>
        /// <returns>byte[]</returns>

        [HttpPost("ExportarBBStatusParcelas")]
        public async Task<IResultadoApplication<byte[]>> ExportarBBStatusParcelas(DescriptionFilterViewModel filtroDTO)
        {
            var result = await appService.ExportarBBStatusParcelas(filtroDTO);
            return result;
        }

        /// <summary>
        /// Recuperar Todas as BB Status Parcela
        /// </summary>
        /// <returns>Lista de Empresas_SapResultadoViewModel</returns>
        /// <param name="filtroDTO"></param>
        [HttpPost("ConsultarBBStatusParcelas")]
        public async Task<IPagingResultadoApplication<ICollection<BBStatusParcelasViewModel>>> ConsultarBBStatusParcelas([FromBody] DescriptionFilterViewModel filtroDTO)
        {
            var resultado = await appService.ConsultarBBStatusParcelas(filtroDTO);

            return resultado;
        }

        ///<summary>
        ///Cadastra ou Atualiza BB Status Parcela
        ///</summary>
        ///<returns>
        ///Retorna o Id caso o cadastro ocorra com sucesso
        ///</returns>
        /// <param name="inclusaoEdicao"></param>
        [HttpPost("SalvarBBStatusParcelas")]
        public async Task<IResultadoApplication> SalvarBBStatusParcelas([FromBody]BBStatusParcelaInclusaoEdicaoDTO inclusaoEdicao)
        {
            var resultado = await appService.SalvarBBStatusParcelas(inclusaoEdicao);

            return resultado;
        }

        /// <summary>
        /// Exclui uma BBStatusParcelas com base no ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExcluirBBStatusParcelas")]
        public async Task<IResultadoApplication> ExcluirBBStatusParcelas(long Codigo)
        {
            var result = await appService.ExcluirBBStatusParcelas(Codigo);
            return result;
        }
    }
}