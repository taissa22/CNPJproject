using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoFormaPagamento;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFormaPagamento;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Compartilhado.Controllers
{
    /// <summary>
    /// Controle responsável por gerenciar as requisições ao recurso Forma de Pagamento.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class FormaPagamentoController : JuridicoControllerBase
    {
        private readonly IFormaPagamentoAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public FormaPagamentoController(IFormaPagamentoAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Recuperar Todos os parametros
        /// </summary>
        /// <returns>Lista de parametros</returns>
        [HttpGet("obterFormaPagamento/{id}")]
        public async Task<IResultadoApplication<FormaPagamentoViewModel>> Get(string id)
        {
            long _id = long.Parse(id);
            var resultado = await appService.RecuperarPorId(_id);
            return resultado;
        }

        /// <summary>
        /// Recuperar Todos os parametros para a grid de manutenção
        /// </summary>
        /// <returns>Lista de parametros para a grid de manutenção</returns>
        [HttpPost("ConsultarFormaPagamento")]
        public async Task<IPagingResultadoApplication<IEnumerable<FormaPagamentoGridViewModel>>> GetFormaPagamentoGridManutencao([FromBody] FormaPagamentoFiltroDTO filtros)
        {
            var resultado = await appService.GetFormaPagamentoGridManutencao(filtros);
            resultado.Total = await appService.GetTotalFormaPagamentoGridManutencao(filtros);
            return resultado;
        }

        /// <summary>
        /// Recuperar e exportar uma lista de forma de pagamento.
        /// </summary>
        /// <returns>Byte em formato CSV</returns>
        [HttpPost("ExportarFormaPagamento")]
        public async Task<IResultadoApplication<byte[]>> ExportarGridFormasPagamento([FromBody] FormaPagamentoFiltroDTO filtroDTO)
        {
            var resultado = await appService.ExportarFormasPagamento(filtroDTO);
            return resultado;
        }

        /// <summary>
        /// Salvar ou Editar Forma Pagamento
        /// </summary>
        /// <returns></returns>
        [HttpPost("SalvarFormaPagamento")]
        public async Task<IResultadoApplication> SalvarFormaPagamento([FromBody] FormaPagamentoInclusaoEdicaoDTO inclusaoEdicaoDTO)
        {
            var resultado = await appService.SalvarFormaPagamento(inclusaoEdicaoDTO);
            return resultado;
        }

        /// <summary>
        /// Exclui uma forma de pagamento
        /// </summary>
        /// <returns>Lista de parametros para a grid de manutenção</returns>
        [HttpGet("ExcluirFormaPagamento")]
        public async Task<IResultadoApplication> ExcluirFormaPagamento(long codigo)
        {
            var resultado = await appService.ExcluirFormaPagamentoComAssociacao(codigo);

            return resultado;
        }
    }
}