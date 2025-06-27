using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Oi.Juridico.Contextos.V2.AgendCargaCompromissoContext.Data;
using Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.WebApi.V2.Areas.AgendamentoCargaCompromissos.DTOs;
using Oi.Juridico.WebApi.V2.Areas.AgendamentoCargaCompromissos.Services;
using Oi.Juridico.WebApi.V2.Areas.AgendamentoRelatorioNegociacao.Services;
using Oi.Juridico.WebApi.V2.Areas.Compromissos.DTOs;
using Oi.Juridico.WebApi.V2.Areas.SolicitacaoAcesso.Controllers;
using Oi.Juridico.WebApi.V2.Attributes;
using Oi.Juridico.WebApi.V2.Helpers;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Repositories;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Services;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.IO;
using System.Runtime.CompilerServices;

namespace Oi.Juridico.WebApi.V2.Areas.Compromissos
{

    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentoCargaCompromissoController : ControllerBase
    {
        private readonly AgendCargaCompromissoDbContext _db;
        private readonly ParametroJuridicoContext _parametroJuridico;
        private ILogger<AgendamentoCargaCompromissoController> _logger;

        public AgendamentoCargaCompromissoController(AgendCargaCompromissoDbContext db, ParametroJuridicoContext parametroJuridico, ILogger<AgendamentoCargaCompromissoController> logger)
        {
            _db = db;
            _parametroJuridico = parametroJuridico;
            _logger = logger;
        }

        [HttpGet("obter-agendamentos")]        
        public async Task<IActionResult> ObterAgendamentos(CancellationToken ct, 
                                                           [FromServices] AgendamentoCargaCompromissoService service,
                                                           [FromQuery] DateTime? dataInicio = null, 
                                                           [FromQuery] DateTime? dataFim = null,
                                                           [FromQuery] int page = 0, 
                                                           [FromQuery] int pageSize = 0)
        {
            try
            {
                var request = new ObterAgendamentoCargaCompromissoRequest()
                {
                    DataFimAgendamento = dataFim,
                    DataInicioAgendamento = dataInicio,
                    Page = page-1,
                    PageSize = pageSize
                };
                var (data, total, mensagemErro) = await service.ObtemDadosAgendamentoAsync(
                    request, ct);

                if (!string.IsNullOrEmpty(mensagemErro))
                {
                    return BadRequest(mensagemErro);
                }

                return Ok(new { data, total });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


      

        [HttpPost("incluir-agendamento")]
        public async Task<ActionResult> IncluirAgendamentoAsync([FromBody] AgendamentoCargaCompromissoRequest requestDTO, 
            [FromServices] AgendamentoCargaCompromissoService service, CancellationToken ct)
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
        public async Task<ActionResult> ExcluirAgendamentoAsync([FromQuery] int id, 
                    [FromServices] AgendamentoCargaCompromissoService service, CancellationToken ct)
        {
            try
            {
                var (excluido, mensagem) = await service.RemoveAgendamentoAsync(id, User.Identity!.Name, ct);

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
        /// <summary>
        /// Cria um novo agendamento.
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Criar([FromServices] AgendamentoCargaCompromissoService service, 
            [FromForm] IFormFileCollection arquivoCompromisso, 
            [FromForm] string tipoProcesso,
            [FromForm] string configExec,
            [FromForm] string mensagem,
            [FromForm] int status,
            [FromForm] DateTime? datAgendamento,
            CancellationToken cancellationToken)
        {
            try
            {
                var req = new AgendamentoCargaCompromissoRequest();

                if (arquivoCompromisso.Count==0)
                    return BadRequest();

                UploadValidator validCSV = new UploadValidator();
                var validCsvResult = (await validCSV.ValidarArquivoCSV(arquivoCompromisso));
                if (!string.IsNullOrEmpty(validCsvResult))
                {
                    //return Ok(new JsonResult("{msg:" + validCsvResult+"}"));
                    //return Ok(new JsonResult(validCsvResult));
                    return BadRequest(new JsonResult(validCsvResult));
                }

                var result = await service.Criar(arquivoCompromisso, 
                    new AgendamentoCargaCompromissoRequest
                    {
                        TipoProcesso = tipoProcesso,
                        ConfigExec = configExec,
                        Status = status,      
                        Mensagem = mensagem,
                        DatAgendamento = (configExec == "0") ? DateTime.Now : datAgendamento.Value
                    }
                    , User.Identity!.Name!, cancellationToken);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        [HttpGet("download/ArquivoBase/{codAgendamento}/{tipo}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObterArquivoBaseCarregadoAsync(CancellationToken ct, int codAgendamento, int tipo , [FromServices] AgendamentoCargaCompromissoService service)
        {
            try
            {
                var (mensagem, nomeArquivo, nasArquivo) = await service.DownloadAgendamentoAsync(codAgendamento,tipo, ct);

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
