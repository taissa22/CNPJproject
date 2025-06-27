using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Contextos.V2.RelatorioPagamentoEscritorioContext.Data;
using Oi.Juridico.Contextos.V2.RelatorioPagamentoEscritorioContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.Extensions;
using Oi.Juridico.WebApi.V2.Areas.VEP.DTOs;

namespace Oi.Juridico.WebApi.V2.Areas.RelatorioPagamentoEscritorio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioPagamentoEscritorioController : ControllerBase
    {
        private readonly RelatorioPagamentoEscritorioDbContext _db;
        private readonly ParametroJuridicoContext _parametroJuridico;
        private ILogger<RelatorioPagamentoEscritorioController> _logger;

        public RelatorioPagamentoEscritorioController(RelatorioPagamentoEscritorioDbContext db, ParametroJuridicoContext parametroJuridico, ILogger<RelatorioPagamentoEscritorioController> logger)
        {
            _db = db;
            _parametroJuridico = parametroJuridico;
            _logger = logger;
        }

        [HttpGet("obter-agendamentos")]
        public async Task<IActionResult> ObterAgendamentos(CancellationToken ct, [FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim, [FromQuery] int page = 0)
        {
            try
            {
                var lista = _db.AgendCalcRelPagEsc
                   .AsNoTracking()
                   .OrderByDescending(x => x.DatAgendamento)
                   .Select(x => new AgendarRelatorioPagamentoEscritorioResponse
                   {
                       Cod = x.CodAgendCalcRelPagEsc,
                       DatAgendamento = x.DatAgendamento,
                       DatInicioExecucao = x.DatInicioExecucao,
                       DatFimExecucao = x.DatFimExecucao,
                       DatProximaExecucao = x.DatProximaExecucao,
                       UsrCodUsuario = x.UsrCodUsuario,
                       PeriodicidadeExecucao = x.PeriodicidadeExecucao,
                       MesReferencia = x.MesReferencia,
                       DiaDoMes = x.DiaDoMes,
                       Status = x.Status
                   });

                if (dataInicio.HasValue && dataFim.HasValue)
                    lista = lista.Where(x => x.DatAgendamento.Date >= dataInicio && x.DatAgendamento.Date <= dataFim);

                var agendamentos = await lista.Skip(page * 5).Take(5).ToArrayAsync(ct);
                var total = await lista.CountAsync(ct);

                return Ok(new { agendamentos, total });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex);
            }
        }

        [HttpPost("incluir-agendamento")]
        public async Task<IActionResult> IncluirAsync(CancellationToken ct, [FromBody] AgendarRelatorioPagamentoEscritorioRequest dto)
        {
            try
            {
                var agend = new AgendCalcRelPagEsc();
                agend.DatAgendamento = DateTime.Now;
                agend.DatEspecifica = dto.PeriodicidadeExecucao == 1 ? dto.DatEspecifica : null;
                agend.DatProximaExecucao = CalcularDataProximoAgendamento(DateTime.Now.Date, dto);
                agend.UsrCodUsuario = User.Identity!.Name;
                agend.PeriodicidadeExecucao = dto.PeriodicidadeExecucao;
                agend.MesReferencia = dto.MesReferencia;
                agend.DiaDoMes = dto.DiaDoMes;
                agend.Status = 0;

                await _db.AgendCalcRelPagEsc.AddAsync(agend, ct);
                await _db.SaveChangesAsync(ct);

                return Ok("Adicionado com Sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex);
            }
        }

        [HttpGet("download/ArquivoBase/{agendamentoId}")]
        public async Task<IActionResult> ObterArquivoBaseCarregadoAsync(CancellationToken ct, int agendamentoId)
        {
            try
            {
                var nomeArquivo = await _db.AgendCalcRelPagEsc
                    .AsNoTracking()
                    .Where(x => x.CodAgendCalcRelPagEsc == agendamentoId)
                    .Select(x => x.NomArquivoGerado)
                    .FirstAsync(ct);

                // obtém o parâmetro jurídico com o caminho do NAS onde o arquivo se encontra
                var arrArquivos = await _parametroJuridico.TratarCaminhoDinamicoArrayAsync(ParametrosJuridicos.DIR_SERV_REL_PAG_ESC, nomeArquivo);

                foreach (var arquivo in arrArquivos)
                {
                    if (System.IO.File.Exists(arquivo))
                    {
                        var dados = await System.IO.File.ReadAllBytesAsync(arquivo, ct);
                        return File(dados, "application/zip", nomeArquivo);
                    }
                }

                return Problem("Arquivo não encontrado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("excluir-agendamento")]
        public async Task<IActionResult> ExcluirAsync(CancellationToken ct, [FromQuery] int codAgendamento)
        {
            try
            {
                var agendamento = _db.AgendCalcRelPagEsc.FirstOrDefault(a => a.CodAgendCalcRelPagEsc == codAgendamento);
                if (agendamento is null)
                {
                    return BadRequest("Agendamento não encontrado");
                }
                _db.Remove(agendamento);
                await _db.SaveChangesAsync(ct);

                return Ok("Excluido com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex);
            }
        }

        #region PRIVATE METHODS

        private DateTime? CalcularDataProximoAgendamento(DateTime dataAtual, AgendarRelatorioPagamentoEscritorioRequest model)
        {
            //Imediato
            if (model.PeriodicidadeExecucao == 0)
                return dataAtual.Date;

            //Data Específica
            if (model.PeriodicidadeExecucao == 1)
                return model.DatEspecifica;

            //Mensal
            if (model.PeriodicidadeExecucao == 4)
                return dataAtual.ToDayOfMonth(model.DiaDoMes!.Value).AddMonths(1);

            return null;
        }

        #endregion


    }
}
