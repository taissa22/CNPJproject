using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Enums;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Repositories;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Services;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.IO;
using FileHandler = System.IO.File;

namespace Perlink.Oi.Juridico.WebApi.Areas.DocumentoCredor {

    /// <summary>
    /// Controller das requisições de agendamento de carga de documentos
    /// </summary>
    [Route("documento-credor/agendamentos-cargas-documentos")]
    [ApiController]
    [AllowAnonymous]    
    public class AgendamentosCargasDocumentosController : ApiControllerBase {

        /// <summary>
        /// Obter os agendamentos
        /// </summary>
        [HttpGet]
        public IActionResult ObterPaginado([FromServices] IAgendamentoCargaDocumentoRepository repository,
           [FromQuery] int pagina) {
            return Result(repository.ObterPaginado(pagina));
        }

        /// <summary>
        /// Download dos arquivos de carga de documentos
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="tipoArquivo"></param>
        /// <param name="agendamentoId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("download-arquivos/{tipoArquivo}")]
        public IActionResult ObterArquivosCarregados([FromServices] IAgendamentoCargaDocumentoRepository repository,
            [FromRoute] string tipoArquivo, [FromQuery] int? agendamentoId = null) {
            TipoArquivo tipoArquivoEnum = EnumHelpers.ParseOrDefault(tipoArquivo, TipoArquivo.NaoInformado);

            var csv = repository.ObterArquivos(tipoArquivoEnum, agendamentoId);

            if (csv.Dados is null) {
                return Result(CommandResult.Invalid("Arquivo não encontrado."));
            }

            return File(FileHandler.ReadAllBytes(csv.Dados), "text/csv", Path.GetFileName(csv.Dados));
        }

        /// <summary>
        /// Exclui um agendamento
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Remover([FromServices] IAgendamentoCargaDocumentoService service,
            [FromRoute] int id) {
            return Result(service.Remover(id));
        }

        /// <summary>
        /// Cria um novo agendamento.
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public IActionResult Criar([FromServices] IAgendamentoCargaDocumentoService service, [FromForm] IFormFile arquivosDocumentos) {
            return Result(service.Criar(arquivosDocumentos));
        }
    }
}