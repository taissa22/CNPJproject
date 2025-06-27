using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Impl;
using Shared.Application.Interface;

namespace Perlink.Oi.Juridico.WebApi.Areas.SAP.Controllers
{
    /// <summary>
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class GruposLotesJuizadosController : JuridicoControllerBase
    {
        private readonly IGruposLotesJuizadosAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public GruposLotesJuizadosController(IGruposLotesJuizadosAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Recuperar Todos os Grupos de Lotes Juizados de acordo com o filtro informado
        /// </summary>
        /// <returns>Lista paginada de FornecedorResultadoViewModel</returns>
        /// <param name="filtros"></param>       
        [HttpPost("ConsultarGruposLotesJuizados")]
        public async Task<IPagingResultadoApplication<ICollection<GruposLotesJuizadosViewModel>>> ConsultarGruposLotesJuizados([FromBody] FiltrosDTO filtros)
        {
            var resultado = await appService.ConsultarGruposLotesJuizadosPorFiltroPaginado(filtros);

            return resultado;
        }

        /// <summary>
        /// Exclui um Grupo de Lote de Juízado com base no ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExcluirGruposLotesJuizados")]
        public async Task<IResultadoApplication> ExcluirGruposLotesJuizados(long codigo)
        {
            var result = await appService.ExcluirGruposLotesJuizados(codigo);
            return result;
        }

        /// <summary>
        /// Exporta os Grupos de Lotes de Juízados
        /// </summary>
        /// <returns>Byte de CSV com os grupos de lotes juizados.</returns>
        [HttpPost("ExportarGruposLotesJuizados")]
        public async Task<IResultadoApplication<byte[]>> ExportarGruposLotesJuizado(FiltrosDTO filtros)
        {
            var result = await appService.ExportarGruposLotesJuizado(filtros);
            return result;
        }

        /// <summary>
        /// Adiciona um grupo de lote juizado no banco.
        /// </summary>
        /// <returns>Byte de CSV com os grupos de lotes juizados.</returns>
        [HttpPost("SalvarGruposLotesJuizados")]
        public async Task<IResultadoApplication> SalvarGruposLotesJuizados([FromBody] GruposLotesJuizadosViewModel model)
        {
            IResultadoApplication result; 
            if(model.Id == 0) // Cadastro
            {
                //model.Id = await appService.ObterUltimoId();
                result = await appService.Inserir(model);
            }
            else // Edição
            {
                result = await appService.Atualizar(model);
            }
            return result;
        }
    }
}