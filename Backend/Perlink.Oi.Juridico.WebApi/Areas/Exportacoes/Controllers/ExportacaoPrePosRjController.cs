using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;

namespace Perlink.Oi.Juridico.WebApi.Areas.Exportacoes.Controllers
{
    /// <summary>
    /// Controller para Exportação Base Pre Pos Rj
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ExportacaoPrePosRjController : JuridicoControllerBase
    {
        public readonly IExportacaoPrePosRJAppService appService;

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="appService"></param>
        public ExportacaoPrePosRjController(IExportacaoPrePosRJAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        ///  Retorna lista de exportações rj paginado
        /// </summary>
        /// <param name="dataExtracao"> busca por  data de extração</param>
        /// <param name="pagina">numero da pagina que a ser exibida</param>
        /// <param name="qtd"> quantidade de linhas </param>
        /// <returns> a lista de exportações </returns>
        [HttpGet("buscar")]
        public async Task<IResultadoApplication<ICollection<ExportacaoPrePosRJViewModel>>> ListarExportacaoPrePosRj(DateTime? dataExtracao = null, int pagina = 1, int qtd = 5)
        {
            var resultado = await appService.ListarExportacaoPrePosRj(dataExtracao, pagina, qtd);
            return resultado;
        }

        /// <summary>
        ///  Recebe o valor do checkbox se a exportação vai ser expurgada ou não.
        /// </summary>
        /// <param name="idExtracao">id da extraçao para identificar qual a extração que vai ser alterada</param>
        /// <param name="naoExpurgar">se a extração vai ser expurgada ou nao</param>
        /// <returns></returns>
        [HttpGet("naoExpurgar")]
        public async Task<IResultadoApplication<ExportacaoPrePosRJViewModel>> ExpurgarExportacaoPrePosRj(long idExtracao, bool naoExpurgar)
        {
            var resultado = await appService.ExpurgarExportacaoPrePosRj(idExtracao, naoExpurgar);
            return resultado;
        }

        /// <summary>
        ///   Faz download dos tipos de processos selecionados
        /// </summary>
        /// <param name="idExtracao">id da extração para pegar os arquivos csv</param>
        /// <param name="tiposProcessos">os arquivos para serem zipados</param>
        /// <returns></returns>
        [HttpPost("download")]
        public async Task<IResultadoApplication<ExportacaoPrePosRJListaViewModel>> DownloadExportacaoPrePosRj(long idExtracao, ICollection<string> tiposProcessos)
        {
            var resultado = await appService.DownloadExportacaoPrePosRj(idExtracao, tiposProcessos);
            return resultado;
        }
    }
}
