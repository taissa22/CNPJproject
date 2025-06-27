using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.AlteracaoBloco.Interface;
using Perlink.Oi.Juridico.Application.AlteracaoBloco.ViewModel;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.AlteracaoBloco.Controllers
{
    /// <summary>
    /// Controller para Agendamento de alteração em bloco
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AlteracaoEmBlocoController : JuridicoControllerBase
    {
        private readonly IAlteracaoEmBlocoAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public AlteracaoEmBlocoController(IAlteracaoEmBlocoAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Retorna uma lista de agendamentos paginada.
        /// </summary>
        /// <param name="pagina">Indice da lista (Posição inicial)</param>
        /// <param name="qtd">Quantidade retornada.</param>
        /// <returns></returns>
        [HttpGet("listar")]
        //public async Task<IResultadoApplication<ICollection<AlteracaoEmBlocoViewModel>>> ListarAgendamentos(int pagina = 1, int qtd = 5)
        public async Task<IResultadoApplication<ICollection<AlteracaoEmBlocoViewModel>>> ListarAgendamentos(int pagina = 1, int qtd = 5)
        {
            var resultado = await appService.ListarAgendamentos(pagina, qtd);
            return resultado;
        }

        /// <summary>
        /// Método para upload do arquivo de agendamento
        /// </summary>
        /// <param name="arquivo"></param>
        /// <param name="codigoTipoProcesso"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<IResultadoApplication> Upload([FromForm] IFormFile arquivo, long codigoTipoProcesso)
        {
            var resultado = await appService.Upload(Request.Form.Files[0], codigoTipoProcesso);
            return resultado;
        }

        /// <summary>
        /// Responsável por baixar a planilha carregada 
        /// </summary>
        /// <param name="id">codigo na tabela ALTERACAO_BLOCO_WEB</param>
        /// <returns></returns>
        [HttpGet("download/{id}")]
        public async Task<IResultadoApplication<AlteracaoEmBlocoDownloadViewModel>> Download(long id)
        {
            var resultado = await appService.BaixarPlanilhaCarregada(id);
            return resultado;
        }

        /// <summary>
        /// Responsável por baixar a planilha de resultado 
        /// </summary>
        /// <param name="id">codigo na tabela ALTERACAO_BLOCO_WEB</param>
        /// <returns></returns>
        [HttpGet("downloadPlanilhaResultado/{id}")]
        public async Task<IResultadoApplication<AlteracaoEmBlocoDownloadViewModel>> BaixarPlanilhaResultado(long id)
        {
            var resultado = await appService.BaixarPlanilhaResultado(id);
            return resultado;
        }

        /// <summary>
        /// Remove um agendamento com status agendado
        /// </summary>
        /// <param name="id">Chave Primária do agendamento no banco de dados.</param>
        [HttpDelete("{id}")]
        public async Task<IResultadoApplication> Delete(long id)
        {
            var resultado = await appService.RemoverAgendamentoComStatusAgendado(id);
            return resultado;
        }
    }
}
