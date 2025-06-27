using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.EmpresaContratadaContext.Entities;
using Oi.Juridico.Contextos.V2.PermissaoContext.Extensions;
using Oi.Juridico.Contextos.V2.ResultadoNegociacaoContext.Data;
using Oi.Juridico.Contextos.V2.ResultadoNegociacaoContext.Entities;
using Oi.Juridico.Contextos.V2.StatusContatoContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.EmpresaContratada.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.EmpresaContratada.DTO_s;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.ResultadoNegociacao.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.ResultadoNegociacao.DTO_s;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.Controller;
using Oi.Juridico.WebApi.V2.Attributes;
using Perlink.Oi.Juridico.Infra.Constants;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.ResultadoNegociacao.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultadoNegociacaoController : ControllerBase
    {
        private readonly ResultadoNegociacaoDbContext _db;
        private readonly ILogger<ContratoEscritorioController> _logger;

        public ResultadoNegociacaoController(ResultadoNegociacaoDbContext db, ILogger<ContratoEscritorioController> logger)
        {
            _db = db;
            _logger = logger;
        }


        [HttpGet("obter/lista-resultado-negociacao")]
        [TemPermissao(Permissoes.ACESSAR_RESULTADO_NEGOCIACAO)]
        public async Task<ActionResult<ListaResultadoNegociacaoResponse>> ObterListaResultadoNegociacaoAsync(CancellationToken ct, [FromQuery] string? filtro, [FromQuery] string? ordem, [FromQuery] bool asc = true, [FromQuery] int page = 0, [FromQuery] int size = 8)
        {
            try
            {
                using var scope = _db.Database.BeginTransaction();

                var query = QueryListar(filtro, ordem, asc);

                // retorna a lista da página
                var lista = query.Skip(page * size).Take(size);

                // conta o total de lista da página
                var total = query.Count();

                return Ok(new { lista, total });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados dos resultados das negociações.", erro = ex.Message });
            }
        }


        [HttpGet("obter/resultado-negociacao")]
        [TemPermissao(Permissoes.ACESSAR_RESULTADO_NEGOCIACAO)]
        public async Task<ActionResult<ResultadoNegociacaoResponse>> ObterResultadoNegociacaoAsync(CancellationToken ct, int codResultado)
        {
            try
            {
                using var scope = _db.Database.BeginTransaction();

                var resultadoNegociacao = await _db.ResultadoNegociacao
                .AsNoTracking()
                .Where(x => x.CodResultadoNegociacao == codResultado)
                .Select(x => new ResultadoNegociacaoResponse
                {
                    CodResultadoNegociacao = x.CodResultadoNegociacao,
                    DscResultadoNegociacao = x.DscResultadoNegociacao,
                    IndNegociacao = x.IndNegociacao,
                    IndPosSentenca = x.IndPosSentenca,
                    CodTipoResultado = x.CodTipoResultado,
                    IndAtivo = x.IndAtivo
                }).ToListAsync(ct);

                return Ok(resultadoNegociacao);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados dos resultados das negociações.", erro = ex.Message });
            }
        }


        [HttpPost("salvar-resultado-negociacao")]
        [TemPermissao(Permissoes.ACESSAR_RESULTADO_NEGOCIACAO)]
        public async Task<IActionResult> SalvarResultadoNegociacaoAsync(CancellationToken ct, ResultadoNegociacaoRequest model)
        {
            try
            {
                Contextos.V2.ResultadoNegociacaoContext.Entities.ResultadoNegociacao resultadoNegociacao = new Contextos.V2.ResultadoNegociacaoContext.Entities.ResultadoNegociacao();

                resultadoNegociacao.CodResultadoNegociacao = model.CodResultadoNegociacao;
                resultadoNegociacao.DscResultadoNegociacao = model.DscResultadoNegociacao;
                resultadoNegociacao.IndNegociacao = model.IndNegociacao ? "S" : "N";
                resultadoNegociacao.IndPosSentenca = model.IndPosSentenca ? "S" : "N";
                resultadoNegociacao.CodTipoResultado = model.CodTipoResultado;
                resultadoNegociacao.IndAtivo = model.IndAtivo ? "S" : "N";

                await _db.ResultadoNegociacao.AddAsync(resultadoNegociacao, ct);
                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                return Ok("Resultado de Negociação registrado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha na inclusão do Resultado de Negociação.", erro = ex.Message });
            }
        }


        [HttpPut("editar-resultado-negociacao")]
        [TemPermissao(Permissoes.ACESSAR_RESULTADO_NEGOCIACAO)]
        public async Task<IActionResult> EditarResultadoNegociacaoAsync(CancellationToken ct, ResultadoNegociacaoRequest model)
        {
            try
            {
                var resultadoNegociacao = _db.ResultadoNegociacao.FirstOrDefault(x => x.CodResultadoNegociacao == model.CodResultadoNegociacao);

                if (resultadoNegociacao == null)
                    return BadRequest("Não foi possivel encontrar o Resultado de Negociação.");

                resultadoNegociacao.CodResultadoNegociacao = model.CodResultadoNegociacao;
                resultadoNegociacao.DscResultadoNegociacao = model.DscResultadoNegociacao;
                resultadoNegociacao.IndNegociacao = model.IndNegociacao ? "S" : "N";
                resultadoNegociacao.IndPosSentenca = model.IndPosSentenca ? "S" : "N";
                resultadoNegociacao.CodTipoResultado = model.CodTipoResultado;
                resultadoNegociacao.IndAtivo = model.IndAtivo ? "S" : "N";

                _db.ResultadoNegociacao.Update(resultadoNegociacao);
                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                return Ok("Resultado de Negociação registrado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha na alteração do Resultado de Negociação.", erro = ex.Message });
            }
        }


        [HttpDelete("excluir-resultado-negociacao")]
        [TemPermissao(Permissoes.ACESSAR_RESULTADO_NEGOCIACAO)]
        public async Task<IActionResult> ExcluirResultadoNegociacaoAsync(CancellationToken ct, int codResultado)
        {
            try
            {
                var resultadoNegociacao = _db.ResultadoNegociacao.FirstOrDefault(x => x.CodResultadoNegociacao == codResultado);
                var existeNegociacaoProcesso = _db.ResultadoCttMtv.Any(x => x.CodResultadoNegociacao == codResultado);

                if (resultadoNegociacao == null)
                    return BadRequest("Não foi possivel encontrar o Resultado de Negociação.");

                if (existeNegociacaoProcesso)
                    return BadRequest("Esse resultado de negociação não poderá ser excluído, pois já foi registrado em um contato de processo.");

                _db.ResultadoNegociacao.Remove(resultadoNegociacao);
                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                return Ok("Resultado de Negociação excluído com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha ao excluir Resultado de Negociação.", erro = ex.Message });
            }
        }


        #region Download Methods

        [HttpGet("download-lista-resultado-negociacao")]
        [TemPermissao(Permissoes.ACESSAR_RESULTADO_NEGOCIACAO)]
        public async Task<FileContentResult> DownloadListaResultadoNegociacaoAsync(CancellationToken ct, [FromQuery] string? filtro, [FromQuery] string? ordem, [FromQuery] bool asc = true)
        {
            var queryResult = QueryListar(filtro, ordem, asc);

            var nomeArquivoListaResultadoNegociacao = $"Lista_Resultado_de_Negociacao_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            var csv = queryResult.ToCsvByteArray(typeof(DownloadListaResultadoNegociacaoResponseMap), false);

            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "application/octet-stream", nomeArquivoListaResultadoNegociacao);
        }


        [HttpGet("download-log-resultado-negociacao")]
        [TemPermissao(Permissoes.ACESSAR_RESULTADO_NEGOCIACAO)]
        public async Task<FileContentResult> DownloadLogResultadoNegociacaoAsync(CancellationToken ct)
        {
            var queryResult = await QueryLogDownload().ToArrayAsync(ct);

            var nomeArquivoListaResultadoNegociacao = $"Log_Resultado_de_Negociacao_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            var csv = queryResult.ToCsvByteArray(typeof(DownloadLogResultadoNegociacaoResponseMap), false);

            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "application/octet-stream", nomeArquivoListaResultadoNegociacao);
        }

        #endregion


        #region Private Methods

        private IEnumerable<ListaResultadoNegociacaoResponse> QueryListar(string? filtro, string? ordem, bool asc)
        {
            _db.PesquisarPorCaseInsensitive();

            // monta a query
            var query = _db.ResultadoNegociacao
                .AsNoTracking()
                .WhereIfNotEmpty(x => x.DscResultadoNegociacao.Contains(filtro!), filtro)
                .Select(x => new ListaResultadoNegociacaoResponse
                {
                    CodResultadoNegociacao = x.CodResultadoNegociacao,
                    DscResultadoNegociacao = x.DscResultadoNegociacao,
                    DscTipoNegociacao = DscTipoNegociacao(x.IndNegociacao, x.IndPosSentenca),
                    DscTipoResultado = ((ResultadoNegociacaoEnum)x.CodTipoResultado!).ToDescription(),
                    IndAtivo = x.IndAtivo == "S" ? "Ativo" : "Inativo"
                }).AsEnumerable();

            // monta a ordenação
            switch (ordem)
            {
                case "dsc":
                    if (asc)
                        query = query.OrderBy(x => x.DscResultadoNegociacao);
                    else
                        query = query.OrderByDescending(a => a.DscResultadoNegociacao);
                    break;
                case "tpNegociacao":
                    if (asc)
                        query = query.OrderBy(x => x.DscTipoNegociacao);
                    else
                        query = query.OrderByDescending(a => a.DscTipoNegociacao);
                    break;
                case "tpResultado":
                    if (asc)
                        query = query.OrderBy(x => x.DscTipoResultado);
                    else
                        query = query.OrderByDescending(a => a.DscTipoResultado);
                    break;
                case "status":
                    if (asc)
                        query = query.OrderBy(x => x.IndAtivo);
                    else
                        query = query.OrderByDescending(a => a.IndAtivo);
                    break;
                default:
                    query = query.OrderBy(x => x.DscResultadoNegociacao);
                    break;
            }

            return query;
        }

        private static string DscTipoNegociacao(string IndNegociacao, string IndPosSentenca)
        {
            var negociacao = "Ilha de Negociação";
            var pos = "Pós Sentença";

            if (IndNegociacao == "S" && IndPosSentenca == "S")
                return negociacao + " e " + pos;
            if (IndPosSentenca == "S")
                return pos;
            if (IndNegociacao == "S")
                return negociacao;

            return string.Empty;
        }

        private IQueryable<DownloadLogResultadoNegociacaoResponse> QueryLogDownload()
        {
            var query = _db.LogResultadoNegociacao
                .AsNoTracking()
                .Select(x => new DownloadLogResultadoNegociacaoResponse
                {
                    CodResultadoNegociacao = x.CodResultadoNegociacao,
                    Operacao = x.Operacao,
                    DatLog = x.DatLog,
                    CodUsuario = x.CodUsuario,
                    DscResultadoNegociacaoA = x.DscResultadoNegociacaoA,
                    DscResultadoNegociacaoD = x.DscResultadoNegociacaoD,
                    IndNegociacaoA = x.IndNegociacaoA,
                    IndNegociacaoD = x.IndNegociacaoD,
                    IndPosSentencaA = x.IndPosSentencaA,
                    IndPosSentencaD = x.IndPosSentencaD,
                    CodTipoResultadoA = x.CodTipoResultadoA.HasValue ? ((ResultadoNegociacaoEnum)x.CodTipoResultadoA!).ToDescription() : string.Empty,
                    CodTipoResultadoD = x.CodTipoResultadoD.HasValue ? ((ResultadoNegociacaoEnum)x.CodTipoResultadoD!).ToDescription() : string.Empty,
                    IndAtivoA = x.IndAtivoA != null ? (x.IndAtivoA == "S" ? "Ativo" : "Inativo") : string.Empty,
                    IndAtivoD = x.IndAtivoD != null ? (x.IndAtivoD == "S" ? "Ativo" : "Inativo") : string.Empty
                }).OrderByDescending(x => x.DatLog);
            
            return query;
        }

        #endregion

    }
}
