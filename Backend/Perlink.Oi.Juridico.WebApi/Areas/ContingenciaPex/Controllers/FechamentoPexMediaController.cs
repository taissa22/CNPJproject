using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.ContingenciaPex.Interface;
using Perlink.Oi.Juridico.Application.ContingenciaPex.ViewModel;
using Perlink.Oi.Juridico.WebApi.Helpers;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.ContingenciaPex.Controllers
{
    /// <summary>
    /// Controller para Fechamento Pex por Média
    /// </summary>
    [Route("[controller]")]
    [ApiController]    
    [AllowAnonymous]
    public class FechamentoPexMediaController : JuridicoControllerBase
    {
        private readonly IFechamentoPexMediaAppService appService;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        /// <param name="appService">Interface com os metodos que podem ser utilizados pela api.</param>
        public FechamentoPexMediaController(IFechamentoPexMediaAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// Método para listar andamentos da tabela fechamento pex
        /// </summary>
        /// <returns></returns>
        [HttpGet("listar")]
        public async Task<IResultadoApplication<IEnumerable<FechamentoContingenciaPexMediaViewModel>>> Listar(string dataInicio, string dataFim, int quantidade = 10, int pagina = 1)
        {
            var resultado = await appService.ListarFechamentos(dataInicio, dataFim, quantidade, pagina);
            return resultado;
        }

        /// <summary>
        /// Método para baixar arquivo do fechamento pex
        /// </summary>
        /// <returns></returns>
        [HttpGet("download/{fechamentoId}/{dataFechamento}/{dataGeracao}")]
        public async Task<IActionResult> ObterArquivoFechamento([FromRoute] int fechamentoId, [FromRoute] DateTime dataFechamento, [FromRoute] DateTime dataGeracao)
        {
            try
            {
                var resultado = await appService.ObterArquivo(fechamentoId, dataFechamento, dataGeracao);
                if (resultado.arquivo is null)
                {
                    return NotFound("Arquivo não encontrado, o mesmo encontra-se na fila e será gerado pelo executor");
                }

                return File(resultado.arquivo, "application/zip", resultado.nomeArquivo);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro interno de servidor: " + ex.Message);
            }
        }
    }
}
