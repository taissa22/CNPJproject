using Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Data;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacoesCivelConsumidorContext.Data;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacoesCivelConsumidorContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.Contingencia.Movimentacao.Dtos;
using Oi.Juridico.WebApi.V2.DTOs.Contingencia.Movimentacao;
using System.IO;
using System.Threading;

namespace Oi.Juridico.WebApi.V2.Areas.Contingencia.Movimentacao.Controllers
{
    [Route("relatorio-movimentacoes-civel-consumidor")]
    [ApiController]
    public class RelatorioMovimentacoesCivelConsumidorController : Controller
    {
        private RelatorioMovimentacoesCivelConsumidorContext _movCCContext;
        private readonly ParametroJuridicoContext _parametroJuridico;
        private readonly ControleDeAcessoContext _db;
        private readonly ILogger<RelatorioMovimentacoesCivelConsumidorController> _logger;

        public RelatorioMovimentacoesCivelConsumidorController(ParametroJuridicoContext parametroJuridico, ControleDeAcessoContext db, RelatorioMovimentacoesCivelConsumidorContext movCCContext, ILogger<RelatorioMovimentacoesCivelConsumidorController> logger)
        {
            _parametroJuridico = parametroJuridico;
            _movCCContext = movCCContext;
            _db = db;
            _logger = logger;
        }

        [HttpGet("listar-agendamentos")]
        public async Task<IActionResult> ListarAsync(CancellationToken ct, [FromQuery] DateTime? dataInicial, [FromQuery] DateTime? dataFinal, [FromQuery] int page = 0, [FromQuery] int size = 5)
        {
            var query = _movCCContext.AgendMovimentacaoCc
                .AsNoTracking()
                .Select(x => new ListarCcResponse
                {
                    Id = x.Id,
                    DatAgendamento = x.DatAgendamento,
                    IniDataFechamento = x.IniDataFechamento,
                    IniNumMesesMediaHistorica = x.IniNumMesesMediaHistorica,
                    IniIndicadorFechamentoMensal = x.IniIndMensal,
                    IniPercentualHaircut = x.IniPercentualHaircut,
                    IniIndFechamentoParcial = x.IniIndFechamentoParcial,
                    IniEmpresas = x.IniEmpresas,
                    FimDataFechamento = x.FimDataFechamento,
                    FimNumMesesMediaHistorica = x.FimNumMesesMediaHistorica,
                    FimIndicadorFechamentoMensal = x.FimIndMensal,
                    FimPercentualHaircut = x.FimPercentualHaircut,
                    FimIndFechamentoParcial = x.FimIndFechamentoParcial,
                    FimEmpresas = x.FimEmpresas,
                    DatInicioExecucao = x.DatInicioExecucao,
                    DatFimExecucao = x.DatFimExecucao,
                    Status = (StatusMovimentacaoCc)x.Status,
                    MsgErro = x.MsgErro,
                    UsuarioId = x.UsrCodUsuario,
                    UsuarioNome = x.UsrCodUsuarioNavigation.NomeUsuario
                });

            if (dataInicial != null && dataFinal != null)
            {
                query = query.Where(x => x.DatAgendamento.Date >= dataInicial && x.DatAgendamento.Date <= dataFinal);
            }

            var total = await query.CountAsync(ct);
            var data = await query.Skip(page * size).OrderByDescending(x => x.DatAgendamento).Take(size).ToListAsync(ct);


            return Ok(new { data, total });
        }


        [HttpPost("incluir-agendamento")]
        public async Task<IActionResult> IncluirAsync(CancellationToken ct, [FromBody] CriarMovimentacaoCc dto)
        {
            try
            {
                if (dto.IniDataFechamento >= dto.FimDataFechamento)
                {
                    return BadRequest("A data do fechamento inicial deve ser menor do que a data do fechamento final!");
                }

                var agendamento = new AgendMovimentacaoCc();
                agendamento.IniDataFechamento = dto.IniDataFechamento;
                agendamento.IniEmpresas = dto.IniEmpresas;
                agendamento.IniIndFechamentoParcial = dto.IniIndFechamentoParcial;
                agendamento.IniIndMensal = dto.IniIndMensal;
                agendamento.IniNumMesesMediaHistorica = dto.IniNumMesesMediaHistorica;
                agendamento.IniPercentualHaircut = dto.IniPercentualHaircut;
                agendamento.FimDataFechamento = dto.FimDataFechamento;
                agendamento.FimEmpresas = dto.FimEmpresas;
                agendamento.FimIndFechamentoParcial = dto.FimIndFechamentoParcial;
                agendamento.FimIndMensal = dto.FimIndMensal;
                agendamento.FimNumMesesMediaHistorica = dto.FimNumMesesMediaHistorica;
                agendamento.FimPercentualHaircut = dto.FimPercentualHaircut;
                agendamento.DatAgendamento = DateTime.Now;
                agendamento.Status = 0;
                agendamento.UsrCodUsuario = User.Identity!.Name;

                _movCCContext.AgendMovimentacaoCc.Add(agendamento);
                await _movCCContext.SaveChangesAsync(ct);

                return Ok("Adicionado com Sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirAsync(CancellationToken ct, decimal id)
        {
            try
            {
                var agendamento = _movCCContext.AgendMovimentacaoCc.FirstOrDefault(a => a.Id == id);
                if (agendamento is null)
                {
                    return BadRequest("Agendamento não encontrado");
                }
                _movCCContext.Remove(agendamento);
                await _movCCContext.SaveChangesAsync(ct);

                return Ok("Excluido com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("listar-fechamentos")]
        public async Task<IActionResult> ObterFechamentosAsync(CancellationToken ct, [FromQuery] DateTime? dataInicial, [FromQuery] DateTime? dataFinal, [FromQuery] bool fechamentoMensal = false, [FromQuery] int page = 0, [FromQuery] int size = 5)
        {
            var query = _movCCContext.VSoliFechMovRelCc
                       .AsNoTracking()
                       .Select(x => new ObterFechamentosResponseCC
                       {
                           DataExecucao = x.DataExecucao,
                           DataFechamento = x.DataFechamento,
                           PercHaircut = x.PercHaircut,
                           NumeroMeses = x.NumeroMeses,
                           Empresas = x.EmpresasGrupo,
                           IndicaFechamentoMensal = x.IndFechamentoMensal,
                           IndicaFechamentoParcial = x.IndFechamentoParcial,
                       });

            if (dataInicial != null && dataFinal != null)
            {
                query = query.Where(x => x.DataFechamento.Date >= dataInicial && x.DataFechamento <= dataFinal);
            }

            if (fechamentoMensal)
            {
                query = query.Where(x => x.IndicaFechamentoMensal == "S");
            }

            var total = await query.CountAsync(ct);
            var data = await query.Skip(page * size).OrderByDescending(x => x.DataFechamento).Take(size).ToListAsync(ct);

            return Ok(new { data, total });
        }

        [HttpGet("agendamentos/download/ArquivoBase/{agendamentoId}")]
        public async Task<IActionResult> ObterArquivoBaseCarregadoAsync(CancellationToken ct, int agendamentoId)
        {
            try
            {
                var nomeArquivo = await _movCCContext
                    .AgendMovimentacaoCc
                    .AsNoTracking()
                    .Where(x => x.Id == agendamentoId)
                    .Select(x => x.NomArquivo)
                    .FirstAsync(ct);

                // obtém o parâmetro jurídico com o caminho do NAS onde o arquivo se encontra
                var nasArquivo = await _parametroJuridico.TratarCaminhoDinamicoArrayAsync(ParametrosJuridicos.DIR_SERV_REL_MOV_CC, nomeArquivo);

                foreach (var arquivo in nasArquivo)
                {
                    if (System.IO.File.Exists(arquivo))
                    {
                        var dados = await System.IO.File.ReadAllBytesAsync(arquivo);
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
