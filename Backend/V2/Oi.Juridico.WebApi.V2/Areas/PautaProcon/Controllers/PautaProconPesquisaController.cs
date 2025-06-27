using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Contextos.V2.PautaJuizadoPesquisaContext.Data;
using Oi.Juridico.Contextos.V2.PautaJuizadoPesquisaContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.PautaJuizado.DTOs;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;

namespace Oi.Juridico.WebApi.V2.Areas.PautaJuizado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautaProconPesquisa : ControllerBase
    {
        private PautaJuizadoPesquisaContext _pautaJuizadoPesquisaContext;
        private ParametroJuridicoContext _parametroJuridicoContext;

        public PautaProconPesquisa(PautaJuizadoPesquisaContext pautaJuizadoPesquisaContext, ParametroJuridicoContext parametroJuridicoContex)
        {
            _pautaJuizadoPesquisaContext = pautaJuizadoPesquisaContext;
            _parametroJuridicoContext = parametroJuridicoContex;
        }

        [HttpGet("ListaTipoAudiencia")]
        public async Task<IActionResult> ListaTipoAudienciaAsync(CancellationToken ct)
        {
            var data = await _pautaJuizadoPesquisaContext.TipoAudiencia
                .AsNoTracking()
                .Where(ta => ta.IndProcon == "S")
                .OrderBy(ta => ta.DscTipoAudiencia)
                .Select(x => new TipoAudiencia
                {
                    CodTipoAud = x.CodTipoAud,
                    DscTipoAudiencia = x.DscTipoAudiencia
                }).ToListAsync(ct);

            return Ok(new { query = data });
        }

        [HttpGet("ListaJuizadosPorComarca/{comarca}")]
        public async Task<IActionResult> ListaJuizadosPorComarca(int comarca, CancellationToken ct)
        {
            var data = await (from vara in _pautaJuizadoPesquisaContext.Vara.AsNoTracking()
                              join tipovara in _pautaJuizadoPesquisaContext.TipoVara
                              on vara.CodTipoVara equals tipovara.CodTipoVara
                              where tipovara.IndProcon == "S" && vara.CodComarca == comarca
                              select new ListarJuizadoPorComarcaResponse
                              {
                                  NomTipoVara = vara.CodVara.ToString() + " º PROCON " + tipovara.NomTipoVara,
                                  CodVara = vara.CodVara.ToString() + "," + tipovara.CodTipoVara
                              }).ToListAsync(ct);

            return Ok(new { query = data.OrderBy(j => j.NomTipoVara) });

        }

        [HttpPost("ListaEmpresasDoGrupo")]
        public async Task<IActionResult> ListaEmpresasDoGrupoAsync(ListarEmpresasDoGrupoCommand filtro, CancellationToken ct)
        {
            var usuario = await _pautaJuizadoPesquisaContext.AcaUsuario.AsNoTracking()
                .Where(x => x.CodUsuario == User.Identity!.Name)
                .Select(x => new
                {
                    EhContador = x.CodProfissional.Any(p => p.IndContador == "S"),
                    EhEscritorio = x.CodProfissional.Any(p => p.IndEscritorio == "S")
                }).FirstOrDefaultAsync(ct);

            if (usuario != null)
            {
                if (usuario.EhContador == false && usuario.EhEscritorio == false)
                {
                    var data = await (from parte in _pautaJuizadoPesquisaContext.Parte.AsNoTracking()
                                      join usuarioRegional in _pautaJuizadoPesquisaContext.UsuarioRegional
                                      on parte.CodRegional equals usuarioRegional.CodRegional
                                      where parte.CodTipoParte == "E" && usuarioRegional.CodTipoProcesso == (short)TipoProcessoEnum.Procon && usuarioRegional.CodUsuario == filtro.Login
                                      orderby parte.NomParte
                                      select new Parte
                                      {
                                          CodParte = parte.CodParte,
                                          NomParte = parte.NomParte
                                      }).ToListAsync(ct);

                    return Ok(new { query = data });
                }
                else
                {
                    var data = await _pautaJuizadoPesquisaContext.Parte.AsNoTracking()
                                                                   .Where(parte => parte.CodTipoParte == "E")
                                                                   .OrderBy(parte => parte.NomParte)
                                                                   .Select(x => new Parte
                                                                   {
                                                                       CodParte = x.CodParte,
                                                                       NomParte = x.NomParte
                                                                   }).ToListAsync(ct);

                    return Ok(new { query = data });
                }
            }
            else
            {
                return Problem("Usuário não encontrado");
            }
        }

        [HttpGet("ListaGrupoProcon")]
        public async Task<IActionResult> ListaGrupoProcon(CancellationToken ct)
        {
            var data = await _pautaJuizadoPesquisaContext.GrupoProcon
                .AsNoTracking()
                .OrderBy(ta => ta.Descricao)
                .Select(x => new GrupoProcon
                {
                    Id = x.Id,
                    Descricao = x.Descricao
                }).ToListAsync(ct);

            return Ok(new { query = data });
        }
    }
}
