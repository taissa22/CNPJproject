using Microsoft.AspNetCore.Authorization;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.WebApi.V2.Areas.LogProcesso.Trabalhista.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Areas.LogProcesso.Trabalhista.DTO;
using Oi.Juridico.WebApi.V2.Areas.LogProcesso.Trabalhista.Services;

namespace Oi.Juridico.WebApi.V2.Areas.LogProcesso.Trabalhista.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogProcessoTrabalhistaController : ControllerBase
    {
        [HttpGet("consulta")]
        [AllowAnonymous]
        public async Task<ActionResult<LogProcessoTrabalhistaResponse[]>> ObterProcessoAudienciaPrepostoAsync([FromQuery] long codProcesso, [FromServices] LogProcessoTrabalhistaService service, CancellationToken ct)
        {
            try
            {
                var processoAudienciaPreposto = await service.ObterProcessoAudienciaPrepostoAsync(codProcesso, ct);
                if (processoAudienciaPreposto is not null)
                    return Ok(processoAudienciaPreposto);

                return NotFound($"Nenhuma informação de log encontrado para o processo: {codProcesso} ");
            }
            catch (Exception e)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados de deduções de dependentes.", erro = e.Message });
            }
        }

        [HttpGet("exportar")]
        [AllowAnonymous]
        public async Task<IActionResult> ExportarAudienciaPrepostosAsync([FromQuery] long codProcesso, [FromServices] LogProcessoTrabalhistaService service, CancellationToken ct)
        {
            var resultado = await service.ObterProcessoAudienciaPrepostoAsync(codProcesso, ct);

            var csv = resultado.ToCsvByteArray(typeof(LogProcessoTrabalhistaResponseMap), false);

            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            var nomeArquivo = $"Rel_Log_Trabalhista_{DateTime.Now.ToString("ddMMyyyy_hhMMss")}.csv";

            return File(bytes, "application/octet-stream", nomeArquivo);
        }

    }
}
