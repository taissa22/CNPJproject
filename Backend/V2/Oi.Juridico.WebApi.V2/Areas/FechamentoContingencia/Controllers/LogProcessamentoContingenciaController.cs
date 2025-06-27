using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oi.Juridico.AddOn.Extensions.Enum;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.FechamentoContingenciaContext.Data;
using Oi.Juridico.Contextos.V2.FechamentoContingenciaContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ControleDeAcesso.Controllers;
using Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.Models;
using Oi.Juridico.WebApi.V2.DTOs.Contingencia.Fechamento;
using Perlink.Oi.Juridico.Infra.Entities;
using SixLabors.ImageSharp;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;

namespace Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogProcessamentoContingenciaController : ControllerBase
    {
        private FechamentoContingenciaContext _db;
        private readonly ILogger<AccountsController> _logger;

        public LogProcessamentoContingenciaController(FechamentoContingenciaContext db, ILogger<AccountsController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("obter-log")]
        public async Task<IActionResult> ObterLogAsync(CancellationToken ct, [FromQuery] IList<short> filtro, [FromQuery] string? ordem, [FromQuery] DateTime dataInicial, [FromQuery] DateTime datafinal, [FromQuery] bool asc, [FromQuery] int page = 0, [FromQuery] int size = 8)
        {
            var query = from ecafa in _db.EmpCentAgendamFechAuto.AsNoTracking()
                        join sfc in _db.SolicFechamentoCont on ecafa.CodSolicFechamentoCont equals sfc.CodSolicFechamentoCont
                        join ec in _db.EmpresasCentralizadoras on ecafa.IdEmpCentralizadora equals ec.Codigo
                        select new
                        {
                            Ecafa = ecafa,
                            Sfc = sfc,
                            Ec = ec,
                            // campo para ordenação pelo tipo de fechamento
                            TipoFechamento = ecafa.TipoFechamento == 1 ? 10 :
                                             ecafa.TipoFechamento == 50 ? 20 :
                                             ecafa.TipoFechamento == 6 ? 30 :
                                             ecafa.TipoFechamento == 49 ? 40 :
                                             ecafa.TipoFechamento == 51 ? 50 :
                                             ecafa.TipoFechamento == 7 ? 60 : 0
                        };

            List<int?> tiposFechamento = new();

            #region Filtro
            if (filtro.Count > 0)
            {
                if (filtro.Contains(1))
                {
                    tiposFechamento.Add((short)TipoFechamentoContingenciaEnum.Civel_Consumidor);
                    tiposFechamento.Add((short)TipoFechamentoContingenciaEnum.Civel_Consumidor_por_Media);
                }
                if (filtro.Contains(6))
                {
                    tiposFechamento.Add((short)TipoFechamentoContingenciaEnum.Civel_Estrategico);
                }
                if (filtro.Contains(49))
                {
                    tiposFechamento.Add((short)TipoFechamentoContingenciaEnum.Juizado_Especial);
                }
                if (filtro.Contains(7))
                {
                    tiposFechamento.Add((short)TipoFechamentoContingenciaEnum.Trabalhista);
                }
                if (filtro.Contains(51))
                {
                    tiposFechamento.Add((short)TipoFechamentoContingenciaEnum.PEX_por_Media);
                }
                query = query.Where(x => tiposFechamento.Contains(x.Ecafa.TipoFechamento));

                if (filtro.Contains(10))
                {
                    query = query.Where(x => x.Ecafa.Data >= dataInicial && x.Ecafa.Data <= datafinal);
                }
                if (filtro.Contains(11))
                {
                    query = query.Where(x => x.Ecafa.StatusAgendamento != 2 && x.Ecafa.StatusAgendamento != 4 && x.Ecafa.StatusAgendamento != 5 && x.Ecafa.StatusAgendamento != 6);
                }
            }
            #endregion

            #region Ordenacao
            switch (ordem)
            {
                case "empresa":
                    if (asc)
                        query = query.OrderBy(x => x.Ec.Nome != null).ThenBy(a => a.Ec.Nome).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    else
                        query = query.OrderByDescending(a => a.Ec.Nome).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    break;
                case "parametro":
                    if (asc)
                        query = query.OrderBy(x => x.Ecafa.DataFechamento).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    else
                        query = query.OrderByDescending(a => a.Ecafa.DataFechamento).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    break;
                case "fechamento":
                    if (asc)
                        query = query.OrderBy(x => x.Sfc.IndFechamentoMensal != null).ThenBy(x => x.Sfc.IndFechamentoMensal).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    else
                        query = query.OrderByDescending(x => x.Sfc.IndFechamentoMensal).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    break;
                case "agendado":
                    if (asc)
                        query = query.OrderBy(x => x.Ecafa.Data != null).ThenBy(x => x.Ecafa.Data).ThenBy(x => x.Ecafa.UsuarioNome).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    else
                        query = query.OrderByDescending(x => x.Ecafa.Data).ThenByDescending(x => x.Ecafa.UsuarioNome).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    break;
                case "execucao":
                    if (asc)
                        query = query.OrderBy(a => a.Ecafa.DataInicioExecucao).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    else
                        query = query.OrderByDescending(a => a.Ecafa.DataInicioExecucao).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    break;
                case "situacao":
                    if (asc)
                        query = query.OrderBy(x => x.Ecafa.StatusAgendamento != null).ThenBy(a => a.Ecafa.StatusAgendamento).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    else
                        query = query.OrderByDescending(a => a.Ecafa.StatusAgendamento).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    break;
                case "tipo":
                default:
                    if (asc)
                        query = query.OrderBy(a => a.TipoFechamento).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    else
                        query = query.OrderByDescending(a => a.TipoFechamento).ThenBy(x => x.Ecafa.IdEmpCentAgendamFechAuto);
                    break;
            }

            #endregion


            var paginado = await query
                            .Skip(page * size).Take(size)
                            .ToArrayAsync(ct);

            var total = query.CountAsync(ct);

            var log = (from x in paginado
                       select new LogProcessamentoContingenciaModel
                       {
                           Id = x.Ecafa.IdEmpCentAgendamFechAuto,
                           TipoFechamento = ((TipoFechamentoContingenciaEnum)x.Ecafa.TipoFechamento!).ToDescription(),
                           EmpresaCentralizadora = x.Ec.Nome,
                           Parametros = Parametros(x.Ecafa, x.Sfc),
                           FechamentoMes = x.Sfc.IndFechamentoMensal == "S" ? "Sim" : "Não",
                           AgendadoPara = String.Format("{0} por {1}", x.Ecafa.Data!.Value.ToString("dd/MM"), x.Ecafa.UsuarioNome),
                           Execucao = Duration(x.Ecafa),
                           Situacao = ((StatusAgendamentoContingenciaEnum)x.Ecafa.StatusAgendamento!).ToDescription(),
                           MensagemErro = x.Ecafa.Error
                       });

            return Ok(new { total, log });
        }

        [HttpDelete("excluir-processo")]
        public async Task<IActionResult> ExcluirProcessoAsync(CancellationToken ct, int id)
        {
            try
            {
                var processo = _db.EmpCentAgendamFechAuto.FirstOrDefault(a => a.IdEmpCentAgendamFechAuto == id);

                if (processo is null)
                {
                    _logger.LogError($"Processo {id} não encontado");
                    return BadRequest("Processo não encontrado");
                }
                _db.Remove(processo);
                await _db.SaveChangesAsync(ct);

                return Ok("Excluido com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        #region Metodos
        private string Parametros(EmpCentAgendamFechAuto ecafa, SolicFechamentoCont sfc)
        {
            var parametro = String.Format("Mês: {0}/{1}, Data: {2}", ecafa.Mes?.ToString("00"), ecafa.Ano, ecafa.DataFechamento?.ToString("dd/MM/yyyy"));
            if (ecafa.TipoFechamento == 49)
            {
                parametro += String.Format(" Nº Meses: {0}", ecafa.NumeroMeses);
                parametro += String.Format(" Valor de Corte de Outliers: R$ {0}", ecafa.ValorCorte?.ToString("N", CultureInfo.GetCultureInfo("pt-BR")));
                if (!string.IsNullOrEmpty(ecafa.ObsValorCorte))
                    parametro += String.Format(" Observação: {0}", ecafa.ObsValorCorte);
                parametro += String.Format(" Valores Pagos para a Média Móvel: {0}", ((TipoDeDataDeMediaMovelEnum)ecafa.TipoDataMediaMovel!).ToDescription());
                parametro += String.Format(" %Haircut: {0}", ecafa.PercentualHaircut.ToString());
            }
            if (ecafa.TipoFechamento == 7)
            {
                parametro += String.Format(" Nº Meses: {0}", ecafa.NumeroMeses);
                parametro += String.Format(" Gerar Base: {0}", sfc.IndGerarBaseDadosFec == "S" ? "Sim" : "Não");
            }
            if (ecafa.TipoFechamento == 50)
            {
                parametro += String.Format(" N Meses: {0}, %Haircut: {1}", ecafa.NumeroMeses, String.Format("{0:0.0}", ecafa.PercentualHaircut == null ? 0 : ecafa.PercentualHaircut.Value));
            }
            if (ecafa.TipoFechamento == 1)
            {
                parametro += String.Format(" ,%Haircut: {0:0.0}", ecafa.PercentualHaircut == null || ecafa.PercentualHaircut == 0 ? "N/A" : ecafa.PercentualHaircut.Value.ToString());
            }
            if (ecafa.TipoFechamento == 51)
            {
                parametro += String.Format(" Nº Meses: {0}", ecafa.NumeroMeses);
                if (sfc.PercentualHaircut.HasValue)
                {
                    parametro += String.Format(", Haircut {0}% ", sfc.PercentualHaircut.Value.ToString("#,##0.00", new CultureInfo("pt-br")).Replace(",00", ""));
                }
                if (sfc.IndAplicarHaircutProcGar == "S")
                {
                    parametro += "(Aplicar também haircut em processos com garantia) ";
                }
                else
                {
                    parametro += "(Não aplicar haircut em processos com garantia) ";
                }
                if (sfc.MultDesvioPadrao.HasValue)
                {
                    parametro += String.Format("{0} Desvio Padrão", sfc.MultDesvioPadrao.Value.ToString("#,##0.00", new CultureInfo("pt-br")).Replace(",00", ""));
                }
            }
            if (ecafa.ValAjusteDesvioPadrao.HasValue || ecafa.ValPercentProcOutliers.HasValue)
            {
                parametro += ", Aplicar Exclusão de Outliers: ";
                if (ecafa.ValAjusteDesvioPadrao.HasValue)
                {
                    parametro += String.Format("{0}x o Desvio Padrão", ecafa.ValAjusteDesvioPadrao.Value.ToString("#,##0.00", new CultureInfo("pt-br")).Replace(",00", ""));
                }
                else
                {
                    if (ecafa.ValPercentProcOutliers.HasValue)
                    {
                        parametro += String.Format("{0}% dos Processos", ecafa.ValPercentProcOutliers.Value.ToString("#,##0.00", new CultureInfo("pt-br")).Replace(",00", ""));
                    }
                }
            }
            else
            {
                if (ecafa.TipoFechamento == 7)
                {
                    parametro += ", Não Aplicar Exclusão de Outlier";
                }
            }
            return parametro;
        }

        private string Duration(EmpCentAgendamFechAuto ecafa)
        {
            if (ecafa.DataInicioExecucao.HasValue && ecafa.DataFimExecucao.HasValue)
            {
                var diff = ecafa.DataFimExecucao.Value.Subtract(ecafa.DataInicioExecucao.Value);
                return String.Format(@"{0} às {1} Duração {2}", ecafa.DataInicioExecucao.Value.ToString("dd/MM HH:mm"), ecafa.DataFimExecucao.Value.ToString("HH:mm"), diff.ToString());
            }
            if (ecafa.DataInicioExecucao.HasValue && !ecafa.DataFimExecucao.HasValue)
            {
                return String.Format("Iniciado em: {0}", ecafa.DataInicioExecucao.Value.ToString("dd/MM HH:mm"));
            }
            return "Não iniciado";
        }

        #endregion
    }
}
