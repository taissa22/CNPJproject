using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.Data;
using Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.DTO;
using Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.Entities;
using Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.Enums;
using Perlink.Oi.Juridico.Application.Relatorios.Contingencia.Interface;
using Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.DTO;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Movimentacoes;
using Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Movimentacoes.MovimentacoesPex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.Areas.Relatorios.Contingencia
{
    [Route("relatorio-movimentacoes-pex")]
    [ApiController]
    [Authorize]
    public class MovimentacoesPexController : ApiControllerBase
    {
        private RelatorioMovimentacoesPexContext _movimentacoesPexContext;
        private IUsuarioAtualProvider _usuarioAtual;
        IParametroJuridicoProvider _parametroJuridico;

        public MovimentacoesPexController(IParametroJuridicoProvider parametroJuridico, RelatorioMovimentacoesPexContext movimentacoesPexContext, IUsuarioAtualProvider usuarioAtual)
        {
            _parametroJuridico = parametroJuridico;
            _movimentacoesPexContext = movimentacoesPexContext;
            _usuarioAtual = usuarioAtual;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int page = 0, [FromQuery] int size = 5)
        {
            var query = _movimentacoesPexContext.AgendMovimentacaoPex
                .AsNoTracking()
                .OrderByDescending(x => x.DatAgendamento)
                .Select(x => new ObterMovimentacoesResponse
                {
                    Id                      = x.Id,
                    DatAgendamento          = x.DatAgendamento ,
                    FechamentoPexMediaIniCodSolic = x.FechamentoPexMediaIniCodSolic ,
                    FechamentoPexMediaFimCodSolic = x.FechamentoPexMediaFimCodSolic ,
                    DatFechamentoIni        = x.DatFechamentoIni ,
                    DatFechamentoFim        = x.DatFechamentoFim ,
                    DatInicioExecucao       = x.DatInicioExecucao,
                    DatFimExecucao          = x.DatFimExecucao,
                    Status                  = (StatusMovimentacaoPex)x.Status,
                    MsgErro                 = x.MsgErro ,
                    UsuarioId               = x.UsuarioId ,
                    UsuarioNome             = x.Usuario.NomeUsuario
                });

            var total = await query.CountAsync();
            var data = await query.Skip(page * size).Take(size).ToListAsync();

            data.ForEach(d =>
            {
                d.FechamentoIni = buscarFechamentoPorCodSolicEData(d.FechamentoPexMediaIniCodSolic, d.DatFechamentoIni);
                d.FechamentoFim = buscarFechamentoPorCodSolicEData(d.FechamentoPexMediaFimCodSolic, d.DatFechamentoFim);
            });

            return Ok(new { data, total });
        }

    

        [HttpPost]
        public async Task<IActionResult> Incluir([FromBody] IncluirMovimentacoesPex dto)
        {
            try
            {

                if(dto.FechamentoPexMediaFimCodSolic == 0 || dto.FechamentoPexMediaIniCodSolic == 0)
                {
                    return BadRequest("Selecione os fechamentos inicial e final que irão compor o relatório!");
                }

                if(dto.DatFechamentoIni >= dto.DatFechamentoFim)
                {
                    return BadRequest("A data do fechamento inicial deve ser menor do que a data do fechamento final!");
                }

                var usuario = _usuarioAtual.ObterUsuario();
                var agendamento = AgendMovimentacaoPex.Adicionar(
                  DateTime.Now,
                  dto.FechamentoPexMediaIniCodSolic,
                  dto.FechamentoPexMediaFimCodSolic,
                  dto.DatFechamentoIni,
                  dto.DatFechamentoFim,
                  0,
                  usuario.Id
                  );

                _movimentacoesPexContext.AgendMovimentacaoPex.Add(agendamento);
                _movimentacoesPexContext.SaveChanges();

                return Ok("Adicionado com Sucesso.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }           
        }

        [HttpDelete]
        public async Task<IActionResult> Excluir(decimal id)
        {
            try
            {
                var agendamento = _movimentacoesPexContext.AgendMovimentacaoPex.FirstOrDefault(a => a.Id == id);
                if (agendamento is null)
                {
                    return BadRequest("Agendamento não encontrado");
                }
                _movimentacoesPexContext.Remove(agendamento);
                _movimentacoesPexContext.SaveChanges();

                return Ok("Excluido com sucesso!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private FechamentoPexMediaResponse buscarFechamentoPorCodSolicEData(int? codSolic, DateTime dataFechamento)
        {
            var dtos = (from fpm in _movimentacoesPexContext.FechamentoPexMedia
                             join emp in _movimentacoesPexContext.EmpresasCentralizadoras
                             on fpm.CodEmpresaCentralizadora equals emp.Codigo
                             join usu in _movimentacoesPexContext.AcaUsuario
                             on fpm.CodUsuario equals usu.CodUsuario
                             join sfc in _movimentacoesPexContext.SolicFechamentoCont
                             on fpm.CodSolicFechamentoCont equals sfc.CodSolicFechamentoCont
                        where fpm.CodSolicFechamentoCont == codSolic && fpm.DatFechamento == dataFechamento
                             select new FechamentoPexMediaResponse
                             {
                                 Id = fpm.CodSolicFechamentoCont,
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

        [HttpGet("agendamentos/download/ArquivoBase/{agendamentoId}")]
        public async Task<IActionResult> ObterArquivoBaseCarregadoAsync(int agendamentoId)
        {
            try
            {
                // obtém o parâmetro jurídico com o caminho do NAS onde o arquivo se encontra
                var RelatorioMovPex = _parametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_RELATORIO_MOV_PEX).First();

                if (!Directory.Exists(RelatorioMovPex))
                {
                    return BadRequest("Diretório NAS não encontrado");
                }

                // obtém a lista de arquivos procurando pelo Id da solicitação do fechamento
                var arquivos = Directory.GetFiles(RelatorioMovPex, $"REL_MOVIMENTACOES_PEX_{agendamentoId}_*");

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