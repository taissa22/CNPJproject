using Oi.Juridico.Contextos.V2.AgendaAudienciaContext.Data;
using Oi.Juridico.WebApi.V2.Areas.AgendaAudiencia.DTOs;

namespace Oi.Juridico.WebApi.V2.Areas.AgendaAudiencia.Controllers
{
    /// <summary>
    /// Api associação das permissões de módulos
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AgendaAudienciaController : ControllerBase
    {
        private readonly AgendaAudienciaContext _db;
        private readonly ILogger<AgendaAudienciaController> _logger;

        /// <summary>
        /// Injeção das dependencias da Api.
        /// </summary>
        public AgendaAudienciaController(AgendaAudienciaContext db, ILogger<AgendaAudienciaController> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Obtém os estados cadastrados no banco de dados
        /// </summary>
        /// <returns>Lista de comarca, empresa, estado, preposto, escritorio e advogado</returns>
        [HttpGet, Route("CarregarFiltros")]
        public async Task<IActionResult> CarregarFiltros()
        {
            FiltrosAgendaAudienciaResponse data = new FiltrosAgendaAudienciaResponse();

            #region UsuarioLogado
            var usuario = await _db.AcaUsuario.AsNoTracking()
                .Where(x => x.CodUsuario == User.Identity!.Name)
                .Select(x => new
                {
                    EhEscritorio = x.AcaUsuarioEscritorio.Any(p => p.CodProfissionalNavigation.IndEscritorio == "S")  ,
                    CodUsuario = x.CodUsuario
                }).FirstOrDefaultAsync();                       

            #endregion UsuarioLogado

            #region ListaComarca
            //EM LINQ NA V1

            var _listaComarca = await _db.Comarca
                .Select(s => new ComarcaResponse
                {
                    Id = s.CodComarca,
                    Descricao = s.CodEstado + " - " + s.NomComarca
                })
                .OrderBy(o => o.Descricao)
                .AsNoTracking()
                .ToListAsync();

            #endregion ListaComarca

            #region ListaEmpresa
            //EM LINQ NA V1

            var _listaEmpresa = await _db.Parte
               .Where(a => a.CodTipoParte.Equals("E"))
               .AsNoTracking()
               .OrderBy(s => s.NomParte)
                .ThenBy(s => s.NomParte)
                .Select(dto => new EmpresaDoGrupoResponse()
                {
                    Id = dto.CodParte,
                    Nome = dto.NomParte
                }).ToListAsync();

            #endregion ListaEmpresa

            #region ListaEstado
            //EM LINQ NA V1

            var _listaEstado = await _db.Estado
                .Select(e => new EstadoResponse()
                {
                    Id = e.CodEstado,
                    Descricao = string.Format("{0} - {1}", e.CodEstado, e.NomEstado)
                }).OrderBy(e => e.Id).ToListAsync();

            #endregion ListaEstado

            #region ListaPreposto
            //EM SQL NA V1

            var _listaPreposto = (from p in (from preposto in ((from preposto in _db.Preposto
                                                                join ap in _db.AudienciaProcesso on preposto.CodPreposto equals ap.CodPreposto
                                                                join proc in _db.Processo on ap.CodProcesso equals proc.CodProcesso
                                                                where preposto.IndPrepostoTrabalhista == "N"
                                                                select preposto.CodPreposto).Union
                        (from pp in _db.Preposto
                         where pp.IndPrepostoTrabalhista == "S"
                         select pp.CodPreposto)).Distinct()
                                             select new
                                             {
                                                 CodPreposto = preposto
                                             })
                                  join pre in _db.Preposto on p.CodPreposto equals pre.CodPreposto
                                  orderby pre.IndPrepostoAtivo == "S" && pre.IndPrepostoTrabalhista == "S" ? pre.NomPreposto : pre.NomPreposto + " [Inativo]"
                                  select new PrepostoResponse()
                                  {
                                      Id = pre.CodPreposto,
                                      Descricao = pre.IndPrepostoAtivo == "S" && pre.IndPrepostoTrabalhista == "S" ? pre.NomPreposto : pre.NomPreposto + " [Inativo]"
                                  }).ToList();

            #endregion ListaPreposto

            #region ListaEscritorio
            //EM SQL NA V1 RecuperarEscritorioTrabalhistaFiltroUsuarioEscritorio OU RecuperarEscritorioTrabalhistaFiltro
            //Primeira consulta recebe o codUsuario
            //Condições do IF ehEscritorio
                        
            List<EscritorioResponse> _listaEscritorio = new List<EscritorioResponse>();            

            if (usuario.EhEscritorio)
            {
                var query1 = from p in _db.Profissional
                             join ap in _db.AudienciaProcesso on p.CodProfissional equals ap.AdvesCodProfissional
                             join p2 in _db.Processo on ap.CodProcesso equals p2.CodProcesso
                             where p.IndEscritorio == "S" && p.IndAreaTrabalhista == "N" && p2.CodTipoProcesso == 2
                             select new EscritorioResponse()
                             {
                                 Id = p.CodProfissional,
                                 Descricao = p.NomProfissional + " [Inativo]"
                             };


                var query2 = from p in _db.Profissional
                             join ap in _db.AudienciaProcesso on p.CodProfissional equals ap.AdvesCodProfissionalAcomp
                             join p2 in _db.Processo on ap.CodProcesso equals p2.CodProcesso
                             where p.IndEscritorio == "S" && p.IndAreaTrabalhista == "N" && p2.CodTipoProcesso == 2
                             select new EscritorioResponse()
                             {
                                 Id = p.CodProfissional,
                                 Descricao = p.NomProfissional + " [Inativo]"
                             };


                var query3 = from p in _db.Profissional
                             where p.IndEscritorio == "S" && p.IndAreaTrabalhista == "S" && p.AcaUsuarioEscritorio.Any(x => x.CodUsuario == usuario.CodUsuario)
                             select new EscritorioResponse()
                             {
                                 Id = p.CodProfissional,
                                 Descricao = p.NomProfissional
                             };

                var q1 = query1.Take(1).ToArray();
                var q2 = query2.Take(1).ToArray();
                var q3 = query3.ToArray();

                _listaEscritorio = q1.Union(q2).Union(q3).OrderBy(x => x.Id).ToList();
            }
            else
            {
                var query1 = from p in _db.Profissional
                             join ap in _db.AudienciaProcesso on p.CodProfissional equals ap.AdvesCodProfissional
                             join p2 in _db.Processo on ap.CodProcesso equals p2.CodProcesso
                             where p.IndEscritorio == "S" && p.IndAreaTrabalhista == "N" && p2.CodTipoProcesso == 2
                             select new EscritorioResponse()
                             {
                                 Id = p.CodProfissional,
                                 Descricao = p.NomProfissional + " [Inativo]"
                             };


                var query2 = from p in _db.Profissional
                             join ap in _db.AudienciaProcesso on p.CodProfissional equals ap.AdvesCodProfissionalAcomp
                             join p2 in _db.Processo on ap.CodProcesso equals p2.CodProcesso
                             where p.IndEscritorio == "S" && p.IndAreaTrabalhista == "N" && p2.CodTipoProcesso == 2
                             select new EscritorioResponse()
                             {
                                 Id = p.CodProfissional,
                                 Descricao = p.NomProfissional + " [Inativo]"
                             };


                var query3 = from p in _db.Profissional
                             where p.IndEscritorio == "S" && p.IndAreaTrabalhista == "S"
                             select new EscritorioResponse()
                             {
                                 Id = p.CodProfissional,
                                 Descricao = p.NomProfissional
                             };

                var q1 = query1.Take(1).ToArray();
                var q2 = query2.Take(1).ToArray();
                var q3 = query3.ToArray();

                _listaEscritorio = q1.Union(q2).Union(q3).OrderBy(x => x.Id).ToList();
            }

            #endregion ListaEscritorio

            #region ListaAdvogado
            //EM SQL NA V1 RecuperarAdvogadoEscritorioUsuarioEscritorio OU RecuperarAdvogadoEscritorio
            //Primeira consulta recebe o codUsuario
            //Condições do IF ehEscritorio

            List<AdvogadoEscritorioResponse> _listaAdvogado = new List<AdvogadoEscritorioResponse>();            

            if (usuario.EhEscritorio)
            {
                _listaAdvogado = (from advogado in _db.AdvogadoEscritorio
                                  join profissional in _db.Profissional on advogado.CodProfissional equals profissional.CodProfissional
                                  join escritorio in _db.AcaUsuarioEscritorio on profissional.CodProfissional equals escritorio.CodProfissional
                                  join acaUsuario in _db.AcaUsuario on escritorio.CodUsuario equals acaUsuario.CodUsuario
                                  where profissional.IndAreaTrabalhista == "S" && profissional.IndEscritorio == "S" && acaUsuario.CodUsuario == usuario.CodUsuario
                                  orderby (advogado.NomAdvogado.Trim() + " (" + profissional.NomProfissional.Trim() + ")") ascending
                                  select new AdvogadoEscritorioResponse()
                                  {
                                      Descricao = advogado.NomAdvogado.Trim() + " (" + profissional.NomProfissional.Trim() + ")",
                                      Id = advogado.CodProfissional,
                                      CodigoInterno = advogado.CodInterno
                                  }).ToList();                
            }
            else
            {
                _listaAdvogado = (from advogado in _db.AdvogadoEscritorio
                                join profissional in _db.Profissional on advogado.CodProfissional equals profissional.CodProfissional
                                where profissional.IndAreaTrabalhista == "S"
                                orderby(advogado.NomAdvogado)
                                select new AdvogadoEscritorioResponse()
                                {
                                    Id = advogado.CodAdvogado,
                                    CodigoInterno = advogado.CodProfissional,
                                    Descricao = (string.Format("{0}{1}{2}{3}", advogado.NomAdvogado, " (", profissional.NomProfissional, ")"))

                                    }).ToList();
                
            }

            #endregion ListaAdvogado

            //Retorno DATA
            data.ListaComarca = _listaComarca;
            data.ListaEmpresa = _listaEmpresa;
            data.ListaEstado = _listaEstado;
            data.ListaPreposto = _listaPreposto;
            data.ListaEscritorio = _listaEscritorio;
            data.ListaAdvogado = _listaAdvogado;

            return Ok(new { data });
        }
    }
}
