using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.SaldoGarantia;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;

namespace Perlink.Oi.Juridico.WebApi.Areas.SAP.Controllers
{
    /// <summary>
    /// Controller do Saldo de Garantia
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class SaldoGarantiaController : JuridicoControllerBase
    {
        private readonly ISaldoGarantiaAppService appService;
        private readonly ILogger<SaldoGarantiaController> logger;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="AppService">Interface com os metodos que podem ser utilizados pela api.</param>
        /// <param name="logger"></param>
        public SaldoGarantiaController(ISaldoGarantiaAppService AppService, ILogger<SaldoGarantiaController> logger)
        {
            this.appService = AppService;
            this.logger = logger;
        }

        /// <summary>
        /// Recupera as informações utilizadas nos filtros de saldo de garantia.
        /// </summary>
        /// <returns>informações utilizadas nos filtros de saldo de garantia.</returns>
        [HttpGet("CarregarFiltros")]
        public async Task<IResultadoApplication<SaldoGarantiaFiltrosViewModel>> CarregarFiltros(long tipoProcesso)
        {
            var resultado = await appService.CarregarFiltros(tipoProcesso);
            return resultado;
        }

        /// <summary>
        /// Retorna o zip do arquivo gerado na execução do agendamento do saldo de garantia.
        /// </summary>
        /// <returns>FileResult</returns>
        /// <param name="nomeArquivo"></param>   
        [HttpGet("DownloadSaldoGarantia")]
        public async Task<ActionResult> DownloadSaldoGarantia(string nomeArquivo)
        {
            var memory = await appService.DownloadSaldoGarantia(nomeArquivo);
            if (memory == null)
                return NotFound();
            return File(memory, GetContentType(nomeArquivo), nomeArquivo);
        }

        /// <summary>
        /// Recupera os Agendamentos do Usuario.
        /// </summary>
        /// <returns> Lista de Agendamentos.</returns>
        [HttpPost("ConsultarAgendamento")]
        public async Task<IPagingResultadoApplication<ICollection<AgendamentoResultadoViewModel>>> ConsultarAgendamento(OrdernacaoPaginacaoDTO ordernacaoPaginacaoDTO)
        {
            var result = await appService.ConsultarAgendamento(ordernacaoPaginacaoDTO);
            return result;
        }
        /// <summary>
        /// Salvar o Agendamento da exportação de Saldo Garantia
        /// </summary>
        /// <returns></returns>
        /// <param name = "filtroDTO" ></param>

        [HttpPost("Agendar")]
        public async Task<IResultadoApplication> SalvarAgendamento(SaldoGarantiaAgendamentoDTO filtroDTO)
        {
            var resultado = await appService.SalvarAgendamento(filtroDTO);

            return resultado;
        }

        /// <summary>
        /// Consulta os Criterios de Pesquisa de um agendamento específico.
        /// </summary>
        /// <returns>Dicionário de Criterios do agendamento</returns>
        /// <param name = "codigoAgendamento" ></param>

        [HttpGet("ConsultarCriteriosPesquisa")]
        public async Task<IResultadoApplication<ICollection<KeyValuePair<string, string>>>> ConsultarCriteriosPesquisa(long codigoAgendamento)
        {
            var resultado = await appService.ConsultarCriteriosPesquisa(codigoAgendamento);

            return resultado;
        }

        /// <summary>
        /// Exclui um Agendamento e seus criterios com base no ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExcluirAgendamento")]
        public async Task<IResultadoApplication> ExcluirAgendamento(long codigoAgendamento)
        {
            var result = await appService.ExcluirAgendamento(codigoAgendamento);
            return result;
        }

        //TODO: [MARCUS] VERIFICAR NECESSIDADE DESSE ENDPOINT
        [HttpGet("executar")]
        public async Task ExecutarRotina()
        {
            await appService.ExecutarAgendamentoSaldoGarantia(logger);
        }
    }
}