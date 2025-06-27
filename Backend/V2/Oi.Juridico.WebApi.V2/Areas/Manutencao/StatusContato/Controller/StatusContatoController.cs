using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.PermissaoContext.Extensions;
using Oi.Juridico.Contextos.V2.StatusContatoContext.Data;
using Oi.Juridico.Contextos.V2.StatusContatoContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.StatusContato.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.StatusContato.DTO_s;
using Oi.Juridico.WebApi.V2.Attributes;
using Perlink.Oi.Juridico.Infra.Constants;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.StatusContato.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusContatoController : ControllerBase
    {
        private readonly StatusContatoDbContext _db;

        public StatusContatoController(StatusContatoDbContext db)
        {
            _db = db;
        }

        [HttpGet("obter/lista-status-contato")]
        [TemPermissao(Permissoes.ACESSAR_STATUS_CONTATO)]
        public async Task<ActionResult<StatusContatoResponse>> ObterListaStatusContatoAsync(CancellationToken ct, [FromQuery] string? filtro, [FromQuery] string? ordem, [FromQuery] bool asc = true, [FromQuery] int page = 0, [FromQuery] int size = 8)
        {
            try
            {
                using var scope = _db.Database.BeginTransaction();

                var query = QueryListar(filtro, ordem, asc);

                // retorna a lista da página
                var lista = await query
                    .Skip(page * size).Take(size)
                    .ToArrayAsync(ct);

                // conta o total de lista da página
                var total = query.Count();

                return Ok(new { lista, total });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados dos status de contato.", erro = ex.Message });
            }
        }


        [HttpGet("obter/status-contato")]
        [TemPermissao(Permissoes.ACESSAR_STATUS_CONTATO)]
        public async Task<ActionResult<StatusContatoResponse>> ObterStatusContatoAsync(CancellationToken ct, int codStatusContato)
        {
            try
            {
                using var scope = _db.Database.BeginTransaction();

                var statusContatoResponse = await _db.StatusContato.AsNoTracking().Where(x => x.CodStatusContato == codStatusContato)
                    .Select(x => new StatusContatoResponse
                    {
                        CodStatusContato = x.CodStatusContato,
                        DscStatusContato = x.DscStatusContato,
                        IndContatoRealizado = x.IndContatoRealizado,
                        IndAcordoRealizado = x.IndAcordoRealizado,
                        IndAtivo = x.IndAtivo == "N" ? "N" :"S"
                    }).FirstOrDefaultAsync(ct);

                if (statusContatoResponse == null)
                    return BadRequest("Falha ao recuperar dados do status de contato.");

                return Ok(statusContatoResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha ao recuperar dados do status de contato.", erro = ex.Message });
            }
        }


        [HttpPost("salvar-status-contato")]
        [TemPermissao(Permissoes.ACESSAR_STATUS_CONTATO)]
        public async Task<IActionResult> SalvarStatusContatoAsync(CancellationToken ct, StatusContatoRequest model)
        {
            try
            {
                var statusContato = new Contextos.V2.StatusContatoContext.Entities.StatusContato();
                statusContato.CodStatusContato = model.CodStatusContato;
                statusContato.DscStatusContato = model.DscStatusContato;
                statusContato.IndContatoRealizado = model.IndContatoRealizado.HasValue ? ((bool)model.IndContatoRealizado ? "S" : "N") : null;
                statusContato.IndAcordoRealizado = model.IndAcordoRealizado.HasValue ? ((bool)model.IndAcordoRealizado ? "S" : "N") : null;
                statusContato.IndAtivo = model.IndAtivo ? "S" : "N";

                await _db.StatusContato.AddAsync(statusContato, ct);
                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                return Ok("Status de Contato registrado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensagem = "Falha na inclusão do status de contato.",
                    erro = ex.Message
                });
            }
        }


        [HttpPut("editar-status-contato")]
        [TemPermissao(Permissoes.ACESSAR_STATUS_CONTATO)]
        public async Task<IActionResult> EditarStatusContatoAsync(CancellationToken ct, StatusContatoRequest model)
        {
            try
            {
                var statusContato = _db.StatusContato.FirstOrDefault(x => x.CodStatusContato == model.CodStatusContato);

                if (statusContato == null)
                    return BadRequest("Não foi possivel encontrar o Status de Contato.");

                statusContato.CodStatusContato = model.CodStatusContato;
                statusContato.DscStatusContato = model.DscStatusContato;
                statusContato.IndContatoRealizado = model.IndContatoRealizado.HasValue ? ((bool)model.IndContatoRealizado ? "S" : "N") : null;
                statusContato.IndAcordoRealizado = model.IndAcordoRealizado.HasValue ? ((bool)model.IndAcordoRealizado ? "S" : "N") : null;
                statusContato.IndAtivo = model.IndAtivo ? "S" : "N";

                _db.StatusContato.Update(statusContato);
                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                return Ok("Status de Contato registrado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha na alteração do status de contato.", erro = ex.Message });
            }
        }


        [HttpDelete("excluir-status-contato")]
        [TemPermissao(Permissoes.ACESSAR_STATUS_CONTATO)]
        public async Task<IActionResult> ExcluirStatusContatoAsync(CancellationToken ct, int codStatusContato)
        {
            try
            {
                var statusContato = _db.StatusContato.FirstOrDefault(x => x.CodStatusContato == codStatusContato);
                var existeContatoProcesso = _db.ContatoProcesso.Any(x => x.CodStatusContato == codStatusContato);

                if (statusContato == null)
                    return BadRequest("Não foi possivel encontrar o Status de Contato.");
                
                if (existeContatoProcesso)
                    return BadRequest("O status de contato não poderá ser excluído, pois está sendo utilizado em negociações já registradas no SISJUR.");

                _db.StatusContato.Remove(statusContato);
                await _db.SaveChangesAsync(User.Identity!.Name!, true, ct);

                return Ok("Status de Contato excluída com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Falha ao excluir Status de Contato.", erro = ex.Message });
            }
        }


        #region Download Methods

        [HttpGet("download-lista-status-contato")]
        [TemPermissao(Permissoes.ACESSAR_STATUS_CONTATO)]
        public async Task<FileContentResult> DownloadListaStatusContatoAsync(CancellationToken ct, [FromQuery] string? filtro, [FromQuery] string? ordem, [FromQuery] bool asc = true)
        {
            var queryResult = QueryListar(filtro, ordem, asc).ToArray();

            var nomeArquivoListaStatusContato = $"Lista_Status_Contato_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            var csv = queryResult.ToCsvByteArray(typeof(DownloadListaStatusContatoResponseMap), false);

            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "application/octet-stream", nomeArquivoListaStatusContato);
        }


        [HttpGet("download-log-status-contato")]
        [TemPermissao(Permissoes.ACESSAR_STATUS_CONTATO)]
        public async Task<FileContentResult> DownloadLogStatusContatoAsync()
        {
            var queryResult = QueryLogDownload().ToArray();

            var nomeArquivoLogStatusContato = $"Log_Status_Contato_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            var csv = queryResult.ToCsvByteArray(typeof(DownloadLogStatusContatoResponseMap), false);

            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return File(bytes, "application/octet-stream", nomeArquivoLogStatusContato);
        }

        #endregion


        #region Private Methods

        private IQueryable<StatusContatoResponse> QueryListar(string? filtro, string? ordem, bool asc)
        {
            _db.PesquisarPorCaseInsensitive();

            // monta a query
            var query = _db.StatusContato
                .AsNoTracking()
                .WhereIfNotEmpty(x => x.DscStatusContato.Contains(filtro!), filtro)
                .Select(x => new StatusContatoResponse
                {
                    CodStatusContato = x.CodStatusContato,
                    DscStatusContato = x.DscStatusContato,
                    IndContatoRealizado = x.IndContatoRealizado == null ? "" : (x.IndContatoRealizado == "S" ? "Sim" : "Não"),
                    IndAcordoRealizado = x.IndAcordoRealizado == null ? "" : (x.IndAcordoRealizado == "S" ? "Sim" : "Não"),
                    IndAtivo = x.IndAtivo == "N" ? "Inativo" : "Ativo"
                });

            // monta a ordenação
            switch (ordem)
            {
                case "dsc":
                    if (asc)
                        query = query.OrderBy(x => x.DscStatusContato);
                    else
                        query = query.OrderByDescending(a => a.DscStatusContato);
                    break;
                case "contato":
                    if (asc)
                        query = query.OrderBy(x => x.IndContatoRealizado);
                    else
                        query = query.OrderByDescending(a => a.IndContatoRealizado);
                    break;
                case "acordo":
                    if (asc)
                        query = query.OrderBy(x => x.IndAcordoRealizado);
                    else
                        query = query.OrderByDescending(a => a.IndAcordoRealizado);
                    break;
                case "status":
                    if (asc)
                        query = query.OrderBy(x => x.IndAtivo);
                    else
                        query = query.OrderByDescending(a => a.IndAtivo);
                    break;
                default:
                    query = query.OrderBy(x => x.DscStatusContato);
                    break;
            }

            return query;
        }

        private IQueryable<DownloadLogStatusContatoResponse> QueryLogDownload()
        {
            return _db.LogStatusContato.Select(x => new DownloadLogStatusContatoResponse
            {
                CodStatusContato = x.CodStatusContato,
                Operacao = x.Operacao,
                DatLog = x.DatLog,
                CodUsuario = x.CodUsuario,
                DscStatusContatoA = x.DscStatusContatoA,
                DscStatusContatoD = x.DscStatusContatoD,
                IndContatoRealizadoA = x.IndContatoRealizadoA,
                IndContatoRealizadoD = x.IndContatoRealizadoD,
                IndAcordoRealizadoA = x.IndAcordoRealizadoA,
                IndAcordoRealizadoD = x.IndAcordoRealizadoD,
                IndAtivoA = x.IndAtivoA,
                IndAtivoD = x.IndAtivoD
            }).OrderByDescending(x => x.DatLog);
        }
        #endregion
    }
}
