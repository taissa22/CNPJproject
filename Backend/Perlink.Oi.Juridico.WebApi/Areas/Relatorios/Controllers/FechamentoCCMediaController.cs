using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;

namespace Perlink.Oi.Juridico.WebApi.Areas.Relatorios.Controllers
{
    /// <summary>
    /// Controller para requisições para orgão.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("fechamento-cc-media")]
    public class FechamentoCCMediaController : ApiControllerBase
    {
        /// <summary>
        /// Recupera os os fechamentos através do perído
        /// </summary>
        [HttpGet]
        public IActionResult ObterPaginado([FromServices] IFechamentoCCMediaRepository repository, [FromQuery] DateTime? dataInicial, [FromQuery] DateTime? dataFinal,
            [FromQuery] int pagina = 1)
        {
            return Result(repository.ObterPaginado(dataInicial, dataFinal, pagina));
        }

        /// <summary>
        /// Trata do download relatório do CC Média
        /// </summary>
        [HttpGet("baixar/{id}")]
        public IActionResult Baixar([FromServices] IFechamentoCCMediaRepository repository, [FromRoute] long id)
        {
            try
            {
                string nomeArquivo = string.Empty;
                var arquivosZip = repository.Baixar(id, ref nomeArquivo);

                if (arquivosZip.Dados is null)
                    return Result(CommandResult.Invalid("Arquivo não encontrado."));
                else
                    return File(arquivosZip.Dados, "application/zip", nomeArquivo);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro interno de servidor: " + ex.Message);
            }
        }
    }
}