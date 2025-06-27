using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Enums;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Repositories;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Services;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.WebApi.DTOs.DocumentoCredor;

namespace Perlink.Oi.Juridico.WebApi.Areas.DocumentoCredor
{
    /// <summary>
    /// Controller das requisições de agendamento de carga de comprovantes
    /// </summary>
    [Route("documento-credor/agendamento-carga-comprovantes")]
    [ApiController]
    [Authorize]
    public class AgendamentosCargasComprovantesController : ApiControllerBase
    {
        /// <summary>
        /// Obter os agendamentos paginados de 5 em 5
        /// </summary>
        [HttpGet]
        public IActionResult ObterPaginado(
           [FromServices] IAgendamentoCargaComprovanteRepository repository,
           [FromQuery] int pagina)
        {
            return Result(repository.ObterPaginado(pagina));
        }

        /// <summary>
        /// Download dos arquivos de carga de comprovantes
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="tipoArquivo"></param>
        /// <param name="agendamentoId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("download-arquivos/{tipoArquivo}")]
        public IActionResult ObterArquivosCarregados([FromServices] IAgendamentoCargaComprovanteRepository repository, [FromRoute] string tipoArquivo, [FromQuery] int? agendamentoId)
        {
            TipoArquivo tipoArquivoEnum = EnumHelpers.ParseOrDefault(tipoArquivo, TipoArquivo.NaoInformado);

            var arquivosZip = repository.ObterArquivos(tipoArquivoEnum, agendamentoId);

            if (arquivosZip.Dados is null)
            {
                return Result(CommandResult.Invalid("Arquivo não encontrado."));
            }

            switch (tipoArquivoEnum)
            {
                case TipoArquivo.ArquivosCarregados:
                    return File(arquivosZip.Dados, "application/zip", "ArquivosCarregados.zip");
                case TipoArquivo.ResultadoCarga:
                    return File(arquivosZip.Dados, "application/zip", "ResultadoCarga.zip");
                case TipoArquivo.ArquivosPadrao:
                    return File(arquivosZip.Dados, "application/zip", "Arquivos Padrão.zip");
                default:
                    return Result(CommandResult.Invalid("Arquivo não encontrado."));
            }
        }

        /// <summary>
        /// Exclui um agendamento
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover([FromServices] IAgendamentoCargaComprovanteService service,
            [FromRoute] int id)
        {
            return Result(service.Remover(id));
        }

        /// <summary>
        /// Cria um novo agendamento.
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public IActionResult Criar([FromServices] IAgendamentoCargaComprovanteService service, [FromForm] CargaComprovantesUploadDTO arquivos)
        {

            var baseSAP = service.ValidarBaseSAP(arquivos.ArquivoBaseSAP);
            if (!baseSAP.IsValid)
            {
                return Result(CommandResult.Invalid(baseSAP.Mensagens));
            }


            return Result(service.Criar(arquivos.ArquivoComprovantes, arquivos.ArquivoBaseSAP));
        }
    }
}