using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacaoTrabalhistaContext.Data;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacaoTrabalhistaContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.Relatorios.Contingencia.DTOs;
using System.Threading;

namespace Oi.Juridico.WebApi.V2.Areas.Relatorios.Contingencia.Controllers
{
    [Route("relatorio-movimentacoes-trabalhista")]
    [ApiController]
    public class MovimentacoesTrabalhistaControllers : ControllerBase
    {
        private RelatorioMovimentacaoTrabalhistaContext _movimentacaoTrabalhistaContext;
        private ILogger<MovimentacoesTrabalhistaControllers> _logger;
        private readonly ParametroJuridicoContext _parametroJuridico;

        public MovimentacoesTrabalhistaControllers(RelatorioMovimentacaoTrabalhistaContext movimentacaoTrabalhistaContext, ILogger<MovimentacoesTrabalhistaControllers> logger, ParametroJuridicoContext parametroJuridico)
        {
            _movimentacaoTrabalhistaContext = movimentacaoTrabalhistaContext;
            _logger = logger;
            _parametroJuridico = parametroJuridico;
        }

        [HttpGet("listar-agendamentos")]
        public async Task<IActionResult> ListarAgendamentosAsync(CancellationToken ct, [FromQuery] DateTime? dataInicial, [FromQuery] DateTime? dataFinal, [FromQuery] int page = 0, [FromQuery] int size = 5)
        {
            var query = _movimentacaoTrabalhistaContext.AgendMovimentacaoTrab
                .AsNoTracking()
                .Select(x => new ListarAgendamentosResponse
                {
                    Id = x.Id,
                    DatAgendamento = x.DatAgendamento,

                    IniDataFechamento = x.IniDataFechamento,
                    IniNumMesesMediaHistorica = x.IniNumMesesMediaHistorica,
                    IniCodTipoOutlier = x.IniCodTipoOutlier,
                    IniValOutlier = x.IniValOutlier,
                    IniIndMensal = x.IniIndMensal,
                    IniIndFechamentoParcial = x.IniIndFechamentoParcial,
                    IniEmpresas = x.IniEmpresas,
                    FimDataFechamento = x.FimDataFechamento,
                    FimNumMesesMediaHistorica = x.FimNumMesesMediaHistorica,
                    FimCodTipoOutlier = x.FimCodTipoOutlier,
                    FimValOutlier = x.FimValOutlier,
                    FimIndMensal = x.FimIndMensal,
                    FimIndFechamentoParcial = x.FimIndFechamentoParcial,
                    FimEmpresas = x.FimEmpresas,

                    DatInicioExecucao = x.DatInicioExecucao,
                    DatFimExecucao = x.DatFimExecucao,

                    UsrCodUsuario = x.UsrCodUsuario,
                    Status = (byte)(StatusMovimentacaoTrabalhista)x.Status,
                    MsgErro = x.MsgErro,
                    NomArquivo = x.NomArquivo
                });

            if (dataInicial.HasValue && dataFinal.HasValue)
            {
                query = query.Where(x => x.DatAgendamento.Date >= dataInicial && x.DatAgendamento.Date <= dataFinal.Value.Date);
            }

            var total = await query.CountAsync();
            var data = await query.OrderByDescending(x => x.DatAgendamento).Skip(page * size).Take(size).ToListAsync(ct);

            return Ok(new { data, total });
        }

        [HttpGet("listar-fechamentos")]
        public async Task<IActionResult> ListarFechamentosAsync(CancellationToken ct, [FromQuery] DateTime? dataInicial, [FromQuery] DateTime? dataFinal, [FromQuery] int page = 0, [FromQuery] int size = 5, [FromQuery] bool fechamentoMensal = false)
        {
            var query = _movimentacaoTrabalhistaContext.VSoliFechMovRelTrab
                .AsNoTracking()
                .Select(x => new ListarFechamentosResponse
                {
                    DataFechamento = x.DataFechamento,
                    DataExecucao = x.DataExecucao,
                    NumeroMeses = x.NumeroMeses,
                    IndFechamentoMensal = x.IndFechamentoMensal,
                    EmpresasGrupo = x.EmpresasGrupo,
                    IndFechamentoParcial = x.IndFechamentoParcial,
                    CodTipoOutlier = x.CodTipoOutlier,
                    ValOutlier = x.ValOutlier
                });

            if (dataInicial.HasValue && dataFinal.HasValue)
            {
                query = query.Where(x => x.DataFechamento.Date >= dataInicial && x.DataFechamento.Date <= dataFinal);
            }

            if (fechamentoMensal)
            {
                query = query.Where(x => x.IndFechamentoMensal == "S");
            }

            var total = await query.CountAsync(ct);
            var data = await query.OrderByDescending(x => x.DataFechamento).Skip(page * size).Take(size).ToListAsync(ct);

            return Ok(new { data, total });
        }

        [HttpPost("incluir-agendamento")]
        public async Task<IActionResult> IncluirAgendamentoAsync([FromBody] IncluirAgendamentosRequest dto, CancellationToken ct)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Selecione os fechamentos inicial e final que irão compor o relatório!");
                }

                if (dto.iniDataFechamento >= dto.fimDataFechamento)
                {
                    return BadRequest("A data do fechamento inicial deve ser menor do que a data do fechamento final!");
                }

                var agendamento = new AgendMovimentacaoTrab();
                agendamento.IniCodTipoOutlier = dto.iniCodTipoOutlier;
                agendamento.IniDataFechamento = dto.iniDataFechamento;
                agendamento.IniEmpresas = dto.iniEmpresas;
                agendamento.IniIndFechamentoParcial = dto.iniIndFechamentoParcial;
                agendamento.IniIndMensal = dto.iniIndMensal;
                agendamento.IniNumMesesMediaHistorica = dto.iniNumMesesMediaHistorica;
                agendamento.IniValOutlier = dto.iniValOutlier;

                agendamento.FimCodTipoOutlier = dto.fimCodTipoOutlier;
                agendamento.FimDataFechamento = dto.fimDataFechamento;
                agendamento.FimEmpresas = dto.fimEmpresas;
                agendamento.FimIndFechamentoParcial = dto.fimIndFechamentoParcial;
                agendamento.FimIndMensal = dto.fimIndMensal;
                agendamento.FimNumMesesMediaHistorica = dto.fimNumMesesMediaHistorica;
                agendamento.FimValOutlier = dto.fimValOutlier;
                agendamento.DatAgendamento = DateTime.Now;
                agendamento.Status = (byte)StatusMovimentacaoTrabalhista.Agendado;

                agendamento.UsrCodUsuario = User!.Identity!.Name;

                await _movimentacaoTrabalhistaContext.AgendMovimentacaoTrab.AddAsync(agendamento, ct);
                await _movimentacaoTrabalhistaContext.SaveChangesAsync(ct);

                return Ok("Adicionado com Sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("excluir-agendamento/{id}")]
        public async Task<IActionResult> ExcluirAgendamentoAsync(decimal id, CancellationToken ct)
        {
            try
            {
                var agendamento = await _movimentacaoTrabalhistaContext.AgendMovimentacaoTrab.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);
                if (agendamento is null)
                {
                    return BadRequest("Agendamento não encontrado");
                }
                _movimentacaoTrabalhistaContext.Remove(agendamento);
                await _movimentacaoTrabalhistaContext.SaveChangesAsync(ct);

                return Ok("Excluido com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("agendamentos/download/ArquivoBase/{agendamentoId}")]
        public async Task<IActionResult> ObterArquivoBaseCarregadoAsync(CancellationToken ct, int agendamentoId)
        {
            try
            {
                var nomeArquivo = await _movimentacaoTrabalhistaContext.AgendMovimentacaoTrab
                    .AsNoTracking()
                    .Where(x => x.Id == agendamentoId)
                    .Select(x => x.NomArquivo)
                    .FirstAsync(ct);

                // obtém o parâmetro jurídico com o caminho do NAS onde o arquivo se encontra
                var arrArquivos = await _parametroJuridico.TratarCaminhoDinamicoArrayAsync(ParametrosJuridicos.DIR_SERV_REL_MOV_TRAB, nomeArquivo);

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
                return Problem(ex.Message);
            }
        }
    }
}
