using Oi.Juridico.Contextos.V2.FechamentoContingenciaContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ControleDeAcesso.Controllers;
using Oi.Juridico.AddOn.Extensions.Enum;
using System.Threading;
using Oi.Juridico.Contextos.V2.FechamentoContingenciaContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.DTOs;
using Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.Extensions;
using Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.Validator;
using FluentValidation.Results;
using System.Linq;

namespace Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FechamentoContingenciaController : ControllerBase
    {

        private FechamentoContingenciaContext _db;
        private readonly ILogger<AccountsController> _logger;

        public FechamentoContingenciaController(FechamentoContingenciaContext db, ILogger<AccountsController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("obter-agendamentos")]
        public async Task<IActionResult> ObterAgendamentos(CancellationToken ct, [FromQuery] IList<short> modulos, [FromQuery] string? ordem, [FromQuery] bool asc = true, [FromQuery] int page = 0, [FromQuery] int size = 10)
        {
            var query = from x in _db.SolicFechamentoCont.AsNoTracking()
                        join u in _db.AcaUsuario on x.CodUsuarioSolicitacao equals u.CodUsuario
                        where x.IndAtivo == "S"
                        && x.DatProximoAgend.HasValue && x.DatProximoAgend.Value.Date >= DateTime.Now.Date
                        && x.CodTipoFechamento.HasValue
                        && modulos.Contains(x.CodTipoFechamento.Value)
                        //&& !new byte?[] { 1 }.Contains(x.PeriodicidadeExecucao)
                        select new
                        {
                            SolicFechamentoCont = x,
                            NomeUsuario = u.NomeUsuario
                        };

            var agendamentos = await query
                .Skip(page * size).Take(size)
                .ToArrayAsync(ct);

            var countEmpresasCentralizadoras = await _db.EmpresasCentralizadoras.CountAsync(ct);

            var lista = (from x in agendamentos
                         let empresasCentralizadoras = _db.SolicFechContEmpCent.Where(e => e.CodSolicFechamentoCont == x.SolicFechamentoCont.CodSolicFechamentoCont).Select(e => new { Id = e.CodEmpresasCentralizadoras, e.CodEmpresasCentralizadorasNavigation.Nome }).ToArray()
                         select new
                         {
                             Id = x.SolicFechamentoCont.CodSolicFechamentoCont,
                             TipoFechamento = ((TipoFechamentoContingenciaEnum)x.SolicFechamentoCont.CodTipoFechamento!).ToDescription(),
                             TodasEmpresas = empresasCentralizadoras.Length == countEmpresasCentralizadoras,
                             Empresas = empresasCentralizadoras.Length == countEmpresasCentralizadoras ? "TODAS" : string.Join(", ", empresasCentralizadoras.Select(e => e.Nome)),
                             EmpresasTooltip = string.Join(", ", empresasCentralizadoras.Select(e => e.Nome)),
                             EmpresasCentralizadoras = empresasCentralizadoras,
                             x.SolicFechamentoCont.NumeroDeMeses,
                             PercentualHaircut = x.SolicFechamentoCont.PercentualHaircut.HasValue ? x.SolicFechamentoCont.PercentualHaircut.ToString() : "",
                             x.SolicFechamentoCont.ValCorteOutliers,
                             x.SolicFechamentoCont.ObsValCorteOutliers,
                             x.SolicFechamentoCont.ValAjusteDesvioPadrao,
                             x.SolicFechamentoCont.ValPercentProcOutliers,
                             x.SolicFechamentoCont.MultDesvioPadrao,
                             x.SolicFechamentoCont.IndAplicarHaircutProcGar,
                             x.SolicFechamentoCont.IndAgendRelMovAutomatico,
                             SolicitacaoExecucao = ObterSolicitacao(x.SolicFechamentoCont),
                             ProximaExecucao = x.SolicFechamentoCont.DatProximoAgend?.ToString("dd/MM/yyyy dddd") ?? "",
                             PeriodicidadeExecucao = ((PeriodicidadeExecucaoEnum)x.SolicFechamentoCont.PeriodicidadeExecucao!).ToDescription(),
                             // EXECUÇÃO IMEDIATA
                             x.SolicFechamentoCont.IndExecucaoImediata,
                             MesContabilEi = x.SolicFechamentoCont.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.ExecucaoImediata ? x.SolicFechamentoCont.MesContabil : null,
                             AnoContabilEi = x.SolicFechamentoCont.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.ExecucaoImediata ? x.SolicFechamentoCont.AnoContabil : null,
                             DataPreviaEi = x.SolicFechamentoCont.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.ExecucaoImediata ? (x.SolicFechamentoCont.DataPrevia).ToString() : "",
                             IndFechamentoMensalEi = x.SolicFechamentoCont.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.ExecucaoImediata ? x.SolicFechamentoCont.IndFechamentoMensal : "N",
                             IndGerarBaseDadosFecEi = x.SolicFechamentoCont.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.ExecucaoImediata ? x.SolicFechamentoCont.IndGerarBaseDadosFec : "N",
                             // DATA ESPECIFICA
                             x.SolicFechamentoCont.DataEspecifica,
                             MesContabilDE = x.SolicFechamentoCont.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.DataEspecifica ? x.SolicFechamentoCont.MesContabil : null,
                             AnoContabilDE = x.SolicFechamentoCont.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.DataEspecifica ? x.SolicFechamentoCont.AnoContabil : null,
                             DataPreviaDE = x.SolicFechamentoCont.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.DataEspecifica ? x.SolicFechamentoCont.DataPrevia.ToString() : "",
                             IndicaFechamentoMensalDE = x.SolicFechamentoCont.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.DataEspecifica ? x.SolicFechamentoCont.IndFechamentoMensal : "N",
                             IndGerarBaseDadosFecDE = x.SolicFechamentoCont.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.DataEspecifica ? x.SolicFechamentoCont.IndGerarBaseDadosFec : "N",
                             // DIARIA
                             DataDiariaIniDI = x.SolicFechamentoCont.DataDiariaIni.HasValue ? x.SolicFechamentoCont.DataDiariaIni.ToString() : "",
                             DataDiariaFimDI = x.SolicFechamentoCont.DataDiariaFim.HasValue ? x.SolicFechamentoCont.DataDiariaFim.ToString() : "",
                             IndicaSomenteDiaUtilDI = x.SolicFechamentoCont.IndSomenteDiaUtil,
                             // SEMANAL
                             x.SolicFechamentoCont.DiaDaSemana,
                             // MENSAL
                             x.SolicFechamentoCont.IndUltimoDiaDoMes,
                             x.SolicFechamentoCont.DiaDoMes,
                             IndicaFechamentoMensalME = x.SolicFechamentoCont.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.Mensal ? x.SolicFechamentoCont.IndFechamentoMensal : "N",
                             IndGerarBaseDadosFecME = x.SolicFechamentoCont.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.Mensal ? x.SolicFechamentoCont.IndGerarBaseDadosFec : "N",


                             x.NomeUsuario,
                             DataSolicitacao = x.SolicFechamentoCont.DataCadastro
                         })
                        .ToArray();

            var ordenado = lista.Select(x => x);

            #region ordenar
            switch (ordem)
            {
                case "Fechamento":
                    if (asc)
                        ordenado = ordenado.OrderBy(x => x.TipoFechamento != null).ThenBy(a => a.TipoFechamento);
                    else
                        ordenado = ordenado.OrderByDescending(a => a.TipoFechamento);
                    break;

                case "Empresas":
                    if (asc)
                        ordenado = ordenado.OrderBy(x => x.Empresas != null).ThenBy(a => a.Empresas);
                    else
                        ordenado = ordenado.OrderByDescending(a => a.Empresas).ThenByDescending(a => a.Empresas);
                    break;

                case "Descricao":
                    if (asc)
                        ordenado = ordenado.OrderBy(x => x.SolicitacaoExecucao != null).ThenBy(a => a.SolicitacaoExecucao);
                    else
                        ordenado = ordenado.OrderByDescending(a => a.SolicitacaoExecucao);
                    break;
                case "ProxExecucao":
                    if (asc)
                        ordenado = ordenado.OrderBy(x => x.ProximaExecucao != null).ThenBy(a => a.ProximaExecucao);
                    else
                        ordenado = ordenado.OrderByDescending(a => a.ProximaExecucao);
                    break;
                case "Usuario":
                    if (asc)
                        ordenado = ordenado.OrderBy(x => x.NomeUsuario != null).ThenBy(a => a.NomeUsuario);
                    else
                        ordenado = ordenado.OrderByDescending(a => a.NomeUsuario);
                    break;
                case "Solicitacao":
                    if (asc)
                        ordenado = ordenado.OrderBy(x => x.DataSolicitacao != null).ThenBy(a => a.DataSolicitacao);
                    else
                        ordenado = ordenado.OrderByDescending(a => a.DataSolicitacao);
                    break;
                default:
                    ordenado = ordenado.OrderBy(x => x.DataSolicitacao);
                    break;
            }
            #endregion

            var total = await query.CountAsync(ct);
            var totalLista = lista.Count();

            return Ok(new { ordenado, total, totalLista });
        }

        [HttpGet("obter-empresas")]
        public async Task<IActionResult> ObterEmpresas(CancellationToken ct)
        {
            var empresas = await _db.EmpresasCentralizadoras.AsNoTracking().Select(e => new
            {
                Id = e.Codigo,
                Nome = e.Nome
            })
                .OrderBy(x => x.Nome)
                .ToArrayAsync(ct);

            return Ok(empresas);
        }

        [HttpGet("obter-empresas-grupos")]
        public async Task<IActionResult> ObterEmpresasGrupos(CancellationToken ct)
        {
            var empresas = (await _db.VGrupamentoEmpresasGrupo.AsNoTracking().ToArrayAsync(ct))
                .GroupBy(x => new { x.Codigo, x.NomGrupoEmpresasOutliers })
                .Select(e => new
                {
                    CodigoEmp = e.Key.Codigo,
                    NomeGrupoEmp = e.Key.NomGrupoEmpresasOutliers,
                    NomeEmp = e.Select(x => string.Join("<br>", x.NomParte))
                })
                .ToArray();

            return Ok(empresas);
        }

        [HttpGet("obter-haircut-padrao")]
        public async Task<IActionResult> ObterHaircutPadarao(CancellationToken ct, [FromQuery] int modulo)
        {
            var percentual = await _db.SolicFechamentoCont.AsNoTracking()
                .Where(x => x.PercentualHaircut != null && x.CodTipoFechamento == modulo)
                .OrderByDescending(x => x.CodSolicFechamentoCont)
                .Take(1)
                .Select(p => p.PercentualHaircut)
                .ToArrayAsync(ct);

            return Ok(percentual[0].Value);
        }

        [HttpGet("obter-valor-outlier")]
        public async Task<IActionResult> ObterValorOutlierPadarao(CancellationToken ct)
        {
            var valOutlier = await _db.SolicFechamentoCont.AsNoTracking()
                .Where(x => x.ValCorteOutliers != null && x.CodTipoFechamento == 49)
                .OrderByDescending(x => x.CodSolicFechamentoCont)
                .Take(1)
                .Select(p => p.ValCorteOutliers)
                .ToArrayAsync(ct);

            return Ok(valOutlier[0].Value);
        }

        [HttpGet("obter-valores-trabalhista")]
        public async Task<IActionResult> ObterValoresTrabalhista(CancellationToken ct)
        {
            var query = _db.SolicFechamentoCont.AsNoTracking()
                .Where(x => x.CodTipoFechamento == 7)
                .OrderByDescending(x => x.CodSolicFechamentoCont);

            var valAjustePadrao = query.Where(x => x.ValAjusteDesvioPadrao != null).Select(x => x.ValAjusteDesvioPadrao).Take(1).ToArrayAsync(ct).Result[0].Value;

            var valPercProOut = query.Where(x => x.ValPercentProcOutliers != null).Select(x => x.ValPercentProcOutliers).Take(1).ToArrayAsync(ct).Result[0].Value;


            return Ok(new { valAjustePadrao, valPercProOut });
        }

        [HttpGet("obter-valores-pex")]
        public async Task<IActionResult> ObterValorPex(CancellationToken ct)
        {
            var multDesvioPadrao = await _db.SolicFechamentoCont.AsNoTracking()
                .Where(x => x.MultDesvioPadrao != null && x.CodTipoFechamento == 51)
                .OrderByDescending(x => x.CodSolicFechamentoCont)
                .Take(1)
                .Select(p => p.MultDesvioPadrao)
                .ToArrayAsync(ct);

            return Ok(multDesvioPadrao[0].Value);
        }

        [HttpDelete("excluir-agendamento")]
        public async Task<IActionResult> ExcluirAgendamentoAsync(CancellationToken ct, int id)
        {
            try
            {
                var agendamento = _db.SolicFechamentoCont.Include(x => x.SolicFechContEmpCent).FirstOrDefault(a => a.CodSolicFechamentoCont == id);

                if (agendamento is null)
                {
                    _logger.LogError($"Agendamento {id} não encontado");
                    return BadRequest("Agendamento não encontrado");
                }
                //PKE22136 - Não pode excluir fisicamente porque a tela de movimentações usa os dados da tabela de agendamento
                agendamento.IndAtivo = "N";
                _db.Update(agendamento);
                await _db.SaveChangesAsync(ct);

                return Ok("Excluido com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("incluir-agendamento")]
        public async Task<IActionResult> IncluirAgendamentoAsync(CancellationToken ct, [FromBody] SolicFechamentoContModel model)
        {
            try
            {
                var solicitacao = new SolicFechamentoCont();

                SalvarAgendamento(ref solicitacao, model);

                #region VALIDAR
                var lista = await _db.SolicFechamentoCont.AsNoTracking()
                    .Include(e => e.SolicFechContEmpCent)
                    .Where(x => x.IndAtivo == "S" && x.DatProximoAgend.HasValue && x.DatProximoAgend.Value.Date >= DateTime.Now.Date)
                    .ToListAsync(ct);

                var validado = Validar(solicitacao);
                if (!validado.IsValid) return BadRequest(validado.ToString());

                if (solicitacao.CodTipoFechamento != 51)
                {
                    var listaEmpresasSelecionadas = model.Empresa.Split(',').Select(a => short.Parse(a)).ToList();
                    var ehDuplicado = ValidarDuplicidade(solicitacao, lista, listaEmpresasSelecionadas);
                    if (ehDuplicado) return BadRequest("Já existe uma solicitação com esses dados.");

                    var ehFechamentoMensalExistente = ValidarExisteFechamentoMensal(solicitacao, lista, listaEmpresasSelecionadas);
                    if (ehFechamentoMensalExistente) return BadRequest("Já existe uma solicitação de fechamento mensal com esse Mês/Ano Contábil.");
                }

                var ehDataValida = ValidarPeriodoDataValidaFechamento(solicitacao);
                if (!ehDataValida) return BadRequest("Período de execução inválido.");

                if (solicitacao.CodTipoFechamento == 49)
                {
                    if (solicitacao.PercentualHaircut == null) return BadRequest("O campo % de Haircut é obrigatório");

                    var ehValidoValorCorteOutlier = ValidarValorCorteOutliers(solicitacao);
                    if (!ehValidoValorCorteOutlier.IsValid) return BadRequest(ehValidoValorCorteOutlier.ToString());
                }
                #endregion

                if (validado.IsValid && ehDataValida)
                {
                    _db.Add(solicitacao);
                    await _db.SaveChangesAsync(ct);
                    return Ok("Agendamento salvo com sucesso!");
                }

                return BadRequest("Não foi possivel salvar o agendamento");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("editar-agendamento")]
        public async Task<IActionResult> EditarAgendamentoAsync(CancellationToken ct, SolicFechamentoContModel model, int id)
        {
            try
            {
                var lista = _db.SolicFechamentoCont.AsNoTracking().Where(x => x.IndAtivo == "S" && x.DatProximoAgend.HasValue && x.DatProximoAgend.Value.Date >= DateTime.Now.Date);
                var solicitacao = lista.Include(x => x.SolicFechContEmpCent).FirstOrDefault(x => x.CodSolicFechamentoCont == id);

                if (solicitacao is null)
                {
                    _logger.LogError($"Agendamento {id} não encontado");
                    return BadRequest("Agendamento não encontrado");
                }

                _db.RemoveRange(solicitacao.SolicFechContEmpCent);

                SalvarAgendamento(ref solicitacao, model);

                #region VALIDAR
                var validado = Validar(solicitacao);
                if (!validado.IsValid) return BadRequest(validado.ToString());

                if (solicitacao.CodTipoFechamento != 51)
                {
                    var listaEmpresasSelecionadas = model.Empresa.Split(',').Select(a => short.Parse(a)).ToList();
                    var ehDuplicado = ValidarDuplicidade(solicitacao, lista, listaEmpresasSelecionadas);
                    if (ehDuplicado) return BadRequest("Já existe uma solicitação com esses dados.");

                    var ehFechamentoMensalExistente = ValidarExisteFechamentoMensal(solicitacao, lista, listaEmpresasSelecionadas);
                    if (ehFechamentoMensalExistente) return BadRequest("Já existe uma solicitação de fechamento mensal com esse Mês/Ano Contábil.");
                }

                var ehDataValida = ValidarPeriodoDataValidaFechamento(solicitacao);
                if (!ehDataValida) return BadRequest("Período de execução inválido.");

                if (solicitacao.CodTipoFechamento == 49)
                {
                    if (solicitacao.PercentualHaircut == null) return BadRequest("O campo % de Haircut é obrigatório");

                    var ehValidoValorCorteOutlier = ValidarValorCorteOutliers(solicitacao);
                    if (!ehValidoValorCorteOutlier.IsValid) return BadRequest(ehValidoValorCorteOutlier.ToString());
                }

                #endregion
                if (validado.IsValid && ehDataValida)
                {
                    _db.Update(solicitacao);
                    await _db.SaveChangesAsync(ct);
                    return Ok("Agendamento atualizado com sucesso!");
                }

                return BadRequest(validado.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        #region METODOS
        private string ObterSolicitacao(SolicFechamentoCont sfc)
        {
            var retorno = new List<string>();
            var periodicidade = sfc.PeriodicidadeExecucao;
            var tipoFechamento = ((TipoFechamentoContingenciaEnum)sfc.CodTipoFechamento!).ToDescription();

            if (tipoFechamento == "Trabalhista por Média" || tipoFechamento == "PEX por Média")
            {
                retorno.Add($"Nº Meses : {sfc.NumeroDeMeses}");
            }
            else if (tipoFechamento == "Juizado Especial")
            {
                retorno.Add($"Nº Meses : {sfc.NumeroDeMeses}");
                if (sfc.ValCorteOutliers.HasValue)
                {
                    retorno.Add($"Valor de Corte de Outliers: {sfc.ValCorteOutliers}");
                }
            }

            if (tipoFechamento == "Cível Consumidor por Média")
            {
                retorno.Add($"Nº Meses: {sfc.NumeroDeMeses}, %Haircut: {sfc.PercentualHaircut}");
            }

            switch (periodicidade)
            {
                case (int)PeriodicidadeExecucaoEnum.ExecucaoImediata:
                    retorno.Add($"Data da prévia: {sfc.DataPrevia:dd/MM/yyyy}");
                    retorno.Add($"Execução Imediata em {sfc.DataCadastro:dd/MM/yyyy}");
                    retorno.Add($"Mês/Ano Contábil: {sfc.MesContabil:D2}/{sfc.AnoContabil}");
                    break;

                case (int)PeriodicidadeExecucaoEnum.DataEspecifica:
                    retorno.Add($"Data da prévia: {sfc.DataPrevia:dd/MM/yyyy}");
                    retorno.Add($"Data específica em {sfc.DataEspecifica:dd/MM/yyyy}");
                    retorno.Add($"Mês/Ano Contábil: {sfc.MesContabil:D2}/{sfc.AnoContabil}");
                    break;

                case (int)PeriodicidadeExecucaoEnum.Diaria:
                    if (sfc.DataDiariaIni != null && sfc.DataDiariaFim != null)
                        retorno.Add($"Diária de {sfc.DataDiariaIni:dd/MM/yyyy} a {sfc.DataDiariaFim:dd/MM/yyyy}");
                    else
                        retorno.Add("Diária");

                    if (sfc.DataDiariaIni != null && sfc.DataDiariaFim == null)
                        retorno.Add($"Diária a partir de {sfc.DataDiariaIni:dd/MM/yyyy}");

                    if (sfc.DataDiariaFim != null && sfc.DataDiariaIni == null)
                        retorno.Add($"Diária até {sfc.DataDiariaFim:dd/MM/yyyy}");

                    if (sfc.IndSomenteDiaUtil == "S")
                        retorno.Add("somente em dias úteis");
                    break;

                case (int)PeriodicidadeExecucaoEnum.Mensal:
                    if (sfc.IndUltimoDiaDoMes == "S")
                        retorno.Add("Todo último dia do mês");
                    else
                        retorno.Add($"Todo dia {sfc.DiaDoMes}");
                    break;

                case (int)PeriodicidadeExecucaoEnum.Semanal:
                    var diaDaSemana = (DayOfWeek)sfc.DiaDaSemana;
                    var descricao = diaDaSemana == DayOfWeek.Monday || diaDaSemana == DayOfWeek.Sunday ? "Todo" : "Toda";
                    retorno.Add($"{descricao} {ObterDescricaoDoDiaDaSemana(diaDaSemana)}");
                    if (sfc.IndFechamentoMensal == "S")
                        retorno.Add("fechamento mensal");
                    //var descricao = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(sfc.DatProximoAgend.Value.DayOfWeek) ? sfc.DatProximoAgend.Value.ToString("To\\do dddd") : sfc.DatProximoAgend.Value.ToString("To\\da dddd");
                    //retorno.Add(descricao);
                    break;
            }

            if (sfc.IndFechamentoMensal == "S")
                retorno.Add("fechamento mensal");

            if (sfc.ValAjusteDesvioPadrao.HasValue || sfc.ValPercentProcOutliers.HasValue)
            {
                var outlier = string.Empty;
                if (sfc.ValAjusteDesvioPadrao.HasValue)
                {
                    outlier = $"{sfc.ValAjusteDesvioPadrao}x o Desvio Padrão";
                }
                else if (sfc.ValPercentProcOutliers.HasValue)
                {
                    outlier = $"{sfc.ValPercentProcOutliers}% dos Processos";
                }
                retorno.Add($"Aplicar Exclusão de Outliers: {outlier}");
            }
            else
            {
                if (tipoFechamento == "Trabalhista por Média")
                {
                    retorno.Add("Não Aplicar Exclusão de Outlier");
                }
            }

            if (tipoFechamento != "Cível Consumidor por Média")
            {
                if (sfc.PercentualHaircut.HasValue)
                {
                    retorno.Add($"{sfc.PercentualHaircut}% Haircut");
                }
            }

            if (sfc.MultDesvioPadrao.HasValue)
            {
                retorno.Add($"{sfc.MultDesvioPadrao} Multiplicador Desvio Padrão");
            }

            if (tipoFechamento == "PEX por Média")
            {
                if (sfc.IndAplicarHaircutProcGar == "S")
                {
                    retorno.Add("Aplicar haircut em processos com garantia");
                }
                else
                {
                    retorno.Add("Não aplicar haircut em processos com garantia");
                }
            }

            if (tipoFechamento == "Juizado Especial")
            {
                if (!string.IsNullOrEmpty(sfc.ObsValCorteOutliers))
                    retorno.Add($"Observação: {sfc.ObsValCorteOutliers}");
            }
            return string.Join(", ", retorno.ToArray());
        }

        private IEnumerable<SolicFechamentoCont> Ordenacao(IEnumerable<SolicFechamentoCont> fechamento, string ordem, bool asc)
        {
            switch (ordem)
            {
                case "Fechamento":
                    if (asc)
                        fechamento = fechamento.OrderBy(x => x.CodTipoFechamento != null).ThenBy(a => a.CodTipoFechamento);
                    else
                        fechamento = fechamento.OrderByDescending(a => a.CodTipoFechamento);
                    break;

                case "Empresas":
                    if (asc)
                        fechamento = fechamento.OrderBy(x => x.SolicFechContEmpCent.Select(x => x.CodEmpresasCentralizadorasNavigation.Nome) != null).ThenBy(a => a.SolicFechContEmpCent.Select(x => x.CodEmpresasCentralizadorasNavigation.Nome));
                    else
                        fechamento = fechamento.OrderByDescending(a => a.SolicFechContEmpCent.Select(x => x.CodEmpresasCentralizadorasNavigation.Nome)).ThenByDescending(a => a.SolicFechContEmpCent.Select(x => x.CodEmpresasCentralizadorasNavigation.Nome));
                    break;

                    //case FechamentoOrder.Nome:
                    //default:
                    //    if (asc)
                    //        fechamento = fechamento.OrderBy(x => x.Descricao != null).ThenBy(a => a.Descricao);
                    //    else
                    //        fechamento = fechamento.OrderByDescending(a => a.Descricao);
                    //    break;
            }
            return fechamento;
        }

        private void SalvarAgendamento(ref SolicFechamentoCont solicitacao, SolicFechamentoContModel model)
        {
            if (model != null)
            {
                var ehExecucaoImediata = model.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.ExecucaoImediata;
                var ehExecucaoMensal = model.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.Mensal;
                var ehExecucaoDataEspecifica = model.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.DataEspecifica;

                solicitacao.DiaDoMes = model.DiaDoMes == 0 ? null : model.DiaDoMes;
                solicitacao.DiaDaSemana = model.DiaDaSemana;
                solicitacao.CodUsuarioSolicitacao = User.Identity!.Name;
                solicitacao.DataDiariaFim = model.DataDiariaFim;
                solicitacao.DataDiariaIni = model.DataDiariaIni;
                solicitacao.DataEspecifica = model.DataEspecifica;
                solicitacao.IndExecucaoImediata = model.IndExecucaoImediata == "S" ? model.IndExecucaoImediata : "N";
                solicitacao.DataCadastro = DateTime.Now;
                solicitacao.IndSomenteDiaUtil = string.IsNullOrEmpty(model.IndSomenteDiaUtil) ? "N" : model.IndSomenteDiaUtil;
                solicitacao.IndUltimoDiaDoMes = string.IsNullOrEmpty(model.IndUltimoDiaDoMes) ? "N" : model.IndUltimoDiaDoMes;
                solicitacao.NumeroDeMeses = model.NumeroDeMeses == null ? null : model.NumeroDeMeses;
                solicitacao.PeriodicidadeExecucao = model.PeriodicidadeExecucao;
                solicitacao.CodTipoFechamento = model.CodTipoFechamento;
                solicitacao.IndAtivo = model.IndAtivo;
                solicitacao.DatProximoAgend = CalcularDataProximoAgendamento(DateTime.Now.Date, model);
                solicitacao.IndAplicarHaircutProcGar = model.IndAplicarHaircutProcGar;
                solicitacao.PercentualHaircut = !String.IsNullOrEmpty(model.PercentualHaircut.ToString()) ? Decimal.Parse(model.PercentualHaircut.ToString()) : (decimal?)null;
                solicitacao.IndFechamentoMensal = model.IndFechamentoMensal;
                solicitacao.IndGerarBaseDadosFec = model.IndGerarBaseDadosFec;
                solicitacao.ValAjusteDesvioPadrao = model.ValAjusteDesvioPadrao != null ? model.ValAjusteDesvioPadrao : null;
                solicitacao.ValPercentProcOutliers = model.ValPercentProcOutliers != null ? model.ValPercentProcOutliers : null;
                solicitacao.ValCorteOutliers = model.ValCorteOutliers != null ? model.ValCorteOutliers : null;
                solicitacao.ObsValCorteOutliers = model.ObsValCorteOutliers;
                solicitacao.MultDesvioPadrao = model.MultDesvioPadrao == 0 ? null : model.MultDesvioPadrao;
                solicitacao.MesContabil = ehExecucaoImediata || ehExecucaoDataEspecifica ? (byte?)model.DataPrevia!.Value.Date.Month : null;
                solicitacao.AnoContabil = ehExecucaoImediata || ehExecucaoDataEspecifica ? (short?)model.DataPrevia!.Value.Date.Year : null;
                solicitacao.DataPrevia = model.DataPrevia;
                solicitacao.IndAgendRelMovAutomatico = model.IndAgendRelMovAutomatico;

                if (solicitacao.CodTipoFechamento == 7)
                    solicitacao.CodTipoFechamentoTrab = "TM";

                var listaEmpresasSelecionadas = model.Empresa.Split(',').Select(a => int.Parse(a)).ToList();

                foreach (var empresaId in listaEmpresasSelecionadas)
                {
                    SolicFechContEmpCent sfcec = new SolicFechContEmpCent();
                    sfcec.CodEmpresasCentralizadoras = (short)empresaId;
                    solicitacao.SolicFechContEmpCent.Add(sfcec);
                }
            }
        }

        #region CALCULAR DATAS
        private DateTime? CalcularDataProximoAgendamento(DateTime dataAtual, SolicFechamentoContModel model)
        {
            var ehPrimeiraExecucao = !model.DatUltimoAgend.HasValue;

            //Imediato
            if (model.PeriodicidadeExecucao == 0 && ehPrimeiraExecucao)
                return dataAtual.Date;

            //Data Específica
            if (model.PeriodicidadeExecucao == 1 && ehPrimeiraExecucao)
                return model.DataEspecifica;

            //Diária
            if (model.PeriodicidadeExecucao == 2)
                return CalcularDataProximoAgendamentoDiario(dataAtual, model);

            //Semanal
            if (model.PeriodicidadeExecucao == 3)
            {
                var diaDaSemana = model.DiaDaSemana!.Value;
                var dataPreviaPrevista = !model.DatUltimoAgend.HasValue ?
                    dataAtual.ToNextDayOfWeek((DayOfWeek)diaDaSemana) :
                    dataAtual.AddDays(1).ToNextDayOfWeek((DayOfWeek)diaDaSemana);

                var dataDaProximaExecucao = dataPreviaPrevista.AddDays(1);
                return dataDaProximaExecucao;
            }

            //Mensal
            if (model.PeriodicidadeExecucao == 4)
                return CalcularDataProximoAgendamentoMensal(dataAtual, model);

            return null;
        }

        private DateTime? CalcularDataProximoAgendamentoMensal(DateTime dataAtual, SolicFechamentoContModel model)
        {
            if (!model.DatUltimoAgend.HasValue)
            {
                var dataUltimoDiaMes = dataAtual.GetLastDayOfMonth();
                var dataPreviaInicial = model.IndUltimoDiaDoMes == "S" || dataUltimoDiaMes.Day <= model.DiaDoMes ?
                     dataUltimoDiaMes :
                     dataAtual.ToDayOfMonth(model.DiaDoMes!.Value);

                if (model.IndUltimoDiaDoMes == "N" && (model.DiaDoMes.HasValue && model.DiaDoMes.Value < dataAtual.Day))
                {
                    dataPreviaInicial = dataPreviaInicial.AddMonths(1);
                }

                var dataInicialAgendamento = dataPreviaInicial.AddDays(1);
                return dataInicialAgendamento;
            }

            var dataProximoAgendamento = dataAtual.AddMonths(1);
            return dataProximoAgendamento;
        }

        private DateTime? CalcularDataProximoAgendamentoDiario(DateTime dataAtual, SolicFechamentoContModel model)
        {
            if (!model.DatUltimoAgend.HasValue)
            {
                var dataPreviaInicial = model.DataDiariaIni!.Value.Date;

                if (model.IndSomenteDiaUtil == "S")
                {
                    bool dataPreviaEmDiaUtil = false;
                    while (!dataPreviaEmDiaUtil)
                    {
                        if (dataPreviaInicial.VerificaSeDiaUtil()) break;
                        dataPreviaInicial = dataPreviaInicial.AddDays(1);
                    }
                    return dataPreviaInicial;
                }

                if (dataPreviaInicial == model.DataDiariaFim!.Value.Date)
                    return dataPreviaInicial;

                dataPreviaInicial = dataPreviaInicial.AddDays(1);
                return dataPreviaInicial;
            }

            var dataPrevia = dataAtual.Date;
            DateTime dataProximoAgendamento = dataPrevia.AddDays(1);
            if (dataPrevia > model.DataDiariaFim) return null;

            if (model.IndSomenteDiaUtil == "S")
            {
                bool dataPreviaEmDiaUtil = false;
                while (!dataPreviaEmDiaUtil)
                {
                    dataPrevia = dataProximoAgendamento.AddDays(-1);
                    if (dataPrevia.VerificaSeDiaUtil()) break;

                    dataProximoAgendamento = dataProximoAgendamento.AddDays(1);
                }
            }

            return dataProximoAgendamento;
        }

        private static string ObterDescricaoDoDiaDaSemana(DayOfWeek diaDaSemana)
        {
            if (diaDaSemana == DayOfWeek.Sunday) return "Domingo";
            if (diaDaSemana == DayOfWeek.Monday) return "Segunda-feira";
            if (diaDaSemana == DayOfWeek.Tuesday) return "Terça-feira";
            if (diaDaSemana == DayOfWeek.Wednesday) return "Quarta-feira";
            if (diaDaSemana == DayOfWeek.Thursday) return "Quinta-feira";
            if (diaDaSemana == DayOfWeek.Friday) return "Sexta-feira";
            if (diaDaSemana == DayOfWeek.Saturday) return "Sábado";
            return string.Empty;
        }
        #endregion


        #region VALIDACOES
        private ValidationResult Validar(SolicFechamentoCont solicitacao)
        {
            SolicFechamentoContValidator validator = new SolicFechamentoContValidator();
            ValidationResult result = validator.Validate(solicitacao);
            return result;
        }

        private bool ValidarDuplicidade(SolicFechamentoCont solicitacao, IEnumerable<SolicFechamentoCont> lista, IList<short> empresas)
        {
            var codTipoOutlier = (byte)(solicitacao.ValAjusteDesvioPadrao.HasValue == false && solicitacao.ValPercentProcOutliers.HasValue == false ? 0 : solicitacao.ValAjusteDesvioPadrao.HasValue ? 1 : 2);

            if (solicitacao.CodSolicFechamentoCont > 0)
            {
                lista = lista.Where(x => x.CodSolicFechamentoCont != solicitacao.CodSolicFechamentoCont);
            }

            lista = lista.Where(x => x.CodTipoFechamento == solicitacao.CodTipoFechamento);

            if (solicitacao.CodTipoFechamento == 7)
            {
                lista = lista.Where(x => x.CodTipoFechamentoTrab == solicitacao.CodTipoFechamentoTrab);
            }

            switch (codTipoOutlier)
            {
                case 0:
                    lista = lista.Where(x => x.ValPercentProcOutliers == null && x.ValAjusteDesvioPadrao == null);
                    break;
                case 1:
                    lista = lista.Where(x => x.ValPercentProcOutliers == null && x.ValAjusteDesvioPadrao != null);
                    break;
                case 2:
                    lista = lista.Where(x => x.ValPercentProcOutliers != null && x.ValAjusteDesvioPadrao == null);
                    break;
            }

            if (solicitacao.PeriodicidadeExecucao == 0)
            {
                lista = lista.Where(x => x.DatUltimoAgend == null);
            }

            lista = lista.Where(x => solicitacao.NumeroDeMeses != null ? x.NumeroDeMeses == solicitacao.NumeroDeMeses : x.NumeroDeMeses == null);
            lista = lista.Where(x => solicitacao.MesContabil != null ? x.MesContabil == solicitacao.MesContabil : x.MesContabil == null);
            lista = lista.Where(x => solicitacao.AnoContabil != null ? x.AnoContabil == solicitacao.AnoContabil : x.AnoContabil == null);
            lista = lista.Where(x => solicitacao.DataPrevia != null ? x.DataPrevia == solicitacao.DataPrevia : x.DataPrevia == null);
            lista = lista.Where(x => solicitacao.DataEspecifica != null ? x.DataEspecifica == solicitacao.DataEspecifica : x.DataEspecifica == null);
            lista = lista.Where(x => solicitacao.DataDiariaIni != null ? x.DataDiariaIni == solicitacao.DataDiariaIni : x.DataDiariaIni == null);
            lista = lista.Where(x => solicitacao.DiaDaSemana != null ? x.DiaDaSemana == solicitacao.DiaDaSemana : x.DiaDaSemana == null);
            lista = lista.Where(x => solicitacao.DiaDoMes != null ? x.DiaDoMes == solicitacao.DiaDoMes : x.DiaDoMes == null);
            lista = lista.Where(x =>
                                x.IndExecucaoImediata == solicitacao.IndExecucaoImediata &&
                                x.PeriodicidadeExecucao == solicitacao.PeriodicidadeExecucao &&
                                x.IndUltimoDiaDoMes == solicitacao.IndUltimoDiaDoMes &&
                                x.IndSomenteDiaUtil == solicitacao.IndSomenteDiaUtil &&
                                x.IndAtivo == "S");

            //if (solicitacao.IndExecucaoImediata == "S" || solicitacao.PeriodicidadeExecucao == 0)
            //{
            //    //criterio.Add(
            //    //    Expression.Sql(" NOT EXISTS (select 1 from JUR.EMP_CENT_AGENDAM_FECH_AUTO f "
            //    //                 + "where f.cod_solic_fechamento_cont = this_.cod_solic_fechamento_cont)"));
            //}

            var lista2 = lista.ToList();

            for (int i = 0; i < lista2.Count; i++)
            {
                lista2[i].SolicFechContEmpCent = lista2[i].SolicFechContEmpCent.Where(e => empresas.Contains(e.CodEmpresasCentralizadoras)).ToList();
            }

            var resultQuery = lista2.ToList();
            return lista2.Any(x => x.SolicFechContEmpCent.Any());
        }

        private bool ValidarExisteFechamentoMensal(SolicFechamentoCont solicitacao, IEnumerable<SolicFechamentoCont> lista, IList<short> empresas)
        {
            if (solicitacao.IndFechamentoMensal == "S")
            {
                //if (solicitacao.MesContabil != null && solicitacao.AnoContabil != null) {
                //Quando há uma solicitação mensal, o mes/ano contabil não são preenchidos, 
                //dessa forma o critério não era atendido caso já houvesse um fechamento mensal
                //Não validava, por isso passamos a considerar valores nulos
                //}

                lista = lista.Where(x => x.CodTipoFechamento == solicitacao.CodTipoFechamento && x.DatProximoAgend != null);

                if (solicitacao.CodSolicFechamentoCont > 0)
                {
                    lista = lista.Where(x => x.CodSolicFechamentoCont != solicitacao.CodSolicFechamentoCont);
                }

                if (solicitacao.CodTipoFechamento == 7)
                {
                    lista = lista.Where(x => x.CodTipoFechamentoTrab == solicitacao.CodTipoFechamentoTrab);
                }

                //if (solicitacao.IndExecucaoImediata == "S" || solicitacao.PeriodicidadeExecucao == 0)
                //{
                //    //criterio.Add(
                //    //    Expression.Sql(" NOT EXISTS (select 1 from JUR.EMP_CENT_AGENDAM_FECH_AUTO f "
                //    //                 + "where f.cod_solic_fechamento_cont = this_.cod_solic_fechamento_cont)"));
                //}
                if (solicitacao.IndExecucaoImediata != "S" || solicitacao.PeriodicidadeExecucao != 0)
                {
                    lista = lista.Where(x => x.PeriodicidadeExecucao == solicitacao.PeriodicidadeExecucao && x.IndFechamentoMensal == "S");
                }

                lista = lista.Where(x =>
                    (x.MesContabil == solicitacao.MesContabil || x.MesContabil == null)
                    && (x.AnoContabil == solicitacao.AnoContabil || x.AnoContabil == null)
                    && (x.IndFechamentoMensal == "S" && x.IndAtivo == "S"));


                var lista2 = lista.ToList();

                for (int i = 0; i < lista2.Count; i++)
                {
                    lista2[i].SolicFechContEmpCent = lista2[i].SolicFechContEmpCent.Where(e => empresas.Contains(e.CodEmpresasCentralizadoras)).ToList();
                }

                var resultQuery = lista2.ToList();
                return lista2.Any(x => x.SolicFechContEmpCent.Any());
            }
            return false;
        }

        private bool ValidarPeriodoDataValidaFechamento(SolicFechamentoCont solicitacao)
        {
            if (solicitacao.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.Diaria)
            {
                bool ehDataValida = solicitacao.DatProximoAgend!.Value <= solicitacao.DataDiariaFim;
                return ehDataValida;
            }
            return true;
        }

        //private bool ValidarPercentualHairCut(SolicFechamentoCont solicitacao, IEnumerable<SolicFechamentoCont> db)
        //{
        //    db = db.Where(x => solicitacao.PercentualHaircut.HasValue);
        //    return db.Any();
        //}

        private ValidationResult ValidarValorCorteOutliers(SolicFechamentoCont solicitacao)
        {
            ValorCorteOutlierValidator validator = new ValorCorteOutlierValidator();
            ValidationResult result = validator.Validate(solicitacao);
            return result;
        }
        #endregion

        #endregion
    }

}