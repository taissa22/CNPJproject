using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oi.Juridico.Contextos.AtmPexContext.Data;
using Oi.Juridico.Contextos.AtmPexContext.Entities;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Atm.AtmPex;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Contingencia.Atm
{
    [Route("contingencia/atm/pex")]
    [ApiController]
    [Authorize]
    public class AtmPexController : ApiControllerBase
    {
        IParametroJuridicoProvider _parametroJuridico;
        private AtmPexContext _atmPexContext;
        private IUsuarioAtualProvider _usuarioAtual;

        public AtmPexController(IParametroJuridicoProvider parametroJuridico, AtmPexContext atmPexContext, IUsuarioAtualProvider usuarioAtual)
        {
            _parametroJuridico = parametroJuridico;
            _atmPexContext = atmPexContext;
            _usuarioAtual = usuarioAtual;
        }

        [HttpGet("fechamentos")]
        public async Task<IActionResult> ObterFechamentos()
        {
            var dtos = await (from fpm in _atmPexContext.FechamentoPexMedia
                              join emp in _atmPexContext.EmpresasCentralizadoras
                              on fpm.CodEmpresaCentralizadora equals emp.Codigo
                              join usu in _atmPexContext.AcaUsuario
                              on fpm.CodUsuario equals usu.CodUsuario
                              join sfc in _atmPexContext.SolicFechamentoCont
                              on fpm.CodSolicFechamentoCont equals sfc.CodSolicFechamentoCont
                              select new FechamentoPexMediaResponse
                              {
                                  CodSolicFechamentoCont = fpm.CodSolicFechamentoCont,
                                  DataExecucao = fpm.DatGeracao,
                                  DataFechamento = fpm.DatFechamento,
                                  PercentualHaircut = fpm.PerHaircut,
                                  NomeUsuario = usu.NomeUsuario,
                                  IndAplicarHaircut = fpm.IndAplicarHaircutProcGar,
                                  NumeroMeses = fpm.NroMesesMediaHistorica,
                                  MultDesvioPadrao = fpm.ValMultDesvioPadrao,
                                  Empresas = emp.Nome,
                                  DataAgendamento = sfc.DatUltimoAgend != null ? sfc.DatUltimoAgend : sfc.DataCadastro
                              }).ToListAsync();

            var resultado = new List<FechamentoPexMediaResponse>();

            dtos.ForEach(d =>
            {
                var dto = resultado.FirstOrDefault(r => r.CodSolicFechamentoCont == d.CodSolicFechamentoCont && r.DataFechamento == d.DataFechamento);
                if (dto == null)
                {
                    resultado.Add(d);
                }
                else
                {
                    dto.Empresas += ", " + d.Empresas;
                    if (d.DataExecucao > dto.DataExecucao)
                    {
                        dto.DataExecucao = d.DataExecucao;
                    }
                }
            });

            return Ok(resultado.OrderByDescending(r => r.DataFechamento).ThenByDescending(r => r.DataExecucao));
        }

        [HttpGet("fechamentos/indices")]
        public async Task<IActionResult> ObterIndicesDoFechamentoAsync()
        {
            var indices = await _atmPexContext.AtmIndiceUfPadrao
                .AsNoTracking()
                .Select(x => new ObterIndicesDoFechamentoResponse
                {
                    CodEstado = x.CodEstado,
                    CodIndice = x.CodIndice,
                })
                .ToArrayAsync();

            if (indices is null)
            {
                return Ok(new ObterIndicesDoFechamentoResponse[0] { });
            }

            return Ok(indices);
        }

        [HttpGet("agendamentos")]
        public async Task<IActionResult> ObterAgendamentosAsync([FromQuery] int page = 0, [FromQuery] int size = 5)
        {
            var query = _atmPexContext.AgendFechAtmPex
                .AsNoTracking()
                .OrderByDescending(x => x.DatAgendamento)
                .Select(x => new ObterAgendamentosResponse
                {
                    AgendamentoId = x.Id,
                    CodSolicFechamento = x.CodSolicFechamento,
                    DataAgendamento = x.DatAgendamento,
                    DataFechamento = x.DatFechamento,
                    DataInicioExecucao = x.DatInicioExecucao,
                    DataFimExecucao = x.DatFimExecucao,
                    CodigoUsuario = x.UsrCodUsuario,
                    NomeUsuario = x.UsrCodUsuarioNavigation.NomeUsuario,
                    NumeroDeMeses = x.NroMesesMediaHistorica,
                    ValDesvioPadrao = x.ValMultDesvioPadrao,
                    Status = x.Status,
                    MsgErro = x.MsgErro,
                });

            var total = await query.CountAsync();
            var data = await query.Skip(page * size).Take(size).ToListAsync();

            data.ForEach(d =>
            {
                d.fechamentoPexMediaResponse = buscarFechamentoPorCodSolicEData(d.CodSolicFechamento, d.DataFechamento);
            });

            return Ok(new { data, total });
        }
        private FechamentoPexMediaResponse buscarFechamentoPorCodSolicEData(int? codSolic, DateTime dataFechamento)
        {
            var dtos = (from fpm in _atmPexContext.FechamentoPexMedia
                        join emp in _atmPexContext.EmpresasCentralizadoras
                        on fpm.CodEmpresaCentralizadora equals emp.Codigo
                        join usu in _atmPexContext.AcaUsuario
                        on fpm.CodUsuario equals usu.CodUsuario
                        join sfc in _atmPexContext.SolicFechamentoCont
                        on fpm.CodSolicFechamentoCont equals sfc.CodSolicFechamentoCont
                        where fpm.CodSolicFechamentoCont == codSolic && fpm.DatFechamento == dataFechamento
                        select new FechamentoPexMediaResponse
                        {
                            CodSolicFechamentoCont = fpm.CodSolicFechamentoCont,
                            DataExecucao = fpm.DatGeracao,
                            DataFechamento = fpm.DatFechamento,
                            PercentualHaircut = fpm.PerHaircut,
                            NomeUsuario = usu.NomeUsuario,
                            IndAplicarHaircut = fpm.IndAplicarHaircutProcGar,
                            NumeroMeses = fpm.NroMesesMediaHistorica,
                            MultDesvioPadrao = fpm.ValMultDesvioPadrao,
                            Empresas = emp.Nome,
                            DataAgendamento = sfc.DatUltimoAgend != null ? sfc.DatUltimoAgend : sfc.DataCadastro
                        }).ToList();

            FechamentoPexMediaResponse resultado = null;

            dtos.ForEach(d =>
            {
                if (resultado == null)
                {
                    resultado = d;
                }
                else
                {
                    resultado.Empresas += ", " + d.Empresas;
                    if (d.DataExecucao > resultado.DataExecucao)
                    {
                        resultado.DataExecucao = d.DataExecucao;
                    }
                }
            });

            return resultado;
        }

        [HttpDelete("agendamentos/excluir/{agendamentoId}")]
        public async Task<IActionResult> RemoverAsync(int agendamentoId)
        {
            var agendamento = await _atmPexContext.AgendFechAtmPex.Include(x => x.AgendFechAtmPexUf).FirstOrDefaultAsync(x => x.Id == agendamentoId);

            if (agendamento != null)
            {
                _atmPexContext.AgendFechAtmPexUf.RemoveRange(agendamento.AgendFechAtmPexUf);
                _atmPexContext.AgendFechAtmPex.Remove(agendamento);
                await _atmPexContext.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("agendamentos/{codSolicfechamento}/{dataFechamento}/{bloqueiaValidacaoIndice}")]
        public async Task<IActionResult> AgendarAsync(int codSolicfechamento, DateTime dataFechamento , bool bloqueiaValidacaoIndice, [FromBody] IEnumerable<AgendarRequest> models)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo inválido.");
            }

            var fechamento = await _atmPexContext.FechamentoPexMedia.AsNoTracking().Select(x => new FechamentoPexMedia()
            {
                Id = x.Id,
                DatFechamento = x.DatFechamento,
                NroMesesMediaHistorica = x.NroMesesMediaHistorica,
                IndMensal = x.IndMensal,
                DatIndMensal = x.DatIndMensal,
                DatGeracao = x.DatGeracao,
                CodUsuario = x.CodUsuario,
                CodEmpresaCentralizadora = x.CodEmpresaCentralizadora,
                PerHaircut = x.PerHaircut,
                IndAplicarHaircutProcGar = x.IndAplicarHaircutProcGar,
                ValMultDesvioPadrao = x.ValMultDesvioPadrao,
                CodSolicFechamentoCont = x.CodSolicFechamentoCont,
                CodUsuarioNavigation = x.CodUsuarioNavigation
            }).FirstOrDefaultAsync(x => x.CodSolicFechamentoCont == codSolicfechamento && x.DatFechamento == dataFechamento);
            if (fechamento is null)
            {
                return BadRequest($"Fechamento '{ codSolicfechamento }' não encontrado.");
            }


            var indicesNaoEncontrados = models.Where(m => !_atmPexContext.CotacaoIndice.Any(c =>
            c.DataCotacao.Year == fechamento.DatFechamento.Year &&
            c.DataCotacao.Month == fechamento.DatFechamento.Month &&
            c.IndiceId == m.CodIndice
            )).ToList();

            if (indicesNaoEncontrados.Any() && !bloqueiaValidacaoIndice)
            {
                var indices = new List<int>();
                foreach (var indiceNaoEncontrado in indicesNaoEncontrados)
                {
                    if (!indices.Any(i => i == indiceNaoEncontrado.CodIndice)) indices.Add(indiceNaoEncontrado.CodIndice);
                }
                return BadRequest(indices);
            }


            // valida se todos os índices estão cadastrados
            foreach (var item in models)
            {
                if (!await _atmPexContext.Indice.AnyAsync(x => x.CodIndice == item.CodIndice))
                {
                    return BadRequest($"Índice '{ item.CodIndice }' não encontrado.");
                }
            }

            var agendamento = new AgendFechAtmPex();
            agendamento.CodSolicFechamento = fechamento.CodSolicFechamentoCont.Value;
            agendamento.ValMultDesvioPadrao = fechamento.ValMultDesvioPadrao;
            agendamento.NroMesesMediaHistorica = fechamento.NroMesesMediaHistorica;
            agendamento.DatFechamento = fechamento.DatFechamento;
            agendamento.DatAgendamento = DateTime.Now;
            agendamento.UsrCodUsuario = _usuarioAtual.Login;
            agendamento.Status = 0;
            agendamento.AgendFechAtmPexUf = new List<AgendFechAtmPexUf>();

            foreach (var model in models)
            {
                var agendamentoUf = new AgendFechAtmPexUf();
                agendamentoUf.CodEstado = model.CodEstado;
                agendamentoUf.CodIndice = model.CodIndice;
                agendamentoUf.CodTipoProcesso = TipoProcesso.PEX.Id;
                agendamento.AgendFechAtmPexUf.Add(agendamentoUf);
            }

            _atmPexContext.AgendFechAtmPex.Add(agendamento);

            // altera os indíces na tabela padrão
            var indicesDefault = await _atmPexContext.AtmIndiceUfPadrao.ToArrayAsync();

            foreach (var item in models)
            {
                indicesDefault.First(x => x.CodEstado == item.CodEstado).CodIndice = item.CodIndice;
            }

            await _atmPexContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("indices")]
        public async Task<IActionResult> ObterIndicesAsync()
        {
            var result = await _atmPexContext.Indice
                .AsNoTracking()
                .Select(x => new ObterIndicesResponse
                {
                    CodIndice = x.CodIndice,
                    Descricao = x.NomIndice,
                    Acumulado = x.IndAcumulado == "S" ? true : false
                })
                .ToArrayAsync();

            return Ok(result);
        }

        [HttpGet("agendamentos/download/ArquivoBase/{agendamentoId}")]
        public async Task<IActionResult> ObterArquivoBaseCarregadoAsync(int agendamentoId)
        {
            try
            {
                // obtém o parâmetro jurídico com o caminho do NAS onde o arquivo se encontra
                var parametroDirNasRelatorioAtmPex = _parametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_RELATORIO_ATM_PEX).First();

                if (!Directory.Exists(parametroDirNasRelatorioAtmPex))
                {
                    return BadRequest("Diretório NAS não encontrado");
                }

                // obtém a lista de arquivos procurando pelo Id da solicitação do fechamento
                var arquivos = Directory.GetFiles(parametroDirNasRelatorioAtmPex, $"ATM_Pex_Base_{agendamentoId}_*");

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
                var parametroDirNasRelatorioAtmPex = _parametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_RELATORIO_ATM_PEX).First();

                if (!Directory.Exists(parametroDirNasRelatorioAtmPex))
                {
                    return BadRequest("Diretório NAS não encontrado");
                }

                // obtém a lista de arquivos procurando pelo Id da solicitação do fechamento
                var arquivos = Directory.GetFiles(parametroDirNasRelatorioAtmPex, $"ATM_Pex_Rel_ATM_{agendamentoId}_*");

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