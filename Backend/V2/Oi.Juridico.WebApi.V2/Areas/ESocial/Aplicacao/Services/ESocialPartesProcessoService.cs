using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Enums.Functions;
using Oi.Juridico.Shared.V2.Enums;

using System.Security.Claims;
using Oi.Juridico.Contextos.V2.ESocialLogContext.Data;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Services
{
    public class ESocialPartesProcessoService
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ESocialLogDbContext _eSocialLogDbContext;
        private readonly ESocialDownloadRetornoService _eSocialRetornoService;

        public ESocialPartesProcessoService(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ESocialLogDbContext eSocialLogDbContext, ESocialDownloadRetornoService eSocialRetornoService)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _eSocialLogDbContext = eSocialLogDbContext;
            _eSocialRetornoService = eSocialRetornoService;
        }

        #region Funções Gerais

        public async Task<List<ESocialPartesProcessoDTO>> ListaFormulariosParteProcesso(int? statusFormulario, IQueryable<ESocialPartesProcessoDTO> queryResultado, CancellationToken ct)
        {
            var listaParteProcesso = await queryResultado.OrderBy(x => x.NomeParte).ToListAsync(ct);

            foreach (var parte in listaParteProcesso)
            {
                var listaFormularios2500 = await _eSocialDbContext.EsF2500.AsNoTracking().Include(x => x.LogCodUsuarioNavigation).Where(x => x.CodParte == parte.CodParte && x.CodProcesso == parte.CodProcesso).ToListAsync();
                var listaFormularios2500Log = await _eSocialLogDbContext.LogEsF2500.AsNoTracking().Where(x => x.CodParte == parte.CodParte && x.CodProcesso == parte.CodProcesso).OrderByDescending(x => x.LogDataOperacao).ToListAsync();


                if (listaFormularios2500.Any())
                {
                    var ultimoItem2500 = listaFormularios2500.MaxBy(x => x.IdF2500)!;
                    var nomeArquivo = string.Empty;

                    if (ultimoItem2500.StatusFormulario == EsocialStatusFormulario.Exclusao3500NaoOk.ToByte())
                    {
                        nomeArquivo = ultimoItem2500.NomeArquivoExclusao;
                    }
                    else
                    {
                        nomeArquivo = ultimoItem2500.NomeArquivoEnviado;
                    }

                    var exibirRetorno2500 = _eSocialRetornoService.ExibirRetorno(_eSocialDbContext, ultimoItem2500.IdF2500, nomeArquivo, "F_2500");

                    parte.Formulario2500 = new ESocialF2500ConsultaDTO()
                    {
                        IdF2500 = ultimoItem2500.IdF2500,
                        LogCodUsuario = ultimoItem2500.LogCodUsuario,
                        NomeUsuario = ultimoItem2500.LogCodUsuarioNavigation.NomeUsuario,
                        IndRetificado = ultimoItem2500.IdeeventoIndretif == 2,
                        LogDataOperacao = ultimoItem2500.LogDataOperacao,
                        StatusFormulario = ultimoItem2500.StatusFormulario,
                        NroRecibo = ultimoItem2500.IdeeventoNrrecibo,
                        FinalizadoEscritorio = ultimoItem2500.FinalizadoEscritorio is null ? null : ultimoItem2500.FinalizadoEscritorio == "S",
                        FinalizadoContador = ultimoItem2500.FinalizadoContador is null ? null : ultimoItem2500.FinalizadoContador == "S",
                        VersaoEsocial = ultimoItem2500.VersaoEsocial,

                        Historico = listaFormularios2500.Select(h => new ESocialF2500ConsultaDTO()
                        {
                            IdF2500 = h.IdF2500,
                            LogCodUsuario = h.LogCodUsuario,
                            NomeUsuario = h.LogCodUsuarioNavigation.NomeUsuario,
                            LogDataOperacao = h.LogDataOperacao,
                            StatusFormulario = h.StatusFormulario,
                            IndRetificado = h.IdeeventoIndretif == 2,
                            NroRecibo = h.IdeeventoNrrecibo,
                            DataRetornoOK = listaFormularios2500Log.Where(l => l.IdF2500 == h.IdF2500
                                                                            && l.StatusFormularioA != EsocialStatusFormulario.RetornoESocialOk.ToByte()
                                                                            && l.StatusFormularioD == EsocialStatusFormulario.RetornoESocialOk.ToByte())
                                                                    .OrderByDescending(l => l.LogDataOperacao)
                                                                    .Select(l => l.LogDataOperacao).FirstOrDefault(),
                            DataRetornoExclusao = listaFormularios2500Log.Where(l => l.IdF2500 == h.IdF2500
                                                                            && l.StatusFormularioA != EsocialStatusFormulario.Excluido3500.ToByte()
                                                                            && l.StatusFormularioD == EsocialStatusFormulario.Excluido3500.ToByte())
                                                                    .OrderByDescending(l => l.LogDataOperacao)
                                                                    .Select(l => l.LogDataOperacao).FirstOrDefault(),
                            VersaoEsocial = h.VersaoEsocial
                        }).OrderByDescending(h => h.IdF2500).ToList(),
                        ExibirRetorno2500 = exibirRetorno2500
                    };
                }

                var listaFormularios2501Completa = await _eSocialDbContext.EsF2501.AsNoTracking().Include(x => x.LogCodUsuarioNavigation).Where(x => x.CodParte == parte.CodParte && x.CodProcesso == parte.CodProcesso).ToListAsync();
                var listaFormularios2501DTO = new List<ESocialF2501ConsultaDTO>();

                if (listaFormularios2501Completa is not null)
                {
                    var listaFormulariosPai = listaFormularios2501Completa.Where(x => x.ParentIdF2501 == 0).OrderBy(x => x.IdF2501).ToList();

                    foreach (var formulario in listaFormulariosPai)
                    {
                        var ultimoItem2501 = listaFormularios2501Completa.Any(x => x.ParentIdF2501 == formulario.IdF2501) ?
                            listaFormularios2501Completa.Where(x => x.ParentIdF2501 == formulario.IdF2501).MaxBy(x => x.LogDataOperacao)!
                            : listaFormularios2501Completa.Where(x => x.IdF2501 == formulario.IdF2501).FirstOrDefault()!;

                        var exibirRetorno2501 = _eSocialRetornoService.ExibirRetorno(_eSocialDbContext, ultimoItem2501.IdF2501, ultimoItem2501.NomeArquivoEnviado, "F_2501");

                        var formularioDTO = new ESocialF2501ConsultaDTO()
                        {
                            CodParte = ultimoItem2501.CodParte,
                            CodProcesso = ultimoItem2501.CodProcesso,
                            IdF2501 = ultimoItem2501.IdF2501,
                            LogCodUsuario = ultimoItem2501.LogCodUsuario,
                            NomeUsuario = ultimoItem2501.LogCodUsuarioNavigation.NomeUsuario,
                            IndRetificado = ultimoItem2501.IdeeventoIndretif == 2,
                            NroRecibo = ultimoItem2501.IdeeventoNrrecibo,
                            PeriodoApuracao = ultimoItem2501.IdeprocPerapurpgto,
                            LogDataOperacao = ultimoItem2501.LogDataOperacao,
                            StatusFormulario = ultimoItem2501.StatusFormulario,
                            FinalizadoEscritorio = ultimoItem2501.FinalizadoEscritorio is null ? null : ultimoItem2501.FinalizadoEscritorio == "S",
                            FinalizadoContador = ultimoItem2501.FinalizadoContador is null ? null : ultimoItem2501.FinalizadoContador == "S",
                            VersaoEsocial = ultimoItem2501.VersaoEsocial,
                            Historico = listaFormularios2501Completa.Where(h => h.ParentIdF2501 == formulario.IdF2501
                                                                            || h.IdF2501 == formulario.IdF2501).Select(h => new ESocialF2501ConsultaDTO()
                                                                            {
                                                                                IdF2501 = h.IdF2501,
                                                                                LogCodUsuario = h.LogCodUsuario,
                                                                                NomeUsuario = h.LogCodUsuarioNavigation.NomeUsuario,
                                                                                IndRetificado = h.IdeeventoIndretif == 2,
                                                                                LogDataOperacao = h.LogDataOperacao,
                                                                                StatusFormulario = h.StatusFormulario,
                                                                                NroRecibo = h.IdeeventoNrrecibo,
                                                                                PeriodoApuracao = h.IdeprocPerapurpgto,
                                                                                VersaoEsocial = h.VersaoEsocial,
                                                                            }).OrderByDescending(h => h.IdF2501).ToList(),
                            ExibirRetorno2501 = exibirRetorno2501
                        };

                        listaFormularios2501DTO.Add(formularioDTO);
                    }

                    listaFormularios2501DTO = listaFormularios2501DTO.OrderBy(x => x.StatusFormulario == EsocialStatusFormulario.Excluido3500.ToByte()).ThenBy(x => x.IdF2501).ToList();

                    parte.ListaFormularios2501 = listaFormularios2501DTO;

                }




            }

            listaParteProcesso = statusFormulario > -1 ? listaParteProcesso.Where(x => x.Formulario2500 is not null
            && x.Formulario2500!.Historico!.Any(y => y.StatusFormulario == statusFormulario)
            || x.ListaFormularios2501!.Any(h => h.Historico!.Any(j => j.StatusFormulario == statusFormulario))).ToList() : listaParteProcesso;

            return listaParteProcesso;
        }

        public async Task<bool> ValidaStatusFormularios(int codigoInterno, ClaimsPrincipal user, CancellationToken ct)
        {
            var queryParteProcesso = from espp in _eSocialDbContext.EsParteProcesso.AsNoTracking()
                                     where espp.CodProcesso == codigoInterno
                                     select espp;

            var processo = queryParteProcesso.Select(x => x.CodProcessoNavigation);

            if (await EscritorioEmpresaPodeAcessarAsync(processo, user, ct))
            {

                var queryResultado = queryParteProcesso.Select(espp => new ESocialPartesProcessoDTO
                {
                    CodParte = espp.CodParte,
                    CodProcesso = espp.CodProcesso,
                    NomeParte = espp.CodParteNavigation.NomParte,
                    CpfParte = espp.CodParteNavigation.CpfParte,
                    StatusReclamante = espp.StatusReclamante
                });

                var listaParteProcesso = await queryResultado.OrderBy(x => x.NomeParte).ToListAsync(ct);


                foreach (var lpp in listaParteProcesso)
                {
                    var listaFormularios2500 = await _eSocialDbContext.EsF2500.AsNoTracking().Include(x => x.LogCodUsuarioNavigation).Where(x => x.CodParte == lpp.CodParte && x.CodProcesso == lpp.CodProcesso && x.StatusFormulario != EsocialStatusFormulario.NaoIniciado.ToByte()).ToListAsync();

                    if (listaFormularios2500.Any())
                    {
                        return true;

                    }

                    var listaFormularios2501Completa = await _eSocialDbContext.EsF2501.AsNoTracking().Include(x => x.LogCodUsuarioNavigation).Where(x => x.CodParte == lpp.CodParte && x.CodProcesso == lpp.CodProcesso && x.StatusFormulario != EsocialStatusFormulario.NaoIniciado.ToByte()).ToListAsync();
                    var listaFormularios2501DTO = new List<ESocialF2501ConsultaDTO>();

                    if (listaFormularios2501Completa is not null)
                    {
                        return true;

                    }
                }
            }
            return false;
        }

        public async Task<DadosIdentificacaoNovoFormularioDTO> RecuperaDadosIdentificacaoNovoFormulario(EsParteProcesso esParteProcesso, byte eSocialCodigoTipoAmbiente, CancellationToken ct)
        {
            var processo = await (from p in _eSocialDbContext.Processo.AsNoTracking().Where(x => x.CodProcesso == esParteProcesso.CodProcesso)
                                  from pp in _eSocialDbContext.Parte.Where(x => x.CodParte == esParteProcesso.CodParte)
                                  from pp2 in _eSocialDbContext.Parte.Where(x => x.CodParte == p.CodParteEmpresa)
                                  select new
                                  {
                                      p.NroProcessoCartorio,
                                      CnpjEmpresa = p.CodParteEmpresaNavigation.CgcParte,
                                      UfComarca = p.CodComarcaNavigation.CodEstado,
                                      NomeComarca = p.CodComarcaNavigation.NomComarca,
                                      pp.CpfParte,
                                      pp.NomParte,
                                      pp.CgcParte,
                                      pp2.IdEsEmpresaAgrupadora
                                  }).FirstAsync(ct);

            var empresaAgrupadora = await _eSocialDbContext.EsEmpresaAgrupadora.AsNoTracking().FirstOrDefaultAsync(e => e.IdEsEmpresaAgrupadora == processo.IdEsEmpresaAgrupadora);

            var nroProcesso = processo.NroProcessoCartorio.Length >= 20 ? Regex.Replace(processo.NroProcessoCartorio, "[^0-9]+", "") : string.Empty;
            string versaoSisjur = ObtemVersaoSisjur();
            var qryMunicipioComarca = await (from mi in _eSocialDbContext.MunicipioIbge.AsNoTracking()
                                             where mi.CodEstado == processo.UfComarca && EF.Functions.Like(mi.NomMunicipio.ToUpper(), $"{processo.NomeComarca}")
                                             select mi.CodMunicipioIbge).ToListAsync(ct);

            int? codMunicipioComarca = qryMunicipioComarca.Count > 1 || qryMunicipioComarca.Count < 1 ? null : qryMunicipioComarca[0];

            string parametroVersaoAtualEsocial = await _parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("VRS_ATUAL_ESOCIAL");

            int parametroEventoTransitadoJugado = int.Parse(await _parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("COD_EV_TRANSITO_JULGADO"));

            var andamento = _eSocialDbContext.AndamentoProcesso.Where(x => x.CodProcesso == (int)esParteProcesso.CodProcesso && x.CodEvento == parametroEventoTransitadoJugado).OrderBy(x => x.DataCriacao);

            var cnpjIdeEmpregador = empresaAgrupadora != null && empresaAgrupadora.CnpjEmpresaPagadora != null ? empresaAgrupadora.CnpjEmpresaPagadora : processo.CnpjEmpresa;
            var cpfParte = processo.CpfParte is not null ? processo.CpfParte : string.Empty;

            var dadosFormulario = new DadosIdentificacaoNovoFormularioDTO
            {
                CodigoProcesso = esParteProcesso.CodProcesso,
                NroProcessoCartorio = nroProcesso,
                CodigoParte = esParteProcesso.CodParte,
                NomeParte = processo.NomParte is not null && processo.NomParte.Length > 70 ? processo.NomParte[..70] : processo.NomParte,
                CpfParte = cpfParte,
                UfComarca = processo.UfComarca,
                IdVara = nroProcesso.Length == 20 ? short.Parse(nroProcesso.Skip(nroProcesso.Length - 4).Take(4).ToArray()) : null,
                CodigoMunicipioComarca = codMunicipioComarca,
                VersaoSisjur = versaoSisjur,
                DataEventoTransito = andamento != null && andamento.Any() ? andamento.First().DatEvento : null,
                CnpjEmpregador = cnpjIdeEmpregador,
                TipoAmbiente = eSocialCodigoTipoAmbiente,
                VersaoEsocial = parametroVersaoAtualEsocial
            };

            return dadosFormulario;
        }

        public async Task<bool> EscritorioEmpresaPodeAcessarAsync(IQueryable<Processo> processo, ClaimsPrincipal user, CancellationToken ct)
        {
            var loginUsuario = user.Identity!.Name;

            if (processo is not null)
            {
                var usuarioEmpresaDoGrupo = _eSocialDbContext.AcaUsuarioEmpresaGrupo.Where(u => u.CodUsuario == loginUsuario).Select(s => s.CodParte);

                if (await usuarioEmpresaDoGrupo.AnyAsync(ct))
                {
                    processo = processo.Where(s => usuarioEmpresaDoGrupo.Contains((int)s.CodParteEmpresa!));
                }

                var usuarioEscritorio = _eSocialDbContext.AcaUsuarioEscritorio.Where(u => u.CodUsuario == loginUsuario).Select(s => s.CodProfissional);
                if (await usuarioEscritorio.AnyAsync(ct))
                {
                    processo = processo.Where(s => usuarioEscritorio.Contains((int)s.CodProfissional!) || usuarioEscritorio.Contains((int)s.CodEscritorioAcompanhante!) || usuarioEscritorio.Contains((int)s.CodContador!));
                }

                return await processo.AnyAsync(ct);

            }

            return false;

        }

        #endregion

        #region 2500
        public EsF2500 CriarNovoFormularioF2500(DadosIdentificacaoNovoFormularioDTO dadosIdentificacaoFormulario, string loginUsuario, DateTime dataOperacao)
        {

            var formulario2500 = new EsF2500()
            {
                CodParte = dadosIdentificacaoFormulario.CodigoParte,
                CodProcesso = dadosIdentificacaoFormulario.CodigoProcesso,
                LogCodUsuario = loginUsuario,
                LogDataOperacao = dataOperacao,
                StatusFormulario = EsocialStatusFormulario.NaoIniciado.ToByte(),

                IdeeventoIndretif = ESocialIndRetif.Original.ToByte(),
                IdeeventoTpamb = dadosIdentificacaoFormulario.TipoAmbiente,
                IdeeventoProcemi = ESocialProcessoEmissao.AplicativoEmpregador.ToByte(),
                IdeeventoVerproc = dadosIdentificacaoFormulario.VersaoSisjur,

                IdeempregadorTpinsc = ESocialTipoInscricaoTabela05.CNPJ.ToByte(),
                IdeempregadorNrinsc = dadosIdentificacaoFormulario.CnpjEmpregador,

                InfoprocessoOrigem = ESocialOrigemProcesso.ProcessoJudicial.ToByte(),
                InfoprocessoNrproctrab = dadosIdentificacaoFormulario.NroProcessoCartorio,

                InfoprocjudUfvara = dadosIdentificacaoFormulario.UfComarca,
                InfoprocjudCodmunic = dadosIdentificacaoFormulario.CodigoMunicipioComarca,
                InfoprocjudIdvara = dadosIdentificacaoFormulario.IdVara,

                IdetrabCpftrab = dadosIdentificacaoFormulario.CpfParte,
                IdetrabNmtrab = dadosIdentificacaoFormulario.NomeParte,

                InfoprocjudDtsent = dadosIdentificacaoFormulario.DataEventoTransito,

                FinalizadoContador = null,
                FinalizadoEscritorio = null,

                VersaoEsocial = dadosIdentificacaoFormulario.VersaoEsocial
            };

            return formulario2500;
        }

        public void LimparFormularioF2500(DadosIdentificacaoNovoFormularioDTO dadosIdentificacaoFormulario, string loginUsuario, DateTime dataOperacao, ref EsF2500 formulario2500)
        {
            formulario2500.CodParte = dadosIdentificacaoFormulario.CodigoParte;
            formulario2500.CodProcesso = dadosIdentificacaoFormulario.CodigoProcesso;
            formulario2500.LogCodUsuario = loginUsuario;
            formulario2500.LogDataOperacao = dataOperacao;
            formulario2500.StatusFormulario = EsocialStatusFormulario.NaoIniciado.ToByte();
      
            formulario2500.IdeeventoIndretif = ESocialIndRetif.Original.ToByte();
            formulario2500.IdeeventoTpamb = dadosIdentificacaoFormulario.TipoAmbiente;
            formulario2500.IdeeventoProcemi = ESocialProcessoEmissao.AplicativoEmpregador.ToByte();
            formulario2500.IdeeventoVerproc = dadosIdentificacaoFormulario.VersaoSisjur;
           
            formulario2500.IdeempregadorTpinsc = ESocialTipoInscricaoTabela05.CNPJ.ToByte();
            formulario2500.IdeempregadorNrinsc = dadosIdentificacaoFormulario.CnpjEmpregador;
          
            formulario2500.InfoprocessoOrigem = ESocialOrigemProcesso.ProcessoJudicial.ToByte();
            formulario2500.InfoprocessoNrproctrab = dadosIdentificacaoFormulario.NroProcessoCartorio;
        
            formulario2500.InfoprocjudUfvara = dadosIdentificacaoFormulario.UfComarca;
            formulario2500.InfoprocjudCodmunic = dadosIdentificacaoFormulario.CodigoMunicipioComarca;
            formulario2500.InfoprocjudIdvara = dadosIdentificacaoFormulario.IdVara;
        
            formulario2500.IdetrabCpftrab = dadosIdentificacaoFormulario.CpfParte;
            formulario2500.IdetrabNmtrab = dadosIdentificacaoFormulario.NomeParte;
          
            formulario2500.InfoprocjudDtsent = dadosIdentificacaoFormulario.DataEventoTransito;
           
            formulario2500.FinalizadoContador = null;
            formulario2500.FinalizadoEscritorio = null;

            formulario2500.VersaoEsocial = dadosIdentificacaoFormulario.VersaoEsocial;
        }

        public async Task<RetornoRetificacaoDTO> CriaNovoFormulario2500Retificacao(EsF2500 f2500, string? loginUsuario, DateTime dataOperacao, CancellationToken ct)
        {
            string parametroVersaoAtualEsocial = await _parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("VRS_ATUAL_ESOCIAL");

            var novoF2500Retificacao = new EsF2500()
            {
                LogCodUsuario = loginUsuario,
                LogDataOperacao = dataOperacao,
                StatusFormulario = EsocialStatusFormulario.Rascunho.ToByte(),
                IdeeventoIndretif = ESocialIndRetif.Retificacao.ToByte(),
                IdeeventoVerproc = ObtemVersaoSisjur(),
                EvtproctrabId = null,
                IdeeventoNrrecibo = f2500.IdeeventoNrrecibo,
                NomeArquivoEnviado = null,
                NomeArquivoRetornado = null,
                MsgsInconsistenciasEsocial = null,

                CodProcesso = f2500.CodProcesso,
                CodParte = f2500.CodParte,
                FinalizadoEscritorio = f2500.FinalizadoEscritorio,
                FinalizadoContador = f2500.FinalizadoContador,
                IdeeventoTpamb = f2500.IdeeventoTpamb,
                IdeeventoProcemi = f2500.IdeeventoProcemi,
                IdeempregadorTpinsc = f2500.IdeempregadorTpinsc,
                IdeempregadorNrinsc = f2500.IdeempregadorNrinsc,
                IderespTpinsc = f2500.IderespTpinsc,
                IderespNrinsc = f2500.IderespNrinsc,
                InfoprocessoOrigem = f2500.InfoprocessoOrigem,
                InfoprocessoNrproctrab = f2500.InfoprocessoNrproctrab,
                InfoprocessoObsproctrab = f2500.InfoprocessoObsproctrab,
                InfoprocjudDtsent = f2500.InfoprocjudDtsent,
                InfoprocjudUfvara = f2500.InfoprocjudUfvara,
                InfoprocjudCodmunic = f2500.InfoprocjudCodmunic,
                InfoprocjudIdvara = f2500.InfoprocjudIdvara,
                InfoccpDtccp = f2500.InfoccpDtccp,
                InfoccpTpccp = f2500.InfoccpTpccp,
                InfoccpCnpjccp = f2500.InfoccpCnpjccp,
                IdetrabCpftrab = f2500.IdetrabCpftrab,
                IdetrabNmtrab = f2500.IdetrabNmtrab,
                IdetrabDtnascto = f2500.IdetrabDtnascto,
                VersaoEsocial = parametroVersaoAtualEsocial,
            };

            _eSocialDbContext.Add(novoF2500Retificacao);
            await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);

            var f2500DependenteRetificao = await _eSocialDbContext.EsF2500Dependente.AsNoTracking().Where(x => x.IdF2500 == f2500.IdF2500).ToListAsync(ct);
            var f2500InfocontratoRetificacao = await _eSocialDbContext.EsF2500Infocontrato.AsNoTracking().Where(x => x.IdF2500 == f2500.IdF2500).ToListAsync(ct);

            if (f2500DependenteRetificao.Any())
            {
                foreach (var formularioDependente in f2500DependenteRetificao)
                {
                    var novoFormularioDependente = new EsF2500Dependente()
                    {
                        IdF2500 = novoF2500Retificacao.IdF2500,
                        LogCodUsuario = loginUsuario,
                        LogDataOperacao = dataOperacao,

                        DependenteCpfdep = formularioDependente.DependenteCpfdep,
                        DependenteTpdep = formularioDependente.DependenteTpdep,
                        DependenteDescdep = formularioDependente.DependenteDescdep,

                    };

                    _eSocialDbContext.Add(novoFormularioDependente);
                }
                await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);
            };

            if (f2500InfocontratoRetificacao.Any())
            {

                foreach (var formularioInfoContrato in f2500InfocontratoRetificacao)
                {
                    var f2500IdeperiodoRetificacao = await _eSocialDbContext.EsF2500Ideperiodo.AsNoTracking().Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                    var f2500MudcategativRetificacao = await _eSocialDbContext.EsF2500Mudcategativ.AsNoTracking().Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                    var f2500ObservacoesRetificacao = await _eSocialDbContext.EsF2500Observacoes.AsNoTracking().Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                    var f2500RemuneracaoRetificacao = await _eSocialDbContext.EsF2500Remuneracao.AsNoTracking().Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                    var f2500UniccontrRetificacao = await _eSocialDbContext.EsF2500Uniccontr.AsNoTracking().Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                    var f2500AbonoRetificacao = await _eSocialDbContext.EsF2500Abono.AsNoTracking().Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);

                    var novoFormularioInfoContrato = new EsF2500Infocontrato()
                    {
                        IdF2500 = novoF2500Retificacao.IdF2500,
                        LogCodUsuario = loginUsuario,
                        LogDataOperacao = dataOperacao,

                        InfocontrTpcontr = formularioInfoContrato.InfocontrTpcontr,
                        InfocontrIndcontr = formularioInfoContrato.InfocontrIndcontr,
                        InfocontrDtadmorig = formularioInfoContrato.InfocontrDtadmorig,
                        InfocontrIndreint = formularioInfoContrato.InfocontrIndreint,
                        InfocontrIndcateg = formularioInfoContrato.InfocontrIndcateg,
                        InfocontrIndnatativ = formularioInfoContrato.InfocontrIndnatativ,
                        InfocontrIndmotdeslig = formularioInfoContrato.InfocontrIndmotdeslig,
                        InfocontrIndunic = formularioInfoContrato.InfocontrIndunic,
                        InfocontrMatricula = formularioInfoContrato.InfocontrMatricula,
                        InfocontrCodcateg = formularioInfoContrato.InfocontrCodcateg,
                        InfocontrDtinicio = formularioInfoContrato.InfocontrDtinicio,
                        InfocomplCodcbo = formularioInfoContrato.InfocomplCodcbo,
                        InfocomplNatatividade = formularioInfoContrato.InfocomplNatatividade,
                        InfovincTpregtrab = formularioInfoContrato.InfovincTpregtrab,
                        InfovincTpregprev = formularioInfoContrato.InfovincTpregprev,
                        InfovincDtadm = formularioInfoContrato.InfovincDtadm,
                        InfovincTmpparc = formularioInfoContrato.InfovincTmpparc,
                        DuracaoTpcontr = formularioInfoContrato.DuracaoTpcontr,
                        DuracaoDtterm = formularioInfoContrato.DuracaoDtterm,
                        DuracaoClauassec = formularioInfoContrato.DuracaoClauassec,
                        DuracaoObjdet = formularioInfoContrato.DuracaoObjdet,
                        SucessaovincTpinsc = formularioInfoContrato.SucessaovincTpinsc,
                        SucessaovincNrinsc = formularioInfoContrato.SucessaovincNrinsc,
                        SucessaovincMatricant = formularioInfoContrato.SucessaovincMatricant,
                        SucessaovincDttransf = formularioInfoContrato.SucessaovincDttransf,
                        InfodesligDtdeslig = formularioInfoContrato.InfodesligDtdeslig,
                        InfodesligMtvdeslig = formularioInfoContrato.InfodesligMtvdeslig,
                        InfodesligDtprojfimapi = formularioInfoContrato.InfodesligDtprojfimapi,
                        InfovincVralim = formularioInfoContrato.InfovincVralim,
                        InfovincPensalim = formularioInfoContrato.InfovincPensalim,
                        InfovincPercaliment = formularioInfoContrato.InfovincPercaliment,  
                        InfotermDtterm = formularioInfoContrato.InfotermDtterm,
                        InfotermMtvdesligtsv = formularioInfoContrato.InfotermMtvdesligtsv,
                        IdeestabTpinsc = formularioInfoContrato.IdeestabTpinsc,
                        IdeestabNrinsc = formularioInfoContrato.IdeestabNrinsc,
                        InfovlrCompini = formularioInfoContrato.InfovlrCompini,
                        InfovlrCompfim = formularioInfoContrato.InfovlrCompfim,
                        InfovlrRepercproc = formularioInfoContrato.InfovlrRepercproc,
                        InfovlrVrremun = formularioInfoContrato.InfovlrVrremun,
                        InfovlrVrapi = formularioInfoContrato.InfovlrVrapi,
                        InfovlrVr13api = formularioInfoContrato.InfovlrVr13api,
                        InfovlrVrinden = formularioInfoContrato.InfovlrVrinden,
                        InfovlrVrbaseindenfgts = formularioInfoContrato.InfovlrVrbaseindenfgts,
                        InfovlrPagdiretoresc = formularioInfoContrato.InfovlrPagdiretoresc,
                        InfovlrIndreperc = formularioInfoContrato.InfovlrIndreperc,
                        InfovlrIdenabono = formularioInfoContrato.InfovlrIdenabono,
                        InfovlrIdensd = formularioInfoContrato.InfovlrIdensd
                    };

                    _eSocialDbContext.Add(novoFormularioInfoContrato);
                    await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);

                    if (f2500IdeperiodoRetificacao.Any())
                    {
                        foreach (var formularioIdePeriodo in f2500IdeperiodoRetificacao)
                        {
                            var novoFormularioIdePeriodo = new EsF2500Ideperiodo()
                            {
                                IdEsF2500Infocontrato = novoFormularioInfoContrato.IdEsF2500Infocontrato,
                                LogCodUsuario = loginUsuario,
                                LogDataOperacao = dataOperacao,

                                IdeperiodoPerref = formularioIdePeriodo.IdeperiodoPerref,
                                BasecalculoVrbccpmensal = formularioIdePeriodo.BasecalculoVrbccpmensal,
                                BasecalculoVrbccp13 = formularioIdePeriodo.BasecalculoVrbccp13,
                                BasecalculoVrbcfgts = formularioIdePeriodo.BasecalculoVrbcfgts,
                                BasecalculoVrbcfgts13 = formularioIdePeriodo.BasecalculoVrbcfgts13,
                                InfoagnocivoGrauexp = formularioIdePeriodo.InfoagnocivoGrauexp,
                                InfofgtsVrbcfgtsguia = formularioIdePeriodo.InfofgtsVrbcfgtsguia,
                                InfofgtsVrbcfgts13guia = formularioIdePeriodo.InfofgtsVrbcfgts13guia,
                                InfofgtsPagdireto = formularioIdePeriodo.InfofgtsPagdireto,
                                InfofgtsVrbcfgtsdecant = formularioIdePeriodo.InfofgtsVrbcfgtsdecant,
                                InfofgtsVrbcfgtsproctrab = formularioIdePeriodo.InfofgtsVrbcfgtsproctrab,
                                InfofgtsVrbcfgtssefip = formularioIdePeriodo.InfofgtsVrbcfgtssefip,
                                BasemudcategCodcateg = formularioIdePeriodo.BasemudcategCodcateg,
                                BasemudcategVrbccprev = formularioIdePeriodo.BasemudcategVrbccprev,
                            };

                            _eSocialDbContext.Add(novoFormularioIdePeriodo);
                        }
                    }

                    if (f2500MudcategativRetificacao.Any())
                    {
                        foreach (var formularioMudCateg in f2500MudcategativRetificacao)
                        {
                            var novoFormularioMudCateg = new EsF2500Mudcategativ()
                            {
                                IdEsF2500Infocontrato = novoFormularioInfoContrato.IdEsF2500Infocontrato,
                                LogCodUsuario = loginUsuario,
                                LogDataOperacao = dataOperacao,

                                MudcategativCodcateg = formularioMudCateg.MudcategativCodcateg,
                                MudcategativNatatividade = formularioMudCateg.MudcategativNatatividade,
                                MudcategativDtmudcategativ = formularioMudCateg.MudcategativDtmudcategativ,
                            };

                            _eSocialDbContext.Add(novoFormularioMudCateg);
                        }
                    }

                    if (f2500ObservacoesRetificacao.Any())
                    {
                        foreach (var formularioObservacao in f2500ObservacoesRetificacao)
                        {
                            var novoFormularioObservacao = new EsF2500Observacoes()
                            {
                                IdEsF2500Infocontrato = novoFormularioInfoContrato.IdEsF2500Infocontrato,
                                LogCodUsuario = loginUsuario,
                                LogDataOperacao = dataOperacao,

                                ObservacoesObservacao = formularioObservacao.ObservacoesObservacao,
                            };

                            _eSocialDbContext.Add(novoFormularioObservacao);
                        }
                    }

                    if (f2500RemuneracaoRetificacao.Any())
                    {
                        foreach (var formularioRemuneracao in f2500RemuneracaoRetificacao)
                        {
                            var novoFormularioRemuneracao = new EsF2500Remuneracao()
                            {
                                IdEsF2500Infocontrato = novoFormularioInfoContrato.IdEsF2500Infocontrato,
                                LogCodUsuario = loginUsuario,
                                LogDataOperacao = dataOperacao,

                                RemuneracaoDtremun = formularioRemuneracao.RemuneracaoDtremun,
                                RemuneracaoVrsalfx = formularioRemuneracao.RemuneracaoVrsalfx,
                                RemuneracaoUndsalfixo = formularioRemuneracao.RemuneracaoUndsalfixo,
                                RemuneracaoDscsalvar = formularioRemuneracao.RemuneracaoDscsalvar,
                            };

                            _eSocialDbContext.Add(novoFormularioRemuneracao);
                        }
                    }

                    if (f2500UniccontrRetificacao.Any())
                    {
                        foreach (var formularioUnicContr in f2500UniccontrRetificacao)
                        {
                            var novoFormularioUnicContr = new EsF2500Uniccontr()
                            {
                                IdEsF2500Infocontrato = novoFormularioInfoContrato.IdEsF2500Infocontrato,
                                LogCodUsuario = loginUsuario,
                                LogDataOperacao = dataOperacao,

                                UniccontrMatunic = formularioUnicContr.UniccontrMatunic,
                                UniccontrCodcateg = formularioUnicContr.UniccontrCodcateg,
                                UniccontrDtinicio = formularioUnicContr.UniccontrDtinicio,
                            };

                            _eSocialDbContext.Add(novoFormularioUnicContr);
                        }
                    }

                    if (f2500AbonoRetificacao.Any())
                    {
                        foreach (var formularioAbono in f2500AbonoRetificacao)
                        {
                            var novoformularioAbono = new EsF2500Abono()
                            {
                                IdEsF2500Infocontrato = novoFormularioInfoContrato.IdEsF2500Infocontrato,
                                LogCodUsuario = loginUsuario,
                                LogDataOperacao = dataOperacao,

                                AbonoAnobase = formularioAbono.AbonoAnobase                                             
                            };

                            _eSocialDbContext.Add(novoformularioAbono);
                        }
                    }
                }

                await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);
            }

            return new RetornoRetificacaoDTO { IdFormulario = novoF2500Retificacao.IdF2500, VersaoEsocial = novoF2500Retificacao.VersaoEsocial};
        }

        public async Task RemoveFormulario2500(EsParteProcesso? esParteProcesso, CancellationToken ct)
        {
            var idFormulario2500Exclusao = await _eSocialDbContext.EsF2500.Where(x => x.CodParte == esParteProcesso!.CodParte && x.CodProcesso == esParteProcesso!.CodProcesso).Select(x => x.IdF2500).FirstAsync(ct);

            await RemoveFormulario2500(idFormulario2500Exclusao, ct);
        }

        public async Task RemoveFormulario2500(int idF2500, CancellationToken ct)
        {
            var formulario2500Exclusao = await _eSocialDbContext.EsF2500.FirstAsync(x => x.IdF2500 == idF2500, ct);
            if (formulario2500Exclusao is not null)
            {
                var formulario2500DependenteExclusao = await _eSocialDbContext.EsF2500Dependente.Where(x => x.IdF2500 == formulario2500Exclusao.IdF2500).ToListAsync(ct);
                var formulario2500InfocontratoExclusao = await _eSocialDbContext.EsF2500Infocontrato.Where(x => x.IdF2500 == formulario2500Exclusao.IdF2500).ToListAsync(ct);

                if (formulario2500DependenteExclusao.Any())
                {
                    foreach (var formulario in formulario2500DependenteExclusao)
                    {
                        _eSocialDbContext.Remove(formulario);
                    }
                }

                if (formulario2500InfocontratoExclusao.Any())
                {
                    foreach (var formularioInfoContrato in formulario2500InfocontratoExclusao)
                    {
                        var formulario2500IdeperiodoExclusao = await _eSocialDbContext.EsF2500Ideperiodo.Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                        var formulario2500MudcategativExclusao = await _eSocialDbContext.EsF2500Mudcategativ.Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                        var formulario2500ObservacoesExclusao = await _eSocialDbContext.EsF2500Observacoes.Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                        var formulario2500RemuneracaoExclusao = await _eSocialDbContext.EsF2500Remuneracao.Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                        var formulario2500UniccontrExclusao = await _eSocialDbContext.EsF2500Uniccontr.Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                        var formulario2500AbonoExclusao = await _eSocialDbContext.EsF2500Abono.Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);

                        if (formulario2500IdeperiodoExclusao.Any())
                        {
                            foreach (var formularioIdePeriodo in formulario2500IdeperiodoExclusao)
                            {
                                _eSocialDbContext.Remove(formularioIdePeriodo);
                            }
                        }

                        if (formulario2500MudcategativExclusao.Any())
                        {
                            foreach (var formularioMudCateg in formulario2500MudcategativExclusao)
                            {
                                _eSocialDbContext.Remove(formularioMudCateg);
                            }
                        }

                        if (formulario2500ObservacoesExclusao.Any())
                        {
                            foreach (var formularioObservacao in formulario2500ObservacoesExclusao)
                            {
                                _eSocialDbContext.Remove(formularioObservacao);
                            }
                        }

                        if (formulario2500RemuneracaoExclusao.Any())
                        {
                            foreach (var formularioRemuneracao in formulario2500RemuneracaoExclusao)
                            {
                                _eSocialDbContext.Remove(formularioRemuneracao);
                            }
                        }

                        if (formulario2500UniccontrExclusao.Any())
                        {
                            foreach (var formularioUnicContr in formulario2500UniccontrExclusao)
                            {
                                _eSocialDbContext.Remove(formularioUnicContr);
                            }
                        }

                        if (formulario2500AbonoExclusao.Count > 0)
                        {
                            foreach (var formularioAbono in formulario2500AbonoExclusao)
                            {
                                _eSocialDbContext.Remove(formularioAbono);
                            }
                        }

                        _eSocialDbContext.Remove(formularioInfoContrato);
                    }
                }
                _eSocialDbContext.Remove(formulario2500Exclusao);
            }
        }

        public async Task<bool> Formulario2500ProcessadoAsync(int codigoFormulario, byte statusFormulario, CancellationToken ct)
        {
            var listaStatusFormularioNaoPermiteAlteracao = ESocialStatusFormularioEnumFunctions.ListaStatusNaoPermitemAlteracaoStatusFormulario();

            return await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario
            && x.StatusFormulario != statusFormulario
            && listaStatusFormularioNaoPermiteAlteracao.Select(x => x.ToByte()).Contains(x.StatusFormulario), ct);
        }

        public async Task<bool> Formulario2500ExclusaoProcessadoAsync(int codigoFormulario, byte statusFormulario, CancellationToken ct)
        {
            var listaStatusFormularioNaoPermiteAlteracao = ESocialStatusFormularioEnumFunctions.ListaStatusNaoPermiteSolicitarExclusaoS3500();

            return await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario
            && x.StatusFormulario != statusFormulario
            && listaStatusFormularioNaoPermiteAlteracao.Select(x => x.ToByte()).Contains(x.StatusFormulario), ct);
        }

        public async Task<bool> Formulario2500CancelarExclusaoProcessadoAsync(int codigoFormulario, byte statusFormulario, CancellationToken ct)
        {
            var listaStatusFormularioNaoPermiteAlteracao = ESocialStatusFormularioEnumFunctions.ListaStatusNaoPermiteCancelarSolicitacaoExclusaoS3500();

            return await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario
            && x.StatusFormulario != statusFormulario
            && listaStatusFormularioNaoPermiteAlteracao.Select(x => x.ToByte()).Contains(x.StatusFormulario), ct);
        }

        public async Task<EsF2500> RetornaFormulario2500Ativo(int CodigoParte, long CogigoProcesso, CancellationToken ct)
        {
            var listaFormularios2500 = await _eSocialDbContext.EsF2500.AsNoTracking().Include(x => x.LogCodUsuarioNavigation).Where(x => x.CodParte == CodigoParte && x.CodProcesso == CogigoProcesso).ToListAsync(ct);

            if (listaFormularios2500.Count <= 0)
            {
                throw new Exception("A chave Parte x Processo informada não possúi formulários ativos.");
            }

            return listaFormularios2500.MaxBy(x => x.IdF2500)!;

        }

        #endregion

        #region 2501

        public EsF2501 AdicionaFormulario2501(DadosIdentificacaoNovoFormularioDTO dadosIdentificacaoFormulario, string loginUsuario, DateTime dataOperacao, int? idF2501 = 0, int? parentId = 0)
        {
            var formulario2501 = new EsF2501()
            {                
                IdF2501 = idF2501.HasValue ? idF2501.Value : 0,
                ParentIdF2501 = parentId,
                CodParte = dadosIdentificacaoFormulario.CodigoParte,
                CodProcesso = dadosIdentificacaoFormulario.CodigoProcesso,
                LogCodUsuario = loginUsuario,
                LogDataOperacao = dataOperacao,
                StatusFormulario = EsocialStatusFormulario.NaoIniciado.ToByte(),

                IdeeventoIndretif = ESocialIndRetif.Original.ToByte(),
                IdeeventoTpamb = dadosIdentificacaoFormulario.TipoAmbiente,

                IdeempregadorTpinsc = ESocialTipoInscricaoTabela05.CNPJ.ToByte(),
                IdeempregadorNrinsc = dadosIdentificacaoFormulario.CnpjEmpregador,

                IdetrabCpftrab = dadosIdentificacaoFormulario.CpfParte,

                IdeeventoProcemi = ESocialProcessoEmissao.AplicativoEmpregador.ToByte(),
                IdeeventoVerproc = dadosIdentificacaoFormulario.VersaoSisjur,
                IdeprocNrproctrab = dadosIdentificacaoFormulario.NroProcessoCartorio,

                VersaoEsocial = dadosIdentificacaoFormulario.VersaoEsocial
            };

            return formulario2501;
        }

        public void LimpaFormulario2501(DadosIdentificacaoNovoFormularioDTO dadosIdentificacaoFormulario, string loginUsuario, DateTime dataOperacao, ref EsF2501 formulario2501)
        {
            formulario2501.CodParte = dadosIdentificacaoFormulario.CodigoParte;
            formulario2501.CodProcesso = dadosIdentificacaoFormulario.CodigoProcesso;
            formulario2501.LogCodUsuario = loginUsuario;
            formulario2501.LogDataOperacao = dataOperacao;
            formulario2501.StatusFormulario = EsocialStatusFormulario.NaoIniciado.ToByte();

            formulario2501.IdeeventoIndretif = ESocialIndRetif.Original.ToByte();
            formulario2501.IdeeventoTpamb = dadosIdentificacaoFormulario.TipoAmbiente;

            formulario2501.IdeempregadorTpinsc = ESocialTipoInscricaoTabela05.CNPJ.ToByte();
            formulario2501.IdeempregadorNrinsc = dadosIdentificacaoFormulario.CnpjEmpregador;

            formulario2501.IdetrabCpftrab = dadosIdentificacaoFormulario.CpfParte;

            formulario2501.IdeeventoProcemi = ESocialProcessoEmissao.AplicativoEmpregador.ToByte();
            formulario2501.IdeeventoVerproc = dadosIdentificacaoFormulario.VersaoSisjur;
            formulario2501.IdeprocNrproctrab = dadosIdentificacaoFormulario.NroProcessoCartorio;

            formulario2501.FinalizadoContador = null;
            formulario2501.FinalizadoEscritorio = null;

            formulario2501.VersaoEsocial = dadosIdentificacaoFormulario.VersaoEsocial;      
        }

        public async Task<RetornoRetificacaoDTO> CriaNovoFormulario2501Retificacao(EsF2501 f2501, string? loginUsuario, DateTime dataOperacao, CancellationToken ct)
        {
            string parametroVersaoAtualEsocial = await _parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("VRS_ATUAL_ESOCIAL");

            var novoFormulario2501Retificacao = new EsF2501()
            {
                // IdF2501 = f2501.IdF2501,
                CodProcesso = f2501.CodProcesso,
                CodParte = f2501.CodParte,
                LogCodUsuario = loginUsuario,
                LogDataOperacao = dataOperacao,
                ParentIdF2501 = f2501.ParentIdF2501 == 0 ? f2501.IdF2501 : f2501.ParentIdF2501,
                StatusFormulario = EsocialStatusFormulario.Rascunho.ToByte(),
                IdeeventoIndretif = ESocialIndRetif.Retificacao.ToByte(),
                IdeeventoVerproc = ObtemVersaoSisjur(),
                EvtcontprocId = null,
                NomeArquivoEnviado = null,
                NomeArquivoRetornado = null,
                MsgsInconsistenciasEsocial = null,
                IdeeventoNrrecibo = f2501.IdeeventoNrrecibo,

                FinalizadoEscritorio = f2501.FinalizadoEscritorio,
                FinalizadoContador = f2501.FinalizadoContador,
                IdeeventoTpamb = f2501.IdeeventoTpamb,
                IdeeventoProcemi = f2501.IdeeventoProcemi,
                IdeempregadorTpinsc = f2501.IdeempregadorTpinsc,
                IdeempregadorNrinsc = f2501.IdeempregadorNrinsc,
                IdeprocNrproctrab = f2501.IdeprocNrproctrab,
                IdeprocPerapurpgto = f2501.IdeprocPerapurpgto,
                IdeprocObs = f2501.IdeprocObs,
                IdetrabCpftrab = f2501.IdetrabCpftrab,
                InfoircomplemDtlaudo = f2501.InfoircomplemDtlaudo,

                VersaoEsocial = parametroVersaoAtualEsocial,
            };

            _eSocialDbContext.Add(novoFormulario2501Retificacao);
            await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);


            var listaFormulario2501CalcTribRetificacao = await _eSocialDbContext.EsF2501Calctrib.AsNoTracking().Where(x => x.IdEsF2501 == f2501.IdF2501).ToListAsync(ct);
            var listaFormulario2501InfocrirrfRetificacao = await _eSocialDbContext.EsF2501Infocrirrf.AsNoTracking().Where(x => x.IdF2501 == f2501.IdF2501).ToListAsync(ct);
            var listaFormulario2501InfoDepRetificacao = await _eSocialDbContext.EsF2501Infodep.AsNoTracking().Where(x => x.IdEsF2501 == f2501.IdF2501).ToListAsync(ct);

            if (listaFormulario2501CalcTribRetificacao.Any())
            {
                int cont = 0;
                foreach (var formularioCalcTrib in listaFormulario2501CalcTribRetificacao)
                {
                    var novoFormularioCalcTrib = new EsF2501Calctrib()
                    {
                        IdEsF2501 = novoFormulario2501Retificacao.IdF2501,
                        LogCodUsuario = loginUsuario,
                        LogDataOperacao = dataOperacao,

                        CalctribPerref = formularioCalcTrib.CalctribPerref,
                        CalctribVrbccpmensal = formularioCalcTrib.CalctribVrbccpmensal,
                        CalctribVrbccp13 = formularioCalcTrib.CalctribVrbccp13,
                        CalctribVrrendirrf = formularioCalcTrib.CalctribVrrendirrf,
                        CalctribVrrendirrf13 = formularioCalcTrib.CalctribVrrendirrf13,
                    };

                    _eSocialDbContext.Add(novoFormularioCalcTrib);
                    await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);
                    cont += 1;

                    var listaFormulario2501InfocrcontribRetificacao = await _eSocialDbContext.EsF2501Infocrcontrib.AsNoTracking().Where(x => x.IdEsF2501Calctrib == formularioCalcTrib.IdEsF2501Calctrib).ToListAsync(ct);

                    if (listaFormulario2501InfocrcontribRetificacao.Any())
                    {
                        foreach (var formularioInfoCrContrib in listaFormulario2501InfocrcontribRetificacao)
                        {
                            var novoFormularioInfoCrContrib = new EsF2501Infocrcontrib()
                            {
                                IdEsF2501Calctrib = novoFormularioCalcTrib.IdEsF2501Calctrib,
                                LogCodUsuario = loginUsuario,
                                LogDataOperacao = dataOperacao,

                                InfocrcontribTpcr = formularioInfoCrContrib.InfocrcontribTpcr,
                                InfocrcontribVrcr = formularioInfoCrContrib.InfocrcontribVrcr,
                            };

                            _eSocialDbContext.Add(novoFormularioInfoCrContrib);
                            await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);
                        }
                    }
                }
            }

            if (listaFormulario2501InfocrirrfRetificacao.Any())
            {
                foreach (var formularioInfoCrIrrf in listaFormulario2501InfocrirrfRetificacao)
                {
                    var novoFormularioInfoCrIrrf = new EsF2501Infocrirrf()
                    {
                        IdF2501 = novoFormulario2501Retificacao.IdF2501,
                        LogCodUsuario = loginUsuario,
                        LogDataOperacao = dataOperacao,

                        InfocrcontribTpcr = formularioInfoCrIrrf.InfocrcontribTpcr,
                        InfocrcontribVrcr = formularioInfoCrIrrf.InfocrcontribVrcr,
                        InfoirDescisenntrib = formularioInfoCrIrrf.InfoirDescisenntrib,
                        InfoirVrjurosmora = formularioInfoCrIrrf.InfoirVrjurosmora,
                        InfoirVrprevoficial = formularioInfoCrIrrf.InfoirVrprevoficial,
                        InfoirVrrendisen65 = formularioInfoCrIrrf.InfoirVrrendisen65,
                        InfoirVrrendisenntrib = formularioInfoCrIrrf.InfoirVrrendisenntrib,
                        InfoirVrrendmolegrave = formularioInfoCrIrrf.InfoirVrrendmolegrave,
                        InfoirVrrendtrib = formularioInfoCrIrrf.InfoirVrrendtrib,
                        InfoirVrrendtrib13 = formularioInfoCrIrrf.InfoirVrrendtrib13,
                        InforraDescrra = formularioInfoCrIrrf.InforraDescrra,
                        InforraQtdmesesrra = formularioInfoCrIrrf.InforraQtdmesesrra,
                        DespprocjudVlrdespadvogados = formularioInfoCrIrrf.DespprocjudVlrdespadvogados,
                        DespprocjudVlrdespcustas = formularioInfoCrIrrf.DespprocjudVlrdespcustas                        
                    };

                    _eSocialDbContext.Add(novoFormularioInfoCrIrrf);
                    await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);

                    var listaFormulario2501IdeAdvRetificacao = await _eSocialDbContext.EsF2501Ideadv.AsNoTracking().Where(x => x.IdEsF2501Infocrirrf == formularioInfoCrIrrf.IdEsF2501Infocrirrf).ToListAsync(ct);
                    var listaFormulario2501DedDepenRetificacao = await _eSocialDbContext.EsF2501Deddepen.AsNoTracking().Where(x => x.IdEsF2501Infocrirrf == formularioInfoCrIrrf.IdEsF2501Infocrirrf).ToListAsync(ct);
                    var listaFormulario2501PenAlimRetificacao = await _eSocialDbContext.EsF2501Penalim.AsNoTracking().Where(x => x.IdEsF2501Infocrirrf == formularioInfoCrIrrf.IdEsF2501Infocrirrf).ToListAsync(ct);
                    var listaFormulario2501InfoProcRetRetificacao = await _eSocialDbContext.EsF2501Infoprocret.AsNoTracking().Where(x => x.IdEsF2501Infocrirrf == formularioInfoCrIrrf.IdEsF2501Infocrirrf).ToListAsync(ct);

                    if (listaFormulario2501IdeAdvRetificacao.Any())
                    {
                        foreach (var formularioIdeAdv in listaFormulario2501IdeAdvRetificacao)
                        {
                            var novoFormularioIdeAdv = new EsF2501Ideadv()
                            {
                                IdEsF2501Infocrirrf = novoFormularioInfoCrIrrf.IdEsF2501Infocrirrf,
                                LogCodUsuario = loginUsuario,
                                LogDataOperacao = dataOperacao,

                                IdeadvNrinsc = formularioIdeAdv.IdeadvNrinsc,
                                IdeadvTpinsc = formularioIdeAdv.IdeadvTpinsc,
                                IdeadvVlradv = formularioIdeAdv.IdeadvVlradv
                            };

                            _eSocialDbContext.Add(novoFormularioIdeAdv);
                            await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);
                        }
                    }

                    if (listaFormulario2501DedDepenRetificacao.Any())
                    {
                        foreach (var formularioDedDepen in listaFormulario2501DedDepenRetificacao)
                        {
                            var novoFormularioDedDepen = new EsF2501Deddepen()
                            {                                

                                IdEsF2501Infocrirrf = novoFormularioInfoCrIrrf.IdEsF2501Infocrirrf,
                                LogCodUsuario = loginUsuario,
                                LogDataOperacao = dataOperacao,

                                DeddepenCpfdep = formularioDedDepen.DeddepenCpfdep,
                                DeddepenTprend = formularioDedDepen.DeddepenTprend,
                                DeddepenVlrdeducao = formularioDedDepen.DeddepenVlrdeducao
                            };

                            _eSocialDbContext.Add(novoFormularioDedDepen);
                            await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);
                        }
                    }

                    if (listaFormulario2501PenAlimRetificacao.Any())
                    {
                        foreach (var formularioPenAlim in listaFormulario2501PenAlimRetificacao)
                        {
                            var novoFormularioPenAlim = new EsF2501Penalim()
                            {
                                IdEsF2501Infocrirrf = novoFormularioInfoCrIrrf.IdEsF2501Infocrirrf,
                                LogCodUsuario = loginUsuario,
                                LogDataOperacao = dataOperacao,

                                PenalimCpfdep = formularioPenAlim.PenalimCpfdep,
                                PenalimTprend = formularioPenAlim.PenalimTprend,
                                PenalimVlrpensao = formularioPenAlim.PenalimVlrpensao
                            };

                            _eSocialDbContext.Add(novoFormularioPenAlim);
                            await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);
                        }
                    }

                    if (listaFormulario2501InfoProcRetRetificacao.Any())
                    {
                        foreach (var formularioInfoProcRet in listaFormulario2501InfoProcRetRetificacao)
                        {   
                            var novoFormularioInfoProcRet = new EsF2501Infoprocret()
                            {
                                IdEsF2501Infocrirrf = novoFormularioInfoCrIrrf.IdEsF2501Infocrirrf,
                                LogCodUsuario = loginUsuario,
                                LogDataOperacao = dataOperacao,

                                InfoprocretCodsusp = formularioInfoProcRet.InfoprocretCodsusp,
                                InfoprocretNrprocret = formularioInfoProcRet.InfoprocretNrprocret,
                                InfoprocretTpprocret  = formularioInfoProcRet.InfoprocretTpprocret
                            };

                            _eSocialDbContext.Add(novoFormularioInfoProcRet);
                            await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);

                            var listaFormulario2501InfoValoresRetificacao = await _eSocialDbContext.EsF2501Infovalores.AsNoTracking().Where(x => x.IdEsF2501Infoprocret == formularioInfoProcRet.IdEsF2501Infoprocret).ToListAsync(ct);

                            if (listaFormulario2501InfoValoresRetificacao.Any())
                            {
                                foreach (var formularioInfoValores in listaFormulario2501InfoValoresRetificacao)
                                {
                                    var novoFormularioInfoValores = new EsF2501Infovalores()
                                    {
                                        IdEsF2501Infoprocret = novoFormularioInfoProcRet.IdEsF2501Infoprocret,
                                        LogCodUsuario = loginUsuario,
                                        LogDataOperacao = dataOperacao,

                                        InfovaloresIndapuracao = formularioInfoValores.InfovaloresIndapuracao,
                                        InfovaloresVlrcmpanoant = formularioInfoValores.InfovaloresVlrcmpanoant,
                                        InfovaloresVlrcmpanocal = formularioInfoValores.InfovaloresVlrcmpanocal,
                                        InfovaloresVlrdepjud = formularioInfoValores.InfovaloresVlrdepjud,
                                        InfovaloresVlrnretido = formularioInfoValores.InfovaloresVlrnretido,
                                        InfovaloresVlrrendsusp = formularioInfoValores.InfovaloresVlrrendsusp
                                    };

                                    _eSocialDbContext.Add(novoFormularioInfoValores);
                                    await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);

                                    var listaFormulario2501DedSuspRetificacao = await _eSocialDbContext.EsF2501Dedsusp.AsNoTracking().Where(x => x.IdEsF2501Infovalores == formularioInfoValores.IdEsF2501Infovalores).ToListAsync(ct);

                                    if (listaFormulario2501DedSuspRetificacao.Any())
                                    {
                                        foreach (var formularioDedSusp in listaFormulario2501DedSuspRetificacao)
                                        {
                                            var novoFormularioDedSusp = new EsF2501Dedsusp()
                                            {
                                                IdEsF2501Infovalores = novoFormularioInfoValores.IdEsF2501Infovalores,
                                                LogCodUsuario = loginUsuario,
                                                LogDataOperacao = dataOperacao,

                                                DedsuspIndtpdeducao = formularioDedSusp.DedsuspIndtpdeducao,
                                                DedsuspVlrdedsusp = formularioDedSusp.DedsuspVlrdedsusp
                                            };

                                            _eSocialDbContext.Add(novoFormularioDedSusp);
                                            await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);

                                            var listaFormulario2501BenefPenRetRetificacao = await _eSocialDbContext.EsF2501Benefpen.AsNoTracking().Where(x => x.IdEsF2501Dedsusp == formularioDedSusp.IdEsF2501Dedsusp).ToListAsync(ct);

                                            if (listaFormulario2501BenefPenRetRetificacao.Any())
                                            {
                                                foreach (var formularioBenefPen in listaFormulario2501BenefPenRetRetificacao)
                                                {
                                                    var novoFormularioBenefPen = new EsF2501Benefpen()
                                                    {
                                                        IdEsF2501Dedsusp = novoFormularioDedSusp.IdEsF2501Dedsusp,
                                                        LogCodUsuario = loginUsuario,
                                                        LogDataOperacao = dataOperacao,

                                                        BenefpenCpfdep = formularioBenefPen.BenefpenCpfdep,
                                                        BenefpenVlrdepensusp = formularioBenefPen.BenefpenVlrdepensusp
                                                    };

                                                    _eSocialDbContext.Add(novoFormularioBenefPen);
                                                    await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (listaFormulario2501InfoDepRetificacao.Any())
            {
                foreach (var formularioInfoDep in listaFormulario2501InfoDepRetificacao)
                {
                    var novoFormularioInfoDep = new EsF2501Infodep()
                    {
                        IdEsF2501 = novoFormulario2501Retificacao.IdF2501,
                        LogCodUsuario = loginUsuario,
                        LogDataOperacao = dataOperacao,

                        InfodepCpfdep = formularioInfoDep.InfodepCpfdep,
                        InfodepDepirrf = formularioInfoDep.InfodepDepirrf,
                        InfodepDescrdep = formularioInfoDep.InfodepDescrdep,
                        InfodepDtnascto = formularioInfoDep.InfodepDtnascto,
                        InfodepNome = formularioInfoDep.InfodepNome,
                        InfodepTpdep = formularioInfoDep.InfodepTpdep
                    };

                    _eSocialDbContext.Add(novoFormularioInfoDep);
                    await _eSocialDbContext.SaveChangesExternalScopeAsync(loginUsuario, true, ct);
                }
            }

            return new RetornoRetificacaoDTO { IdFormulario = novoFormulario2501Retificacao.IdF2501, VersaoEsocial = novoFormulario2501Retificacao.VersaoEsocial };

        }

        public async Task RemoveFormularios2501(EsParteProcesso esParteProcesso, CancellationToken ct)
        {
            var listaFormularios2501Exclusao = await _eSocialDbContext.EsF2501.Where(x => x.CodParte == esParteProcesso.CodParte && x.CodProcesso == esParteProcesso.CodProcesso).ToListAsync(ct);
            if (listaFormularios2501Exclusao.Any())
            {
                foreach (var formulario2501 in listaFormularios2501Exclusao)
                {
                    await RemoveFormulario2501(formulario2501.IdF2501, ct);
                }
            }
        }

        public async Task RemoveFormulario2501(int idF2501, CancellationToken ct)
        {
            var formulario2501Exclusao = await _eSocialDbContext.EsF2501.FirstAsync(x => x.IdF2501 == idF2501, ct);

            if (formulario2501Exclusao is not null)
            {
                var listaFormulario2501CalcTribExclusao = await _eSocialDbContext.EsF2501Calctrib.Where(x => x.IdEsF2501 == formulario2501Exclusao.IdF2501).ToListAsync(ct);
                var listaFormulario2501InfocrirrfExclusao = await _eSocialDbContext.EsF2501Infocrirrf.Where(x => x.IdF2501 == formulario2501Exclusao.IdF2501).ToListAsync(ct);
                var listaFormulario2501InfodepExclusao = await _eSocialDbContext.EsF2501Infodep.Where(x => x.IdEsF2501 == formulario2501Exclusao.IdF2501).ToListAsync(ct);

                if (listaFormulario2501CalcTribExclusao.Any())
                {
                    foreach (var calcTrib in listaFormulario2501CalcTribExclusao)
                    {
                        var listaFormulario2501InfocrcontribExclusao = await _eSocialDbContext.EsF2501Infocrcontrib.Where(x => x.IdEsF2501Calctrib == calcTrib.IdEsF2501Calctrib).ToListAsync(ct);

                        if (listaFormulario2501InfocrcontribExclusao.Any())
                        {
                            foreach (var infoCrContrib in listaFormulario2501InfocrcontribExclusao)
                            {
                                _eSocialDbContext.Remove(infoCrContrib);
                            }
                        }
                        _eSocialDbContext.Remove(calcTrib);
                    }
                }

                if (listaFormulario2501InfocrirrfExclusao.Any())
                {
                    foreach (var infoCrIrrf in listaFormulario2501InfocrirrfExclusao)
                    {
                        var listaFormulario2501IdeadvExclusao = await _eSocialDbContext.EsF2501Ideadv.Where(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf).ToListAsync(ct);
                        var listaFormulario2501DeddepenExclusao = await _eSocialDbContext.EsF2501Deddepen.Where(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf).ToListAsync(ct);
                        var listaFormulario2501PenAlimExclusao = await _eSocialDbContext.EsF2501Penalim.Where(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf).ToListAsync(ct);
                        var listaFormulario2501ProcRetExclusao = await _eSocialDbContext.EsF2501Infoprocret.Where(x => x.IdEsF2501Infocrirrf == infoCrIrrf.IdEsF2501Infocrirrf).ToListAsync(ct);

                        if (listaFormulario2501IdeadvExclusao.Any())
                        {
                            foreach (var Ideadv in listaFormulario2501IdeadvExclusao)
                            {
                                _eSocialDbContext.Remove(Ideadv);
                            }
                        }

                        if (listaFormulario2501DeddepenExclusao.Any())
                        {
                            foreach (var Deddepen in listaFormulario2501DeddepenExclusao)
                            {
                                _eSocialDbContext.Remove(Deddepen);
                            }
                        }

                        if (listaFormulario2501PenAlimExclusao.Any())
                        {
                            foreach (var PenAlim in listaFormulario2501PenAlimExclusao)
                            {
                                _eSocialDbContext.Remove(PenAlim);
                            }
                        }

                        if (listaFormulario2501ProcRetExclusao.Any())
                        {
                            foreach (var ProcRet in listaFormulario2501ProcRetExclusao)
                            {
                                var listaFormulario2501InfoValoresExclusao = _eSocialDbContext.EsF2501Infovalores.AsNoTracking().Where(x => x.IdEsF2501Infoprocret == ProcRet.IdEsF2501Infoprocret);

                                if (listaFormulario2501InfoValoresExclusao.Any())
                                {
                                    foreach (var InfoValores in listaFormulario2501InfoValoresExclusao)
                                    {
                                        var listaFormulario2501DedSuspExclusao = _eSocialDbContext.EsF2501Dedsusp.AsNoTracking().Where(x => x.IdEsF2501Infovalores == InfoValores.IdEsF2501Infovalores);
                                        if (listaFormulario2501DedSuspExclusao.Any())
                                        {
                                            foreach (var DedSusp in listaFormulario2501DedSuspExclusao)
                                            {
                                                var listaFormulario2501BenefPenExclusao = _eSocialDbContext.EsF2501Benefpen.AsNoTracking().Where(x => x.IdEsF2501Dedsusp == DedSusp.IdEsF2501Dedsusp);

                                                if (listaFormulario2501BenefPenExclusao.Any())
                                                {
                                                    foreach (var BenefPen in listaFormulario2501BenefPenExclusao)
                                                    {
                                                        _eSocialDbContext.Remove(BenefPen);
                                                    }
                                                }
                                                _eSocialDbContext.Remove(DedSusp);
                                            }
                                        }
                                        _eSocialDbContext.Remove(InfoValores);
                                    }
                                }
                                _eSocialDbContext.Remove(ProcRet);
                            }
                        }


                        _eSocialDbContext.Remove(infoCrIrrf);
                    }
                }

                if (listaFormulario2501InfodepExclusao.Any())
                {
                    foreach (var Infodep in listaFormulario2501InfodepExclusao)
                    {
                        _eSocialDbContext.Remove(Infodep);
                    }
                }

                _eSocialDbContext.Remove(formulario2501Exclusao);
            }
        }

        public async Task<bool> Formulario2501ProcessadoAsync(int codigoFormulario, byte statusFormulario, CancellationToken ct)
        {
            var listaStatusFormularioNaoPermiteAlteracao = ESocialStatusFormularioEnumFunctions.ListaStatusNaoPermitemAlteracaoStatusFormulario();

            return await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario
            && x.StatusFormulario != statusFormulario
            && listaStatusFormularioNaoPermiteAlteracao.Select(x => x.ToByte()).Contains(x.StatusFormulario), ct);
        }

        public async Task<bool> Formulario2501ExclusaoProcessadoAsync(int codigoFormulario, byte statusFormulario, CancellationToken ct)
        {
            var listaStatusFormularioNaoPermiteAlteracao = ESocialStatusFormularioEnumFunctions.ListaStatusNaoPermiteSolicitarExclusaoS3500();

            return await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario
            && x.StatusFormulario != statusFormulario
            && listaStatusFormularioNaoPermiteAlteracao.Select(x => x.ToByte()).Contains(x.StatusFormulario), ct);
        }

        public async Task<bool> Formulario2501CancelarExclusaoProcessadoAsync(int codigoFormulario, byte statusFormulario, CancellationToken ct)
        {
            var listaStatusFormularioNaoPermiteAlteracao = ESocialStatusFormularioEnumFunctions.ListaStatusNaoPermiteCancelarSolicitacaoExclusaoS3500();

            return await _eSocialDbContext.EsF2501.AnyAsync(x => x.IdF2501 == codigoFormulario
            && x.StatusFormulario != statusFormulario
            && listaStatusFormularioNaoPermiteAlteracao.Select(x => x.ToByte()).Contains(x.StatusFormulario), ct);
        }

        public async Task<List<EsF2501>> ListaFormularios2501Ativos(int CodigoParte, long CogigoProcesso, CancellationToken ct)
        {
            var listaFormularios2501Completa = await _eSocialDbContext.EsF2501.AsNoTracking().Include(x => x.LogCodUsuarioNavigation).Where(x => x.CodParte == CodigoParte && x.CodProcesso == CogigoProcesso).ToListAsync(ct);
            var listaFormularios2501 = new List<EsF2501>();

            if (listaFormularios2501Completa is not null)
            {
                var listaFormulariosPai = listaFormularios2501Completa.Where(x => x.ParentIdF2501 == 0).OrderBy(x => x.IdF2501).ToList();

                foreach (var formulario in listaFormulariosPai)
                {
                    var ultimoItem2501 = listaFormularios2501Completa.Any(x => x.ParentIdF2501 == formulario.IdF2501) ?
                        listaFormularios2501Completa.Where(x => x.ParentIdF2501 == formulario.IdF2501).MaxBy(x => x.LogDataOperacao)!
                        : listaFormularios2501Completa.Where(x => x.IdF2501 == formulario.IdF2501).FirstOrDefault()!;


                    listaFormularios2501.Add(ultimoItem2501);
                }
            }

            return listaFormularios2501;
        }

        #endregion

        #region Funções privadas

        private string ObtemVersaoSisjur()
        {
            return _eSocialDbContext.VersaoServidorModulo.OrderByDescending(v => v.Data).First().Versao;
        }
        #endregion
    }
}
