using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oi.Juridico.AddOn.Extensions.HttpContext;
using Oi.Juridico.Contextos.AtmJecContext.Data;
using Oi.Juridico.Contextos.AtmJecContext.Entities;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Atm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Contingencia.Atm
{
    [Route("contingencia/atm/jec")]
    [ApiController]
    [Authorize]
    public class AtmJecController : ApiControllerBase
    {
        IParametroJuridicoProvider _parametroJuridico;
        AtmJecContext _atmJecContext;
        IUsuarioAtualProvider _usuarioAtual;

        public AtmJecController(IParametroJuridicoProvider parametroJuridico, AtmJecContext atmJecContext, IUsuarioAtualProvider usuarioAtual)
        {
            _parametroJuridico = parametroJuridico;
            _atmJecContext = atmJecContext;
            _usuarioAtual = usuarioAtual;
        }

        [HttpGet("fechamentos")]
        public async Task<IActionResult> ObterFechamentosAsync()
        {
            var sw = new Stopwatch();
            sw.Start();

            var fechamentos = await _atmJecContext.VFechamentoAtmJec
                .AsNoTracking()
                .OrderByDescending(x => x.DataFechamento)
                .Select(x => new ObterFechamentosResponse
                {
                    Id = x.FajId,
                    MesAnoFechamento = x.MesAnoFechamento,
                    DataFechamento = x.DataFechamento,
                    NumeroDeMeses = x.NumMesesFechamento
                })
                .ToArrayAsync();

            sw.Stop();

            HttpContext.InsertTempoConsultaInResponse(sw);

            return Ok(fechamentos);
        }

        //[HttpGet("fechamentos/{fechamentoId}/indices")]
        //public IActionResult ObterIndicesDoFechamento([FromServices] IDatabaseContext database, [FromRoute] int fechamentoId)
        //{
        //    var fechamento = database.AgendamentosDeFechamentosAtmJec
        //        .Include(x => x.Indices)
        //        .AsNoTracking()
        //        .Where(x => x.Fechamento.Id == fechamentoId)
        //        .OrderByDescending(x => x.DataAgendamento)
        //        .FirstOrDefault();

        //    if (fechamento is null)
        //    {
        //        return Ok(new AgendamentoDeFechamentoAtmUfJec[0] { });
        //    }

        //    return Ok(fechamento.Indices);
        //}

        [HttpGet("fechamentos/ObterIndicesUltimoAgendamento")]
        public async Task<IActionResult> ObterIndicesUltimoAgendamentoAsync()
        {
            var sw = new Stopwatch();
            sw.Start();

            var indices = await _atmJecContext.AgendFechAtmUfJec
                .AsNoTracking()
                .GroupBy(x => x.Afaj)
                .OrderByDescending(x => x.Key.AfajId)
                .Take(1)
                .SelectMany(x => x.Select(i => new ObterIndicesUltimoAgendamentoResponse 
                {
                    Id = i.AfajId, 
                    Estado = i.CodEstado,
                    IndiceId = i.CodIndice,
                    Acumulado = i.IndAcumulado == "S" ? true : false
                }))
                .ToArrayAsync();

            sw.Stop();

            HttpContext.InsertTempoConsultaInResponse(sw);

            return Ok(indices);
        }

        [HttpGet("agendamentos")]
        public async Task<IActionResult> ObterAgendamentosAsync([FromQuery] int page = 0, [FromQuery] int size = 5)
        {
            var query = _atmJecContext.AgendFechAtmJec
                .AsNoTracking()
                .OrderByDescending(x => x.DatAgendamento)
                .Select(x => new ObterAgendamentosResponse
                {
                    Id = x.AfajId,
                    CodigoUsuario = x.UsrCodUsuario,
                    NomeUsuario = x.UsrCodUsuarioNavigation.NomeUsuario,
                    DataAgendamento = x.DatAgendamento,
                    DataFechamento = x.Faj.DataFechamento,
                    Erro = x.MsgErro,
                    FimDaExecucao = x.DatFimExecucao,
                    InicioDaExecucao = x.DatInicioExecucao,
                    MesAnoFechamento = x.Faj.MesAnoFechamento,
                    NumeroDeMeses = x.Faj.NumMesesFechamento,
                    Status = x.Status
                });              

            int total = await query.CountAsync();

            var sw = new Stopwatch();
            sw.Start();

            var data = await query.Skip(page * size).Take(size).ToArrayAsync();

            sw.Stop();

            HttpContext.InsertTempoConsultaInResponse(sw);

            return Ok(new { data, total });
        }

        [HttpPost("agendamentos/{fechamentoId}")]
        public async Task<IActionResult> AgendarAsync(int fechamentoId, [FromBody] IEnumerable<AgendarRequest> models)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo inválido.");
            }

            var fechamento = await _atmJecContext.FechamentoAtmJec.AsNoTracking().FirstOrDefaultAsync(x => x.FajId == fechamentoId);
            if (fechamento is null)
            {
                return BadRequest($"Fechamento '{ fechamentoId }' não encontrado.");
            }

            // valida se todos os índices estão cadastrados
            foreach (var item in models.GroupBy(x => x.Indice))
            {
                if (!await _atmJecContext.Indice.AnyAsync(x => x.CodIndice == item.Key))
                {
                    return BadRequest($"Índice '{ item.Key }' não encontrado.");
                }
            }

            var indices = await _atmJecContext.Indice
                .AsNoTracking()
                .Where(x => models.GroupBy(m => m.Indice).Select(m => m.Key).Contains(x.CodIndice))
                .Select(x => new
                {
                    x.CodIndice,
                    x.IndAcumulado
                })
                .ToArrayAsync();

            var agendamento = new AgendFechAtmJec();
            agendamento.FajId = fechamento.FajId;
            agendamento.DatAgendamento = DateTime.Now;
            agendamento.UsrCodUsuario = _usuarioAtual.Login;
            agendamento.Status = 0;

            foreach (var model in models)
            {
                var agendamentoUf = new AgendFechAtmUfJec();
                agendamentoUf.CodEstado = model.Estado;
                agendamentoUf.CodIndice = model.Indice;
                agendamentoUf.IndAcumulado = indices.First(x => x.CodIndice == model.Indice).IndAcumulado;
                agendamento.AgendFechAtmUfJec.Add(agendamentoUf);
            }

            _atmJecContext.AgendFechAtmJec.Add(agendamento);
            await _atmJecContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("indices")]
        public async Task<IActionResult> ObterIndicesAsync()
        {
            var sw = new Stopwatch();
            sw.Start();

            var result = await _atmJecContext.Indice
                .AsNoTracking()
                .OrderByDescending(x => x.NomIndice)
                .Select(x => new ObterIndicesResponse
                {
                    Id = x.CodIndice,
                    Descricao = x.NomIndice,
                    CodigoTipoIndice = x.CodTipoIndice,
                    CodigoValorIndice = x.CodValorIndice,
                    Acumulado = x.IndAcumulado == "S" ? true : false
                })
                .ToArrayAsync();

            sw.Stop();

            HttpContext.InsertTempoConsultaInResponse(sw);

            return Ok(result);
        }

        [HttpGet("agendamentos/download/ArquivoBase/{agendamentoId}")]
        public async Task<IActionResult> ObterArquivoBaseCarregadoAsync(int agendamentoId)
        {
            try
            {
                // obtém o parâmetro jurídico com o caminho do NAS onde o arquivo se encontra
                var parametroDirNasRelatorioAtmJec = _parametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_RELATORIO_ATM_JEC).First();

                if (!Directory.Exists(parametroDirNasRelatorioAtmJec))
                {
                    return BadRequest("Diretório NAS não encontrado");
                }

                // obtém a lista de arquivos procurando pelo Id da solicitação do fechamento
                var arquivos = Directory.GetFiles(parametroDirNasRelatorioAtmJec, $"ATM_JEC_Base_{agendamentoId}_*");

                // verifica se o arquivo existe
                if (!arquivos.Any())
                {
                    return BadRequest("Arquivo não encontrado.");
                }

                var nomeArquivo = Path.GetFileName(arquivos[0]);
                var dados = await System.IO.File.ReadAllBytesAsync(arquivos[0]);
                return File(dados, "application/zip", nomeArquivo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("agendamentos/download/ArquivoRelatorio/{agendamentoId}")]
        public async Task<IActionResult> ObterArquivoRelatorioCarregadoAsync(int agendamentoId)
        {
            try
            {
                // obtém o parâmetro jurídico com o caminho do NAS onde o arquivo se encontra
                var parametroDirNasRelatorioAtmJec = _parametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_RELATORIO_ATM_JEC).First();

                if (!Directory.Exists(parametroDirNasRelatorioAtmJec))
                {
                    return BadRequest("Diretório NAS não encontrado");
                }

                // obtém a lista de arquivos procurando pelo Id da solicitação do fechamento
                var arquivos = Directory.GetFiles(parametroDirNasRelatorioAtmJec, $"ATM_JEC_Rel_ATM_{agendamentoId}_*");

                // verifica se o arquivo existe
                if (!arquivos.Any())
                {
                    return BadRequest("Arquivo não encontrado.");
                }

                var nomeArquivo = Path.GetFileName(arquivos[0]);
                var dados = await System.IO.File.ReadAllBytesAsync(arquivos[0]);
                return File(dados, "application/zip", nomeArquivo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}