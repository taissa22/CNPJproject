using Oi.Juridico.WebApi.V2.Areas.AgendamentoRelatorioNegociacao.DTOs;
using Oi.Juridico.WebApi.V2.Areas.AgendamentoRelatorioNegociacao.Services;
using Oi.Juridico.WebApi.V2.Attributes;
using Perlink.Oi.Juridico.Infra.Constants;

namespace Oi.Juridico.WebApi.V2.Areas.AgendamentoRelatorioNegociacao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentoRelatorioNegociacaoController : ControllerBase
    {

        [HttpPost("obter-dados-agendamento")]
        [TemPermissao(Permissoes.ACESSAR_AGENDAMENTO_RELATORIO_NEGOCIACAO)]
        public async Task<ActionResult> ObterDadosAgendamentoAsync([FromBody] ObterAgendamentoRelatorioNegociacaoRequest requestDTO, [FromServices] AgendamentoRelatorioNegociacaoService service, CancellationToken ct)
        {
            try
            {
                var (dadosAgendamento, total, mensagemErro) = await service.ObtemDadosAgendamentoAsync(requestDTO, ct);

                if (!string.IsNullOrEmpty(mensagemErro))
                {
                    return BadRequest(mensagemErro);
                }

                return Ok(new { dadosAgendamento, total });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("incluir-agendamento")]
        [TemPermissao(Permissoes.ACESSAR_AGENDAMENTO_RELATORIO_NEGOCIACAO)]
        public async Task<ActionResult> IncluirAgendamentoAsync([FromBody] AgendamentoRelatorioNegociacaoRequest requestDTO, [FromServices] AgendamentoRelatorioNegociacaoService service, CancellationToken ct)
        {
            try
            {
                var mensagem = await service.SalvarAgendamentoAsync(requestDTO, User.Identity!.Name!, ct);

                return Ok(mensagem);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("excluir-agendamento")]
        [TemPermissao(Permissoes.ACESSAR_AGENDAMENTO_RELATORIO_NEGOCIACAO)]
        public async Task<ActionResult> ExcluirAgendamentoAsync([FromQuery] int CodAgendExecRelNegociacao, [FromServices] AgendamentoRelatorioNegociacaoService service, CancellationToken ct)
        {
            try
            {
                var (excluido, mensagem) = await service.RemoveAgendamentoAsync(CodAgendExecRelNegociacao, ct);

                if (!excluido)
                {
                    return BadRequest(mensagem);
                }

                return Ok(mensagem);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("download/ArquivoBase/{CodAgendExecRelNegociacao}")]
        [TemPermissao(Permissoes.ACESSAR_AGENDAMENTO_RELATORIO_NEGOCIACAO)]
        public async Task<IActionResult> ObterArquivoBaseCarregadoAsync(CancellationToken ct, int CodAgendExecRelNegociacao, [FromServices] AgendamentoRelatorioNegociacaoService service)
        {
            try
            {
                var (mensagem, nomeArquivo, nasArquivo) = await service.DownloadAgendamentoAsync(CodAgendExecRelNegociacao, ct);

                if (mensagem is not null)
                    return BadRequest(mensagem);

                foreach (var arquivo in nasArquivo)
                {
                    if (System.IO.File.Exists(arquivo))
                    {
                        var dados = await System.IO.File.ReadAllBytesAsync(arquivo);
                        return File(dados, "application/zip", nomeArquivo);
                    }
                }

                return BadRequest("Arquivo não encontrado.");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

    }
}
