using HashidsNet;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.ManutencaoSolicitanteContext.Data;
using Oi.Juridico.Contextos.V2.PermissaoContext.Extensions;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.Solicitante.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.Solicitante.DTOs;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Solicitante
{
    [Route("api/manutencao/[controller]")]
    [ApiController]
    public class SolicitantesController : ControllerBase
    {
        private readonly ManutencaoSolicitanteContext _db;
        private readonly IHashids _hashids;
        private readonly ILogger<SolicitantesController> _logger;

        public SolicitantesController(ManutencaoSolicitanteContext db, ILogger<SolicitantesController> logger, IHashids hashids)
        {
            _db = db;
            _logger = logger;
            _hashids = hashids;
        }

        [HttpPost("listar")]
        public async Task<IActionResult> ListarAsync(CancellationToken ct, [FromQuery] string? nome, [FromQuery] string? ordem, [FromQuery] bool asc = true, [FromQuery] int page = 0, [FromQuery] int size = 8)
        {
            try
            {
                using var scope = _db.Database.BeginTransaction();
                
                IQueryable<ListarResponse> query = QueryListar(nome, ordem, asc);

                // retorna a lista da página
                var lista = await query
                    .Skip(page * size).Take(size)
                    .ToArrayAsync(ct);

                // monta os hashs
                foreach (var item in lista)
                    item.CodSolicitanteHash = _hashids.Encode(item.CodSolicitante!.Value);

                // conta o total de linhas
                var total = await query.CountAsync(ct);

                // conta o total de lista da página
                var totalLista = lista.Length;

                return Ok(new { lista, total, totalLista });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem("Ocorreu um erro na execução da consulta.");
            }
        }

        [HttpPost("salvar")]
        public async Task<IActionResult> SalvarAsync(CancellationToken ct, [FromBody] SalvarRequest model)
        {
            try
            {
                Contextos.V2.ManutencaoSolicitanteContext.Entities.Solicitante? solicitante = null;

                // inclusão
                if (model.CodSolicitante.HasValue == false || model.CodSolicitante == 0)
                {
                    solicitante = new();
                    _db.Solicitante.Add(solicitante);
                }
                // alteração
                else
                {
                    solicitante = await _db.Solicitante
                        .FirstOrDefaultAsync(x => x.CodSolicitante == model.CodSolicitante, ct);

                    if (solicitante == null)
                        return NotFound("Solicitante não encontrado.");
                }

                solicitante.Nome = model.Nome;
                solicitante.Email = model.Email.ToLower();

                await _db.SaveChangesAsync(User.Identity!.Name, true, ct);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem("Não foi possível salvar o solicitante.");
            }
        }

        [HttpDelete("excluir")]
        public async Task<IActionResult> ExcluirAsync(CancellationToken ct, [FromQuery] string codSolicitante)
        {
            try
            {
                var id = _hashids.Decode(codSolicitante);

                if (id.Length == 0)
                    return NotFound("Solicitante não encontrado.");

                using var scope = _db.Database.BeginTransaction();

                _db.ExecutarProcedureDeLog(User.Identity!.Name, true);

                var qtdeExcluidos = await _db.Solicitante
                        .Where(x => x.CodSolicitante == id[0])
                        .ExecuteDeleteAsync(ct);

                scope.Commit();

                return qtdeExcluidos > 0 ? Ok() : NotFound("Solicitante não encontrado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                if (ex.Message.StartsWith("ORA-02292"))
                    return Problem("O solicitante está sendo referenciado em outra parte do sistema.");
                else
                    return Problem("Não foi possível excluir o solicitante.");
            }
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadAsync(CancellationToken ct, [FromQuery] string? nome, [FromQuery] string? ordem, [FromQuery] bool asc = true)
        {
            try
            {
                using var scope = _db.Database.BeginTransaction();

                var lista = await QueryListar(nome, ordem, asc).ToListAsync(ct);

                var nomeArquivo = $"Solicitantes_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                var csv = lista.ToCsvByteArray(typeof(ListarResponseMap), false);

                var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

                return File(bytes, "application/octet-stream", nomeArquivo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem("Ocorreu um erro na geração do arquivo.");
            }

        }

        private IQueryable<ListarResponse> QueryListar(string? nome, string? ordem, bool asc)
        {
            _db.PesquisarPorCaseInsensitive();

            // monta a query
            var query = _db.Solicitante
                .AsNoTracking()
                .WhereIfNotEmpty(x => x.Nome.Contains(nome!), nome)
                .Select(x => new ListarResponse
                {
                    CodSolicitante = x.CodSolicitante,
                    Nome = x.Nome,
                    Email = x.Email,
                });

            // monta a ordenação
            switch (ordem)
            {
                case "email":
                    if (asc)
                        query = query.OrderBy(x => x.Email);
                    else
                        query = query.OrderByDescending(a => a.Email);
                    break;

                case "codigo":
                    if (asc)
                        query = query.OrderBy(x => x.CodSolicitante);
                    else
                        query = query.OrderByDescending(a => a.CodSolicitante);
                    break;

                case "nome":
                default:
                    if (asc)
                        query = query.OrderBy(x => x.Nome);
                    else
                        query = query.OrderByDescending(a => a.Nome);
                    break;
            }

            return query;
        }
    }
}
