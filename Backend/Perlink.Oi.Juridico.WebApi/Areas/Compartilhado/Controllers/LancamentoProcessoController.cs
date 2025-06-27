using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.EstornoLancamentoPago;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.CivelEstrategico.WebApi.Areas.Compartilhado.Controllers
{
    /// <summary>16012012
    /// Controller dos parametros
    /// </summary>
    [Route("[controller]")]
    [ApiController]  
    public class LancamentoProcessoController : JuridicoControllerBase
    {
        private readonly ILancamentoProcessoAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public LancamentoProcessoController(ILancamentoProcessoAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Recuperar Todos os Numeros de Guia
        /// </summary>
        /// <returns>Lista de numeros de guia</returns>
        [HttpGet("obterLancamento")]
        public async Task<IResultadoApplication<ICollection<LancamentoProcessoViewModel>>> Get()
        {
            var resultado = await appService.RecuperarTodos();
            return resultado;
        }

        /// <summary>
        /// Recuperar Todas as partes de Numeros de Guia
        /// </summary>
        /// <returns>Lista as partes de numeros de guia</returns>
        [HttpGet]
        [Route("getByParteNumeroGuia")]
        public async Task<IResultadoApplication<ICollection<LancamentoProcessoViewModel>>> GetByParteNumeroGuia(long NumeroGuia)
        {
            var resultado = await appService.RecuperarPorParteNumeroGuia(NumeroGuia);
            return resultado;
        }
        /// <summary>
        /// Recuperar Todas as partes de Numeros de Guia
        /// </summary>
        /// <returns>Lista as partes de numeros de guia</returns>
        [HttpGet]
        [Route("RecuperarNumeroContaJudicial")]
        public async Task<IResultadoApplication<long?>> RecuperarNumeroContaJudicial(long NumeroContaJudicial)
        {
            var resultado = await appService.RecuperarNumeroContaJudicial(NumeroContaJudicial);
            return resultado;
        }

        /// <summary>
        /// Recuperar Lançamento Processo filtrando  numero guia e codigo do Processo
        /// </summary>
        /// <returns>Lista os processos de acordo com o numero guia</returns>
        [HttpGet]
        [Route("RecuperarNumeroGuia")]
        public async Task<IResultadoApplication<long>> RecuperarLancamentoProcesso(long NumeroGuia, long CodigoTipoProcesso) {
            var resultado = await appService.RecuperarLancamentoProcesso(NumeroGuia, CodigoTipoProcesso);
            return resultado;
        }

        /// <summary>
        /// Recuperar Lançamento Processo do lote
        /// </summary>
        /// <returns>Lista LoteLancamentoViewModel</returns>
        [HttpGet]
        [Route("ObterLancamentoDoLote")]
        public async Task<IResultadoApplication<ICollection<LoteLancamentoViewModel>>> ObterLancamentoDoLote(long codigoLote) {
            var resultado = await appService.ObterLancamentoDoLote(codigoLote);
            return resultado;
        }

        /// <summary>
        /// Retorna o csv dos lancamentos de acordo com o código do lote.
        /// </summary>
        /// <returns>byte[]</returns>
        /// <param name="codigoLote"></param>
        /// <param name="codigoTipoProcesso"></param>
        [HttpGet("ExportarLancamentoDoLote")]
        public async Task<IResultadoApplication<byte[]>> ExportarLancamentoDoLote(long codigoLote, long codigoTipoProcesso)
        {
            var result = await appService.ExportarLancamentoDoLote(codigoLote, codigoTipoProcesso);
            
            return result;
        }

        /// <summary>
        /// Atualiza a data de envio escritorio de lançamentos selecionados
        /// </summary>
        /// <returns>Se operação foi realizada</returns>
        /// <param name="loteLancamentos"></param>        
        [HttpPost("AlterarDataEnvioEscritorio")]
        public async Task<IResultadoApplication> AlterarDataEnvioEscritorio([FromBody] List<LoteLancamentoViewModel> loteLancamentos) {
            var result = await appService.AlterarDataEnvioEscritorio(loteLancamentos);

            return result;
        }

        #region Estorno de lançamentos
        /// <summary>
        /// Retorna lista de processos para estorno de lançamentos
        /// </summary>
        /// <returns>Lista de DadosProcessoEstornoViewModel</returns>
        /// <param name="lancamentoEstornoFiltroDTO"></param>
        [HttpPost("ConsultaLancamentoEstorno")]
        public async Task<IResultadoApplication<ICollection<DadosProcessoEstornoViewModel>>> ConsultaLancamentoEstorno([FromBody] LancamentoEstornoFiltroDTO lancamentoEstornoFiltroDTO)
        {
            var result = await appService.ConsultaLancamentoEstorno(lancamentoEstornoFiltroDTO);

            return result;
        }

        /// <summary>
        /// Executra o estorno de um lançamento
        /// </summary>
        /// <returns>o resultado da operação</returns>
        /// <param name="dadosLancamentoEstornoDTO"></param>
        [HttpPost("EstornaLancamento")]
        public async Task<IResultadoApplication> EstornaLancamento([FromBody] DadosLancamentoEstornoDTO dadosLancamentoEstornoDTO) {
            var result = await appService.EstornaLancamento(dadosLancamentoEstornoDTO);

            return result;
        }

        #endregion Estorno de lançamentos
    }
}