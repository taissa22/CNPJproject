using Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Data;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacoesJecContext.Data;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacoesJecContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.Contingencia.Movimentacao.DTOs;
using System.IO;
using System.Threading;

namespace Oi.Juridico.WebApi.V2.Areas.Contingencia.Movimentacao.Controllers
{
    [Route("relatorio-movimentacoes-jec")]
    [ApiController]
    public class RelatorioMovimentacoesJecController : ControllerBase
    {
        private RelatorioMovimentacoesJecContext _movimentacoesJecContext;
        private readonly ParametroJuridicoContext _parametroJuridico;
        private readonly ControleDeAcessoContext _db;
        //private readonly ILogger _logger;

        public RelatorioMovimentacoesJecController(ParametroJuridicoContext parametroJuridico, ControleDeAcessoContext db, RelatorioMovimentacoesJecContext movimentacoesJecContext)
        {
            _parametroJuridico = parametroJuridico;
            _movimentacoesJecContext = movimentacoesJecContext;
            _db = db;
            //_logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ListarAsync(CancellationToken ct, [FromQuery] DateTime? dataInicial, [FromQuery] DateTime? dataFinal, [FromQuery] int page = 0, [FromQuery] int size = 5)
        {
            var query = _movimentacoesJecContext.AgendMovimentacaoJec
                .AsNoTracking()
                .Select(x => new ListarJecResponse
                {
                    Id = x.Id,
                    DatAgendamento = x.DatAgendamento,
                    IniDataFechamento = x.IniDataFechamento,
                    IniNumMesesMediaHistorica = x.IniNumMesesMediaHistorica,
                    IniIndicadorFechamentoMensal = x.IniIndMensal,
                    IniPercentualHaircut = x.IniPercentualHaircut,
                    IniValCorteOutliers = x.IniValCorteOutliers,
                    IniIndFechamentoParcial = x.IniIndFechamentoParcial,
                    IniEmpresas = x.IniEmpresas,
                    FimDataFechamento = x.FimDataFechamento,
                    FimNumMesesMediaHistorica = x.FimNumMesesMediaHistorica,
                    FimIndicadorFechamentoMensal = x.FimIndMensal,
                    FimPercentualHaircut = x.FimPercentualHaircut,
                    FimValCorteOutliers = x.FimValCorteOutliers,
                    FimIndFechamentoParcial = x.FimIndFechamentoParcial,
                    FimEmpresas = x.FimEmpresas,
                    DatInicioExecucao = x.DatInicioExecucao,
                    DatFimExecucao = x.DatFimExecucao,
                    Status = (StatusMovimentacaoJec)x.Status,
                    MsgErro = x.MsgErro,
                    UsuarioId = x.UsrCodUsuario,
                    UsuarioNome = x.UsrCodUsuarioNavigation.NomeUsuario
                });

            if (dataInicial != null && dataFinal != null)
            {
                query = query.Where(x => x.DatAgendamento.Date >= dataInicial && x.DatAgendamento.Date <= dataFinal);
            }

            var total = await query.CountAsync(ct);
            var data = await query.OrderByDescending(x => x.DatAgendamento).Skip(page * size).Take(size).ToListAsync(ct);

            return Ok(new { data, total });
        }


        [HttpPost, Route("incluir-agendamento")]
        public async Task<IActionResult> IncluirAsync(CancellationToken ct, [FromBody] IncluirAgendamentoMovimentacaoJecRequest dto)
        {
            try
            {
                if (dto.IniDataFechamento >= dto.FimDataFechamento)
                {
                    return BadRequest("A data do fechamento inicial deve ser menor do que a data do fechamento final!");
                }

                var agendamento = new AgendMovimentacaoJec();
                agendamento.IdBaseMovIni = dto.IdBaseMovIni;
                agendamento.IniDataFechamento = dto.IniDataFechamento;
                agendamento.IniEmpresas = dto.IniEmpresas;
                agendamento.IniIndFechamentoParcial = dto.IniIndFechamentoParcial;
                agendamento.IniIndMensal = dto.IniIndMensal;
                agendamento.IniNumMesesMediaHistorica = dto.IniNumMesesMediaHistorica;
                agendamento.IniPercentualHaircut = dto.IniPercentualHaircut;
                agendamento.IniValCorteOutliers = dto.IniValCorteOutliers;
                agendamento.IdBaseMovFim = dto.IdBaseMovFim;
                agendamento.FimDataFechamento = dto.FimDataFechamento;
                agendamento.FimEmpresas = dto.FimEmpresas;
                agendamento.FimIndFechamentoParcial = dto.FimIndFechamentoParcial;
                agendamento.FimIndMensal = dto.FimIndMensal;
                agendamento.FimNumMesesMediaHistorica = dto.FimNumMesesMediaHistorica;
                agendamento.FimPercentualHaircut = dto.FimPercentualHaircut;
                agendamento.FimValCorteOutliers =dto.FimValCorteOutliers;
                agendamento.DatAgendamento = DateTime.Now;
                agendamento.Status = 0;
                agendamento.UsrCodUsuario = User.Identity!.Name;

                _movimentacoesJecContext.AgendMovimentacaoJec.Add(agendamento);
                await _movimentacoesJecContext.SaveChangesAsync(ct);

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
                var agendamento = _movimentacoesJecContext.AgendMovimentacaoJec.FirstOrDefault(a => a.Id == id);
                if (agendamento is null)
                {
                    return BadRequest("Agendamento não encontrado");
                }
                _movimentacoesJecContext.Remove(agendamento);
                await _movimentacoesJecContext.SaveChangesAsync(ct);

                return Ok("Excluido com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet, Route("listar-fechamentos")]
        public async Task<IActionResult> ObterFechamentosAsync(CancellationToken ct, [FromQuery] DateTime? dataInicial, [FromQuery] DateTime? dataFinal, [FromQuery] bool fechamentoMensal = false, [FromQuery] int page = 0, [FromQuery] int size = 5)
        {
            var query = _movimentacoesJecContext.FechamentoMovJec
                .AsNoTracking()
                .Where(x => _movimentacoesJecContext.ContingenciaJuizado.Any(y => y.EmpceDataFechamentoFim.Value.Date == x.DatFechamento.Date))
                .Select(fpm => new ObterFechamentosJecResponse
                {
                    Id = fpm.IdBaseMov,
                    DataFechamento = fpm.DatFechamento.Date,
                    PercHaircut = fpm.PercHaircut,
                    ValorCorteOutliers = fpm.ValCorteOutliers,
                    NumeroMeses = fpm.NumMesesFechamento,
                    Empresas = "TODAS AS EMPRESAS", // fpm.CodEmpresaCentralizadoraNavigation.Nome,
                    IndicaFechamentoParcial = fpm.DatFechamento.Date == fpm.DatSolFec.Date,
                    IndicaFechamentoMensal = fpm.IndFechamentoMensal
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
            var data = await query.OrderByDescending(x => x.DataFechamento).Skip(page * size).Take(size).ToListAsync(ct);

            return Ok(new { data, total });
        }

        [HttpGet("agendamentos/download/ArquivoBase/{agendamentoId}")]
        public async Task<IActionResult> ObterArquivoBaseCarregadoAsync(CancellationToken ct, int agendamentoId)
        {
            try
            {
                var nomeArquivo = await _movimentacoesJecContext
                    .AgendMovimentacaoJec
                    .AsNoTracking()
                    .Where(x => x.Id == agendamentoId)
                    .Select(x => x.NomArquivo)
                    .FirstAsync(ct);

                // obtém o parâmetro jurídico com o caminho do NAS onde o arquivo se encontra
                var nasArquivo = await _parametroJuridico.TratarCaminhoDinamicoArrayAsync(ParametrosJuridicos.DIR_SERV_REL_MOV_JUIZADO, nomeArquivo);

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
                //_logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

    }
}
