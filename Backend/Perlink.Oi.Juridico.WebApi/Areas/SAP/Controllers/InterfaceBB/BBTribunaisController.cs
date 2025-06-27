using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Perlink.Oi.Juridico.WebApi.Areas.Compartilhado.Controllers.InterfaceBB
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]
 
    public class BBTribunaisController : JuridicoControllerBase
    {
        private readonly IBBTribunaisAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="AppService">Interface com os metodos que podem ser utilizados pela api.</param>


        public BBTribunaisController(IBBTribunaisAppService AppService)
        {
            this.appService = AppService;
        }

        /// <summary>
        /// Retorna um csv com o resultado.
        /// </summary>
        /// <returns>byte[]</returns>

        [HttpPost("ExportarBBTribunais")]
        public async Task<IResultadoApplication<byte[]>> ExportarTribunaisBB(FiltrosDTO filtros)
        {
            var result = await appService.ExportarBBTribunais(filtros);
            return result;
        }

        /// <summary>
        /// Recuperar Todos BBTribunais
        /// </summary>
        /// <returns>Lista de BBTribunaisViewModel</returns>
        /// <param name="filtros"></param>

        [HttpPost("ConsultarBBTribunais")]
        public async Task<IPagingResultadoApplication<ICollection<BBTribunaisViewModel>>> ConsultarBBTribunais(FiltrosDTO filtros)
        {
            var resultado = await appService.ConsultarBBTribunais(filtros);


         return resultado;
        }

        /// <summary>
        /// Adicionar e Editar BBTribunais
        /// </summary>
        /// <returns>Resultado</returns>
        /// <param name="tributarioInclusaoEdicaoDTO"></param>

        [HttpPost("SalvarBBTribunais")]
        public async Task<IResultadoApplication> SalvarBBTribunais(BBTributarioInclusaoEdicaoDTO tributarioInclusaoEdicaoDTO)
        {
            var resultado = await appService.SalvarBBTribunais(tributarioInclusaoEdicaoDTO);

            return resultado;
        }

        /// <summary>
        /// Excluir BBTribunais
        /// </summary>
        /// <param name="Id"></param>

        [HttpGet("ExcluirBBTribunais")]
        public async Task<IResultadoApplication> ExcluirBBTribunais(long codigo)
        {
            var resultado = await appService.ExcluirBBTribunais(codigo);

            return resultado;
        }
    }
}
