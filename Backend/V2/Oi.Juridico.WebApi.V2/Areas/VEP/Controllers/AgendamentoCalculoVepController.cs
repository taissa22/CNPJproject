using Oi.Juridico.Contextos.V2.AgendamentoCalculoVEPContext.Data;
using Oi.Juridico.Contextos.V2.AgendamentoCalculoVEPContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.Extensions;
using Oi.Juridico.WebApi.V2.Areas.VEP.DTOs;

namespace Oi.Juridico.WebApi.V2.Areas.VEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentoCalculoVepController : ControllerBase
    {
        private readonly AgendamentoCalculoVEPDbContext _db;
        private readonly ParametroJuridicoContext _parametroJuridico;
        private ILogger<AgendamentoCalculoVepController> _logger;

        public AgendamentoCalculoVepController(AgendamentoCalculoVEPDbContext db, ParametroJuridicoContext parametroJuridico, ILogger<AgendamentoCalculoVepController> logger)
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
                var lista = _db.AgendCalcVep
                   .AsNoTracking()
                   .OrderByDescending(x => x.DatAgendamento)
                   .Select(x => new AgendarCalculoVepResponse
                   {
                       Cod = x.CodAgendCalcVep,
                       DatAgendamento = x.DatAgendamento,
                       DatInicioExecucao = x.DatInicioExecucao,
                       DatFimExecucao = x.DatFimExecucao,
                       DatProximaExecucao = x.DatProximaExecucao,
                       UsrCodUsuario = x.UsrCodUsuario,
                       PeriodicidadeExecucao = x.PeriodicidadeExecucao,
                       NumMeses = x.NumMeses,
                       IndUltimoDiaDoMes = x.IndUltimoDiaDoMes,
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
        public async Task<IActionResult> IncluirAsync(CancellationToken ct, [FromBody] AgendarCalculoVepRequest dto)
        {
            try
            {
                var agend = new AgendCalcVep();
                agend.DatAgendamento = DateTime.Now;
                agend.DatEspecifica = dto.PeriodicidadeExecucao == 1 ? dto.DatEspecifica : null;
                agend.DatProximaExecucao = CalcularDataProximoAgendamento(DateTime.Now.Date, dto);
                agend.UsrCodUsuario = User.Identity!.Name;
                agend.PeriodicidadeExecucao = dto.PeriodicidadeExecucao;
                agend.NumMeses = dto.NumMeses;
                agend.IndUltimoDiaDoMes = dto.IndUltimoDiaDoMes ? "S" : "N";
                agend.DiaDoMes = dto.DiaDoMes;
                agend.Status = 0;

                await _db.AgendCalcVep.AddAsync(agend, ct);
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
                var nomeArquivo = await _db.AgendCalcVep
                    .AsNoTracking()
                    .Where(x => x.CodAgendCalcVep == agendamentoId)
                    .Select(x => x.NomArquivoGerado)
                    .FirstAsync(ct);

                // obtém o parâmetro jurídico com o caminho do NAS onde o arquivo se encontra
                var arrArquivos = await _parametroJuridico.TratarCaminhoDinamicoArrayAsync(ParametrosJuridicos.DIR_SERV_CALCULO_VEP, nomeArquivo);

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
                var agendamento = _db.AgendCalcVep.FirstOrDefault(a => a.CodAgendCalcVep == codAgendamento);
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

        private DateTime? CalcularDataProximoAgendamento(DateTime dataAtual, AgendarCalculoVepRequest model)
        {
            //Imediato
            if (model.PeriodicidadeExecucao == 0)
                return dataAtual.Date;

            //Data Específica
            if (model.PeriodicidadeExecucao == 1)
                return model.DatEspecifica;

            //Mensal
            if (model.PeriodicidadeExecucao == 4)
                return model.IndUltimoDiaDoMes ? dataAtual.GetLastDayOfMonth() : dataAtual.ToDayOfMonth(model.DiaDoMes!.Value).AddMonths(1);

            return null;
        }

        #endregion


    }
}
