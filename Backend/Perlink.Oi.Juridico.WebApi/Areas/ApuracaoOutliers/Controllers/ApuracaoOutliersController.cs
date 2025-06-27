using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.ApuracaoOutliers.Interface;
using Perlink.Oi.Juridico.Application.ApuracaoOutliers.ViewModel;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;

namespace Perlink.Oi.Juridico.WebApi.Areas.ApuracaoOutliers.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApuracaoOutliersController : JuridicoControllerBase
    {
        private readonly IApuracaoOutliersAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public ApuracaoOutliersController(IApuracaoOutliersAppService appService) {
            this.appService = appService;
        }

        /// <summary>
        /// Retorna uma lista de agendamentos da apuração de outliers
        /// </summary>        
        /// <returns></returns>
        [HttpGet("CarregarAgendamento")]
        public async Task<IResultadoApplication<ICollection<AgendarApuracaoOutliersViewModel>>> CarregarAgendamento(int pagina = 1, int qtd = 5) {
            var resultado = await appService.CarregarAgendamento(pagina, qtd);
            return resultado;
        }

        /// <summary>
        /// Retorna uma lista de fechamentos JEC disponiveis para agendamento.
        /// </summary>        
        /// <returns></returns>
        [HttpGet("CarregarFechamentosDisponiveisParaAgendamento")]
        public async Task<IResultadoApplication<ICollection<FechamentoJecDisponivelViewModel>>> CarregarFechamentosDisponiveisParaAgendamento() {
            var resultado = await appService.CarregarFechamentosDisponiveisParaAgendamento();
            return resultado;
        }
 
        /// <summary>
        /// Agendar apuração do valor de corte outliers.
        /// </summary>
        /// <returns>Sem retorno</returns>
        [HttpPost("AgendarApuracaoOutliers")]
        [AllowAnonymous]
        public async Task<IResultadoApplication> AgendarApuracaoOutliers(AgendarApuracaoOutliersDTO dto)
        {
            var resultado = await appService.AgendarApuracaoOutliers(dto);
            return resultado;
        }

        /// <summary>
        /// Remove um agendamento com status agendado
        /// </summary>
        /// <param name="id"></param>                
        [HttpDelete("Delete")]
        public async Task<IResultadoApplication> Delete(long id) {
            var resultado = await appService.RemoverAgendamento(id);
            return resultado;
        }

        /// <summary>
        /// Retorna o arquivo gerado na execução do agendamento da Apuração Outliers.
        /// </summary>
        /// <returns>FileResult</returns>
        /// <param name="nomeArquivo"></param>   
        [HttpGet("DownloadApuracaoOutliers")]
        public async Task<ActionResult> DownloadApuracaoOutliers(string nomeArquivo) {
            var memory = await appService.Download(nomeArquivo);
            if (memory == null)
                return NotFound();
            return File(memory, GetContentType(nomeArquivo), nomeArquivo);
        }
    }
}
