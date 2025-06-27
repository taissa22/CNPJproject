using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [AllowAnonymous]
    public class BBResumoProcessamentoController : JuridicoControllerBase
    {
        private readonly IBBResumoProcessamentoAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="AppService">Interface com os metodos que podem ser utilizados pela api.</param>

        public BBResumoProcessamentoController(IBBResumoProcessamentoAppService AppService)
        {
            appService = AppService;
        }

        /// <summary>
        /// Recuperar os arquivos remessas por filtro
        /// </summary>
        /// <returns>Lista de Arquivos Remessas</returns>
        /// <param name = "filtroDTO" ></param>
        [HttpPost("ConsultarArquivoRetorno")]
        public async Task<IResultadoApplication<ICollection<BBResumoProcessamentoResultadoViewModel>>> ConsultarArquivoRetorno([FromBody] BBResumoProcessamentoFiltroDTO filtroDTO)
        {
            var resultado = await appService.ConsultarArquivoRetorno(filtroDTO);

            return resultado;
        }

        /// <summary>
        /// Recuperar parametros necessários sobre o upload de arquivos da imporatação BB.
        /// </summary>
        /// <returns>Objeto com o limite máximo de tamanho do arquivo e com o número máximo de arquivos para Upload</returns>
        [HttpGet("ConsultarParametrosUpload")]
        public async Task<IResultadoApplication<ImportacaoParametroJuridicoUploadViewModel>> ConsultarParametrosUpload()
        {
            return await appService.ConsultarParametrosUpload();
        }

        /// <summary>
        /// Exportar os arquivos remessas por filtro
        /// </summary>
        /// <returns>byte csv com lista de remessas</returns>
        /// <param name = "filtroDTO" ></param>
        [HttpPost("ExportarArquivoRetorno")]
        public async Task<IResultadoApplication<byte[]>> ExportarArquivoRetorno([FromBody] BBResumoProcessamentoFiltroDTO filtroDTO)
        {
            var resultado = await appService.ExportarArquivoRetorno(filtroDTO);

            return resultado;
        }

        /// <summary>
        /// Recupera as Guias do arquivo pelo numero do lote bb
        /// </summary>
        /// <returns>Lista de Guias</returns>
        /// <param name = "numeroLoteBB" ></param>
        [HttpGet("BuscarGuias")]
        public async Task<IResultadoApplication<ICollection<BBResumoProcessamentoGuiaViewModel>>> BuscarGuias(long numeroLoteBB)
        {
            var resultado = await appService.BuscarGuiasOK(numeroLoteBB);

            return resultado;
        }

        /// <summary>
        /// Recupera a Guia do arquivo pelo codigoProcesso e codigoLancamento
        /// </summary>
        /// <returns>Lista de Guias</returns>
        /// <param name="codigoProcesso"></param>
        /// <param name="codigoLancamento"></param>
        [HttpGet("BuscarGuiaExibicao")]
        public async Task<IResultadoApplication<BBResumoProcessamentoGuiaExibidaViewModel>> BuscarGuiaExibicao(long codigoProcesso, long codigoLancamento) {
            var resultado = await appService.BuscarGuiaExibicao(codigoProcesso, codigoLancamento);

            return resultado;
        }

       
        ///<summary>
        ///Realiza a inclusão e alteração de dados do arquivo importado
        ///</summary>
        ///<returns>execução da solicitação</returns>
        [HttpPost("SalvarImportacao")]
        public async Task<IResultadoApplication> SalvarImportacao([FromBody]List<BBResumoProcessamentoImportacaoDTO> listaImportacao)
        {
            return await appService.SalvarImportacao(listaImportacao);
        }

        /// <summary>
        /// Exporta as guias OK pelo número lote bb
        /// </summary>
        /// <returns>byte csv</returns>
        /// <param name = "numeroLoteBB" ></param>
        [HttpGet("ExportarGuias")]
        public async Task<IResultadoApplication<byte[]>> ExportarGuias(long numeroLoteBB)
        {
            var resultado = await appService.ExportarGuias(numeroLoteBB);

            return resultado;
        }

        /// <summary>
        /// Importa um arquivo e retorna a lista de guias OKs e com problemas
        /// </summary>
        /// <param name = "file" ></param>
        [HttpPost("Upload")]
        public async Task<IResultadoApplication<List<BBResumoProcessamentoImportacaoViewModel>>> Upload(List<IFormFile> files)
        {
            var resultado = await appService.Upload(files);

            return resultado;
        }
    }
}