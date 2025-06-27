using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.PerfilContext.Data;
using Oi.Juridico.Contextos.V2.PermissaoContext.Extensions;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.DTOs.Perfil;
using Oi.Juridico.WebApi.V2.DTOs.Perfil.CsvHelperMap;
using Oi.Juridico.WebApi.V2.DTOs.Permissao.CsvHelperMap;

namespace Oi.Juridico.WebApi.V2.Areas.Perfil.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly PerfilContext _db;
        private readonly ILogger<PerfilController> _logger;

        public PerfilController(PerfilContext db, ILogger<PerfilController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet, Route("Obter")]
        public async Task<IActionResult> ObterPerfilAsync(CancellationToken ct, [FromQuery] int pagina, [FromQuery] int quantidade, [FromQuery] string? filtro, [FromQuery] string status = "Todos", [FromQuery] string modulo = "Todos", [FromQuery] string coluna = "padrao", [FromQuery] string direcao = "asc")
        {
            try
            {
                var lstPerfis = ObterPerfil(filtro, coluna, status, modulo, string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), false);
                var total = lstPerfis.Count();              

                var perfil = lstPerfis.Skip((pagina - 1) * quantidade).Take(quantidade);

                return Ok(new { total, perfil });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        [HttpDelete, Route("Excluir")]
        public async Task<IActionResult> ExcluirPerfil(CancellationToken ct, string cod)
        {
            try
            {
                var perfil = _db.AcaUsuario.FirstOrDefault(x => x.CodUsuario == cod);
                if (perfil is null)
                {
                    return BadRequest("Perfil não encontrado");
                }
                _db.Remove(perfil);
                await _db.SaveChangesAsync(ct);

                return Ok("Excluido com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        [HttpGet, Route("Exportar")]
        public async Task<ActionResult> ExportarAsync(CancellationToken ct, [FromQuery] string? filtro, [FromQuery] string? status = "Todos", [FromQuery] string? modulo = "Todos", [FromQuery] string coluna = "padrao", [FromQuery] string direcao = "asc")
        {
            try
            {
            var perfil = ObterPerfil(filtro, coluna, status, modulo, string.IsNullOrEmpty(direcao) || direcao.Equals("asc"), true);

            var csv = perfil.ToList()
                                .ToCsvByteArray(typeof(PerfilResponseMap), header: ">>> PERFIS", textoComAspas: true);

            DateTime dtExportacao = DateTime.Now;
            string nomeArquivo = $"relatorio_juridico_perfis_{dtExportacao:yyyy_MM_dd}_{dtExportacao.Hour}h{dtExportacao.Minute}m{3}s_{dtExportacao.Second}_{this.GeraValorAleatorioParaNomeDeArquivo()}.csv";
            return File(csv, "application/csv", nomeArquivo);
        }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
           
        }

        #region METHODS
        private IQueryable<PerfilResponse> ObterPerfil(string? filtro, string coluna, string status, string modulo, bool asc, bool exportar)
        {            
            IQueryable<PerfilResponse>? query;

            if (exportar)
            {
                query = from u in _db.AcaUsuario
                        join g in _db.AcaUsuario on u.CodGestorDefault equals g.CodUsuario
                        into ug
                        from nomeGestor in ug.DefaultIfEmpty()
                        join f in _db.AcaRUsuarioFuncao on u.CodUsuario equals f.CodUsuario
                        into uf
                        from usuarioFuncao in uf.DefaultIfEmpty()
                        join m in _db.AcaMenu on usuarioFuncao.CodMenu equals m.CodMenu
                        into am
                        from acaMenu in am.DefaultIfEmpty()
                        where u.Perfil == "S"
                        orderby u.CodUsuario
                        select new PerfilResponse
                        {
                            Nome = u.CodUsuario,
                            Restrito = u.IndRestrito == "S" ? "SIM" : "NÃO",
                            Descricao = u.NomeUsuario,
                            GestorDefault = nomeGestor.NomeUsuario == null ? "(Sem gestor default)" : nomeGestor.NomeUsuario,
                            Modulo = u.IndPerfilWeb == "S" ? "Web" : "Interno",
                            NomePermissao = acaMenu.DescricaoMenu,
                            Status = u.IndAtivo == "S" ? "Ativo" : "Inativo",
                        };
            }
            else
            {
                query = from u in _db.AcaUsuario
                        join g in _db.AcaUsuario
                        on u.CodGestorDefault equals g.CodUsuario
                        into ug
                        from nomeGestor in ug.DefaultIfEmpty()
                        where u.Perfil == "S"
                        orderby u.CodUsuario
                        select new PerfilResponse
                        {
                            Nome = u.CodUsuario,
                            Restrito = u.IndRestrito == "S" ? "SIM" : "NÃO",
                            Descricao = u.NomeUsuario,
                            GestorDefault = nomeGestor.NomeUsuario == null ? "(Sem gestor default)" : nomeGestor.NomeUsuario,
                            Modulo = u.IndPerfilWeb == "S" ? "Web" : "Interno",
                            Status = u.IndAtivo == "S" ? "Ativo" : "Inativo",
                        };
            }



            var newQuery = query
                            .AsNoTracking()
                            .WhereIfNotEmpty(x => x.Descricao.ToLower().Contains(filtro!.ToLower()) || x.Nome.ToLower().Contains(filtro!.ToLower()), filtro)
                            .Where(x => (x.Status == status && status != "Todos") || (status == "Todos"))
                            .Where(x => (x.Modulo == modulo && modulo != "Todos") || (modulo == "Todos"));

            if (status != "Todos" || modulo != "Todos")
            {
                newQuery = IncluirFiltroStatusModulo((IOrderedQueryable<PerfilResponse>)newQuery, status, modulo);
            }

            return IncluirOrdenacao(newQuery, coluna, asc);
        }

        private IOrderedQueryable<PerfilResponse> IncluirFiltroStatusModulo(IOrderedQueryable<PerfilResponse> query, string status, string modulo)
        {
            if (status == "Ativo")
            {
                query = (IOrderedQueryable<PerfilResponse>)query.Where(x => x.Status == "Ativo");
            }
            else if (status == "Inativo")
            {
                query = (IOrderedQueryable<PerfilResponse>)query.Where(x => x.Status == "Inativo");
            }

            if (modulo == "Web")
            {
                query = (IOrderedQueryable<PerfilResponse>)query.Where(x => x.Modulo == "Web");
            }
            else if (modulo == "Interno")
            {
                query = (IOrderedQueryable<PerfilResponse>)query.Where(x => x.Modulo == "Interno");
            }

            return query;
        }

        private IQueryable<PerfilResponse> IncluirOrdenacao(IQueryable<PerfilResponse> query, string coluna, bool asc)
        {
            switch (coluna)
            {
                case "descricao":
                    if (asc)
                        query = query.OrderBy(x => x.Descricao != null).ThenBy(a => a.Descricao);
                    else
                        query = query.OrderByDescending(a => a.Descricao);
                    break;

                case "gestor":
                    if (asc)
                        query = query.OrderBy(x => x.GestorDefault != null).ThenBy(a => a.GestorDefault);
                    else
                        query = query.OrderByDescending(a => a.GestorDefault);
                    break;
                case "modulo":
                    if (asc)
                        query = query.OrderBy(x => x.Modulo != null).ThenBy(a => a.Modulo);
                    else
                        query = query.OrderByDescending(a => a.Modulo);
                    break;
                case "status":
                    if (asc)
                        query = query.OrderBy(x => x.Status != null).ThenBy(a => a.Status);
                    else
                        query = query.OrderByDescending(a => a.Status);
                    break;
                case "nome":
                    if (asc)
                        query = query.OrderBy(x => x.Nome != null).ThenBy(a => a.Nome);
                    else
                        query = query.OrderByDescending(a => a.Nome);
                    break;
                case "padrao":
                default:
                    query = query.OrderByDescending(x => x.Modulo).ThenBy(a => a.Nome);                  
                    break;
            }
            return query;
        }

        private string GeraValorAleatorioParaNomeDeArquivo()
        {
            var random = new Random(DateTime.Now.Millisecond);
            return $"{random.Next(1, 9999):0000}";
        }
        #endregion METHODS
    }
}
