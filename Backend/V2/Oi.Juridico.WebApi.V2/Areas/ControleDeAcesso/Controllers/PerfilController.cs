using Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Data;
using Oi.Juridico.WebApi.V2.Areas.ControleDeAcesso.DTOs.Perfil;


namespace Oi.Juridico.WebApi.V2.Areas.ControleDeAcesso.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly ControleDeAcessoContext _db;
        private readonly ILogger<PerfilController> _logger;

        public PerfilController(ControleDeAcessoContext db, ILogger<PerfilController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpDelete("excluir")]
        public async Task<IActionResult> ExcluirPerfilAsync([FromQuery] string codPerfil, CancellationToken ct)
        {
            try
            {
                _db.ExecutarProcedureDeLog(User.Identity!.Name!, true);

                var perfil = await _db.AcaUsuario.FirstOrDefaultAsync(x => x.CodUsuario == codPerfil && x.Perfil == "S", ct);

                if (perfil == null)
                {
                    return NotFound("Perfil não encontrado.");
                }

                await _db.Database.ExecuteSqlInterpolatedAsync($"BEGIN JUR.EXCLUIR_PERFIL({codPerfil}); END;");

                return Ok("Perfil excluído.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        [HttpGet("obter-usuarios")]
        public async Task<object> ObterUsuariosAsync(string codPerfil, bool base64, CancellationToken ct)
        {
            if (base64)
            {
                codPerfil = BitConverter.ToString(Convert.FromBase64String(codPerfil));
            }

            var query = from r in _db.AcaUsuario.AsNoTracking()
                        where r.Perfil == "N" && r.IndPerfilWeb == "N"
                        join ad in _db.AcaUsuarioDelegacao.Where(x => x.CodUsuarioDelegacao == codPerfil) on r.CodUsuario equals ad.CodUsuario into leftJoin
                        from ad in leftJoin.DefaultIfEmpty()
                        select new { r.CodUsuario, r.NomeUsuario, r.DscEmail, Associado = ad != null, Ativo = r.IndAtivo == "S" };

            var query2 = query
                .Where(x => x.Associado || (x.Associado == false && x.Ativo))
                .OrderBy(x => x.NomeUsuario)
                .Select(x => new ObterUsuariosResponse
                {
                    Codigo = x.CodUsuario,
                    Nome = x.Ativo ? x.NomeUsuario + " - " + x.CodUsuario : x.NomeUsuario + " - " + x.CodUsuario + " - INATIVO",
                    Email = x.DscEmail,
                    Associado = x.Associado,
                    Ativo = x.Ativo
                });

            var lista = await query2.ToListAsync(ct);

            var qtdeAssociados = lista.Where(x => x.Associado).Count();
            var qtdeNaoAssociados = lista.Where(x => !x.Associado).Count();

            return new { lista, qtdeAssociados, qtdeNaoAssociados };
        }
    }
}
