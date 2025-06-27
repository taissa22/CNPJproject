using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Data;
using Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Entities;
using Oi.Juridico.Contextos.V2.Extensions;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Seguranca;
using Oi.Juridico.Shared.V2.Services;
using Oi.Juridico.WebApi.V2.DTOs.ControleDeAcesso.Accounts;
using System.Text.Json;
using System.Threading;

namespace Oi.Juridico.WebApi.V2.Areas.ControleDeAcesso.Controllers
{
    /// <summary>
    /// Api Account Resposável pelo controle de acessos.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly ControleDeAcessoContext _db;
        private readonly ParametroJuridicoContext _dbPJ;
        private readonly JwtService _jwtService;
        private readonly ILogger<AccountsController> _logger;
        private IDistributedCache _cache;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        public AccountsController(JwtService jwtService, ControleDeAcessoContext db, ParametroJuridicoContext dbPJ, ILogger<AccountsController> logger, IDistributedCache cache)
        {
            _jwtService = jwtService;
            _db = db;
            _dbPJ = dbPJ;
            _logger = logger;
            _cache = cache;
        }

        /// <summary>
        /// Metodo Teste Login
        /// </summary>
        [HttpPost, AllowAnonymous, Route("Login")]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest model, CancellationToken ct)
        {
            try
            {
                bool credenciaisValidas = false;
                AcaUsuario? usuario = null;

                if (model.GrantType.Equals("password"))
                {
                    var senhaCriptografada = Criptografador.CriptografarSenha(model.Password);
                    usuario = await _db.AcaUsuario
                        .AsNoTracking()
                        .Where(a => a.CodUsuario == model.Username && a.Senha == senhaCriptografada)
                        .Where(a => model.Username == "SISJUR_JOB" || (model.Username != "SISJUR_JOB" && a.IndAtivo == "S"))
                        .Select(a => new AcaUsuario { CodUsuario = a.CodUsuario, NomeUsuario = a.NomeUsuario })
                        .FirstOrDefaultAsync(ct);
               

                    if (usuario is null)
                    {
                        return Unauthorized();
                    }

                    credenciaisValidas = usuario != null;

                }
                else if (model.GrantType == "refresh_token")
                {
                    if (!String.IsNullOrWhiteSpace(model.RefreshToken))
                    {
                        RefreshToken? refreshTokenBase = null;

                        string strTokenArmazenado =
                           _cache.GetString(model.RefreshToken);
                        if (!String.IsNullOrWhiteSpace(strTokenArmazenado))
                        {
                            refreshTokenBase = JsonSerializer
                                .Deserialize<RefreshToken>(strTokenArmazenado);
                        }

                        credenciaisValidas = (refreshTokenBase != null &&
                            model.Username == refreshTokenBase.Username &&
                            model.RefreshToken == refreshTokenBase.Token);

                        usuario = await _db.AcaUsuario
                            .AsNoTracking()
                            .Where(x => x.CodUsuario == model.Username)
                            .Where(x => model.Username == "SISJUR_JOB" || (model.Username != "SISJUR_JOB" && x.IndAtivo == "S"))
                            .Select(x => new AcaUsuario { CodUsuario = x.CodUsuario, NomeUsuario = x.NomeUsuario })
                            .FirstOrDefaultAsync(ct);

                        if (usuario is null)
                        {
                            return Unauthorized();
                        }

                        // Elimina o token de refresh já que um novo será gerado
                        if (credenciaisValidas && usuario != null ) 
                            _cache.Remove(model.RefreshToken);                        
                    }
                }

                if (credenciaisValidas)
                {
                    var jwt = _jwtService.CreateJsonWebToken(usuario!.NomeUsuario, usuario!.CodUsuario, string.Empty);

                    return Ok(new LoginResponse(jwt.AccessToken, jwt.TokenType, jwt.ExpiresIn, jwt.RefreshToken.Token));
                } else
                {
                    return Unauthorized();
                }                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Obtém os dados do usuário logado
        /// </summary>
        /// <returns>Dados do usuário logado</returns>
        [HttpGet, Route("User")]
        public async Task<ActionResult<GetUserResponse>> GetUserAsync(CancellationToken ct)
        {
            var usuario = await _db.AcaUsuario
                .AsNoTracking()
                .Where(x => x.CodUsuario == User.Identity!.Name)
                .Select(x => new
                {
                    Codigo = x.CodUsuario,
                    CodigoOrigemUsuario = x.CodOrigemUsuario,
                    EhPerfilWeb = x.IndPerfilWeb == "S",
                    Email = x.DscEmail,
                    Nome = x.NomeUsuario,
                })
                .FirstOrDefaultAsync(ct);

            if (usuario == null)
            {
                return Problem("Usuário não encontrado");
            }
            else
            {
                return Ok(new GetUserResponse
                {
                    Username = usuario.Codigo,
                    Nome = usuario.Nome,
                    Permissoes = (await _db.ObtemPermissoesAsync(User.Identity!.Name)).Select(x => x.CodMenu).ToArray(),
                    Ambiente = (await _dbPJ.ParametroJuridico.FirstAsync(x => x.CodParametro == ParametrosJuridicos.NOME_BANCO, ct)).DscConteudoParametro,
                    EhEscritorio = usuario.CodigoOrigemUsuario == 5
                });
            }
        }
    }

}