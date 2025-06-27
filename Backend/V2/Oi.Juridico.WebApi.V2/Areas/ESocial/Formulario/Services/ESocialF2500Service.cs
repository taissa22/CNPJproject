using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums.Functions;
using Oi.Juridico.Shared.V2.Enums;
using Perlink.Oi.Juridico.Infra.Constants;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using System.Security.Claims;
using Oi.Juridico.WebApi.V2.Services;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Perlink.Oi.Juridico.Application.Security;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2500Service
    {
        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;
        private readonly ESocialDbContext _eSocialDbContext;
        private readonly ControleDeAcessoService _controleDeAcessoService;

        public ESocialF2500Service(ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext eSocialDbContext, ControleDeAcessoService controleDeAcessoService)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
            _eSocialDbContext = eSocialDbContext;
            _controleDeAcessoService = controleDeAcessoService;
        }

        public async Task<IEnumerable<string>> ValidaAlteracaoF2500(EsF2500 formulario2500, CancellationToken ct)
        {
            var parametroEventoTransitadoJugado = int.Parse(await _parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("COD_EV_TRANSITO_JULGADO"));
            var evento = await _eSocialDbContext.Evento.FirstAsync(x => x.CodEvento == parametroEventoTransitadoJugado, ct);

            var listaErrosGlobal = new List<string>();

            if (!await _eSocialDbContext.AndamentoProcesso.AnyAsync(x => x.CodProcesso == (int)formulario2500!.CodProcesso && x.CodEvento == parametroEventoTransitadoJugado, ct))
            {
                listaErrosGlobal.Add($"Necessário lançamento do evento {evento.DscEvento} para o processo, no módulo trabalhista judicial.");
            }
            if ((formulario2500!.IdetrabNmtrab is null || formulario2500.IdetrabNmtrab.Length < 2) &&
                !await _eSocialDbContext.EsF2500Infocontrato.AnyAsync(x => x.IdF2500 == formulario2500.IdF2500 && x.InfocontrIndcontr == "S", ct))
            {
                listaErrosGlobal.Add("O Nome do Trabalhador (Bloco A) é obrigatório quando no bloco D não existir um contrato com o campo “Possui Inf. Evento Admissão/Início” preenchido como “Sim”.");
            }
            if (formulario2500!.IdetrabDtnascto is null &&
                !await _eSocialDbContext.EsF2500Infocontrato.AnyAsync(x => x.IdF2500 == formulario2500.IdF2500 && x.InfocontrIndcontr == "S", ct))
            {
                listaErrosGlobal.Add("A Data de Nascimento do Trabalhador (Bloco A) é obrigatória quando no bloco D não existir um contrato com o campo “Possui Inf. Evento Admissão/Início” preenchido como “Sim”.");
            }
            if (formulario2500.IdeeventoIndretif == ESocialIndRetif.Retificacao.ToByte() &&
                (await _eSocialDbContext.EsF2500.FirstAsync(x => x.CodParte == formulario2500.CodParte && x.CodProcesso == formulario2500.CodProcesso && x.IdeeventoIndretif == ESocialIndRetif.Original.ToByte(), ct)).InfoprocjudDtsent != formulario2500.InfoprocjudDtsent)
            {
                listaErrosGlobal.Add("A \"Data da Sentença ou Homologação do Acordo do Processo Judicial\" (bloco B) deve ser a mesma informada no formulário 2500 original.");
            }
            if (formulario2500.IdeeventoIndretif == ESocialIndRetif.Retificacao.ToByte() &&
                (await _eSocialDbContext.EsF2500.FirstAsync(x => x.CodParte == formulario2500.CodParte && x.CodProcesso == formulario2500.CodProcesso && x.IdeeventoIndretif == ESocialIndRetif.Original.ToByte(), ct)).InfoccpDtccp != formulario2500.InfoccpDtccp)
            {
                listaErrosGlobal.Add("A \"Data do Acordo celebrado perante à CCP ou NINTER\" (bloco B) deve ser a mesma informada no formulário 2500 original.");
            }

            var listaErrosFilhos = await FilhosInvalido(formulario2500!, ct);

            listaErrosGlobal.AddRange(listaErrosFilhos.ToList());

            return listaErrosGlobal;
        }

        #region Métodos Auxiliares

        public async Task<bool> FormularioProcessadoAsync(int codigoFormulario, byte statusFormulario, CancellationToken ct)
        {
            var listaStatusFormularioNaoPermiteAlteracao = ESocialStatusFormularioEnumFunctions.ListaStatusNaoPermitemAlteracaoStatusFormulario();

            return await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario
            && x.StatusFormulario != statusFormulario
            && listaStatusFormularioNaoPermiteAlteracao.Select(x => x.ToByte()).Contains(x.StatusFormulario), ct);
        }

        public void PreencheEntidadeFormulario2500(EsF2500RequestDTO requestDTO, ref EsF2500? formulario2500, ClaimsPrincipal? user, byte statusFormulario)
        {

            formulario2500!.StatusFormulario = statusFormulario == EsocialStatusFormulario.NaoIniciado.ToByte() ? EsocialStatusFormulario.NaoIniciado.ToByte() : EsocialStatusFormulario.Rascunho.ToByte();
            formulario2500!.LogCodUsuario = user!.Identity!.Name;
            formulario2500!.LogDataOperacao = DateTime.Now;

            if (_controleDeAcessoService.TemPermissao(Permissoes.ESOCIAL_ALTERAR_BLOCOABCDEFHI))
            {

                formulario2500!.IdeempregadorTpinsc = requestDTO.IdeempregadorTpinsc;
                formulario2500!.IdeempregadorNrinsc = requestDTO.IdeempregadorNrinsc is not null ? Regex.Replace(requestDTO.IdeempregadorNrinsc, "[^0-9]+", "") : requestDTO.IdeempregadorNrinsc;
                formulario2500!.IderespTpinsc = requestDTO.IderespTpinsc;
                formulario2500!.IderespNrinsc = requestDTO.IderespNrinsc is not null ? Regex.Replace(requestDTO.IderespNrinsc, "[^0-9]+", "") : requestDTO.IderespNrinsc;
                formulario2500!.InfoprocessoOrigem = requestDTO.InfoprocessoOrigem;
                formulario2500!.InfoprocessoNrproctrab = requestDTO.InfoprocessoNrproctrab is not null ? Regex.Replace(requestDTO.InfoprocessoNrproctrab, "[^0-9]+", "") : requestDTO.InfoprocessoNrproctrab;
                formulario2500!.InfoprocessoObsproctrab = requestDTO.InfoprocessoObsproctrab;
                formulario2500!.InfoprocjudDtsent = requestDTO.InfoprocjudDtsent.HasValue ? requestDTO!.InfoprocjudDtsent.Value.Date : null;
                formulario2500!.InfoprocjudUfvara = requestDTO.InfoprocjudUfvara;
                formulario2500!.InfoprocjudCodmunic = requestDTO.InfoprocjudCodmunic;
                formulario2500!.InfoprocjudIdvara = requestDTO.InfoprocjudIdvara;
                formulario2500!.InfoccpDtccp = requestDTO.InfoccpDtccp.HasValue ? requestDTO!.InfoccpDtccp.Value.Date : null;
                formulario2500!.InfoccpTpccp = requestDTO.InfoccpTpccp;
                formulario2500!.InfoccpCnpjccp = requestDTO.InfoccpCnpjccp is not null ? Regex.Replace(requestDTO.InfoccpCnpjccp, "[^0-9]+", "") : requestDTO.InfoccpCnpjccp;
                formulario2500!.IdetrabCpftrab = requestDTO.IdetrabCpftrab is not null ? Regex.Replace(requestDTO.IdetrabCpftrab, "[^0-9]+", "") : requestDTO.IdetrabCpftrab;
                formulario2500!.IdetrabNmtrab = requestDTO.IdetrabNmtrab is not null ? Regex.Replace(requestDTO.IdetrabNmtrab!.Trim(), @"\s+"," ") : requestDTO.IdetrabNmtrab!;
                formulario2500!.IdetrabDtnascto = requestDTO.IdetrabDtnascto.HasValue ? requestDTO!.IdetrabDtnascto.Value.Date : null;
                formulario2500!.IderespMatrespdir = requestDTO.IderespMatrespdir is not null ? Regex.Replace(requestDTO.IderespMatrespdir!.Trim(), @"\s+", string.Empty): requestDTO.IderespMatrespdir;
                formulario2500!.IderespDtadmrespdir = requestDTO.IderespDtadmrespdir.HasValue ? requestDTO.IderespDtadmrespdir.Value.Date : null;
            }
        }

        public EsF2500DTO PreencheFormulario2500DTO(ref EsF2500? formulario2500)
        {
            var formularioDTO = new EsF2500DTO()
            {
                IdF2500 = formulario2500!.IdF2500,
                CodParte = formulario2500!.CodParte,
                CodProcesso = formulario2500!.CodProcesso,
                StatusFormulario = formulario2500!.StatusFormulario,
                LogCodUsuario = formulario2500!.LogCodUsuario,
                LogDataOperacao = formulario2500!.LogDataOperacao,
                EvtproctrabId = formulario2500!.EvtproctrabId,
                IdeeventoIndretif = formulario2500!.IdeeventoIndretif,
                IdeeventoNrrecibo = formulario2500!.IdeeventoNrrecibo,
                IdeeventoTpamb = formulario2500!.IdeeventoTpamb,
                IdeeventoProcemi = formulario2500!.IdeeventoProcemi,
                IdeeventoVerproc = formulario2500!.IdeeventoVerproc,
                IdeempregadorTpinsc = formulario2500!.IdeempregadorTpinsc,
                IderespTpinsc = formulario2500!.IderespTpinsc,
                InfoprocessoOrigem = formulario2500!.InfoprocessoOrigem,
                InfoprocessoNrproctrab = formulario2500!.InfoprocessoNrproctrab,
                InfoprocessoObsproctrab = formulario2500!.InfoprocessoObsproctrab,
                InfoprocjudDtsent = formulario2500!.InfoprocjudDtsent.HasValue ? formulario2500!.InfoprocjudDtsent.Value.Date : null,
                InfoprocjudUfvara = formulario2500!.InfoprocjudUfvara,
                InfoprocjudCodmunic = formulario2500!.InfoprocjudCodmunic,
                InfoprocjudIdvara = formulario2500!.InfoprocjudIdvara,
                InfoccpDtccp = formulario2500!.InfoccpDtccp.HasValue ? formulario2500!.InfoccpDtccp.Value.Date : null,
                InfoccpTpccp = formulario2500!.InfoccpTpccp,
                InfoccpCnpjccp = formulario2500!.InfoccpCnpjccp,
                IdetrabCpftrab = formulario2500!.IdetrabCpftrab,
                IdetrabNmtrab = formulario2500!.IdetrabNmtrab,
                IdetrabDtnascto = formulario2500!.IdetrabDtnascto.HasValue ? formulario2500!.IdetrabDtnascto.Value.Date : null,
                IdeempregadorNrinsc = formulario2500!.IdeempregadorNrinsc,
                IderespNrinsc = formulario2500!.IderespNrinsc,
                ExclusaoNrrecibo = formulario2500!.ExclusaoNrrecibo,
                FinalizadoContador = formulario2500!.FinalizadoContador,
                FinalizadoEscritorio = formulario2500!.FinalizadoEscritorio,
                OkSemRecibo = formulario2500!.OkSemRecibo,
                IderespMatrespdir = formulario2500!.IderespMatrespdir,
                IderespDtadmrespdir = formulario2500!.IderespDtadmrespdir
            };

            return formularioDTO;
        }

        public async Task RemoveFilhosFormulario2500(int codigoFormulario, CancellationToken ct)
        {
            var formulario2500Exclusao = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
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
            }
        }

        public async Task LimpaFilhosFormulario2500(int codigoFormulario, CancellationToken ct)
        {
            var formulario2500Exclusao = await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
            if (formulario2500Exclusao is not null)
            {
                var formulario2500DependenteExclusao = await _eSocialDbContext.EsF2500Dependente.Where(x => x.IdF2500 == formulario2500Exclusao.IdF2500).ToListAsync(ct);
                var formulario2500InfocontratoExclusao = await _eSocialDbContext.EsF2500Infocontrato.Where(x => x.IdF2500 == formulario2500Exclusao.IdF2500).ToListAsync(ct);

                if (formulario2500DependenteExclusao.Count > 0)
                {
                    foreach (var formulario2500Dependente in formulario2500DependenteExclusao)
                    {
                        _eSocialDbContext.Remove(formulario2500Dependente);
                    }
                }

                if (formulario2500InfocontratoExclusao.Count > 0)
                {
                    foreach (var formularioInfoContrato in formulario2500InfocontratoExclusao)
                    {
                        var formulario2500IdeperiodoExclusao = await _eSocialDbContext.EsF2500Ideperiodo.Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                        var formulario2500MudcategativExclusao = await _eSocialDbContext.EsF2500Mudcategativ.Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                        var formulario2500ObservacoesExclusao = await _eSocialDbContext.EsF2500Observacoes.Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                        var formulario2500RemuneracaoExclusao = await _eSocialDbContext.EsF2500Remuneracao.Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                        var formulario2500UniccontrExclusao = await _eSocialDbContext.EsF2500Uniccontr.Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);
                        var formulario2500AbonoExclusao = await _eSocialDbContext.EsF2500Abono.Where(x => x.IdEsF2500Infocontrato == formularioInfoContrato.IdEsF2500Infocontrato).ToListAsync(ct);

                        if (formulario2500IdeperiodoExclusao.Count > 0)
                        {
                            foreach (var formularioIdePeriodo in formulario2500IdeperiodoExclusao)
                            {
                                var formulario2500InfoIntermExclusao = await _eSocialDbContext.EsF2500Infointerm.Where(x => x.IdEsF2500Ideperiodo == formularioIdePeriodo.IdEsF2500Ideperiodo).ToListAsync(ct);
                                
                                foreach(var formularioInfoInterm in formulario2500InfoIntermExclusao)
                                {
                                    _eSocialDbContext.Remove(formularioInfoInterm);
                                }

                                _eSocialDbContext.Remove(formularioIdePeriodo);
                            }
                        }

                        if (formulario2500MudcategativExclusao.Count > 0)
                        {
                            foreach (var formularioMudCateg in formulario2500MudcategativExclusao)
                            {
                                _eSocialDbContext.Remove(formularioMudCateg);
                            }
                        }

                        if (formulario2500ObservacoesExclusao.Count > 0)
                        {
                            foreach (var formularioObservacao in formulario2500ObservacoesExclusao)
                            {
                                _eSocialDbContext.Remove(formularioObservacao);
                            }
                        }

                        if (formulario2500RemuneracaoExclusao.Count > 0)
                        {
                            foreach (var formularioRemuneracao in formulario2500RemuneracaoExclusao)
                            {
                                _eSocialDbContext.Remove(formularioRemuneracao);
                            }
                        }

                        if (formulario2500UniccontrExclusao.Count > 0)
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
            }
        }

        public async Task<EsF2500HeaderDTO?> ConsultaHeaderF2500(int codigoFormulario, CancellationToken ct)
        {
            return await (from es2500 in _eSocialDbContext.EsF2500.AsNoTracking()
                          from pt in _eSocialDbContext.Parte.Where(x => x.CodParte == es2500.CodParte).Select(x => new { x.CodParte, x.CpfParte, x.NomParte })
                          join p in _eSocialDbContext.Processo on es2500.CodProcesso equals p.CodProcesso
                          join tv in _eSocialDbContext.TipoVara on p.CodTipoVara equals tv.CodTipoVara
                          where es2500.IdF2500 == codigoFormulario
                          select new EsF2500HeaderDTO()
                          {
                              CodProcesso = es2500.CodProcesso,
                              NroProcessoCartorio = p.NroProcessoCartorio,
                              NomeComarca = p.CodComarcaNavigation.NomComarca,
                              NomeVara = $"{p.CodVara}ª VARA {tv.NomTipoVara}",
                              UfVara = p.CodComarcaNavigation.CodEstado,
                              IndAtivo = p.IndProcessoAtivo == "S" ? "ATIVO" : "INATIVO",
                              NomeEmpresaGrupo = p.CodParteEmpresaNavigation.NomParte,
                              IndProprioTerceiro = p.IndProprioTerceiro == "P" ? "PRÓPRIO" : "TERCEIRO",
                              LogCodUsuario = es2500.LogCodUsuario,
                              NomeUsuario = es2500.LogCodUsuarioNavigation.NomeUsuario,
                              LogDataOperacao = es2500.LogDataOperacao,
                              StatusFormulario = es2500.StatusFormulario,
                              CodParte = pt.CodParte,
                              NomeParte = pt.NomParte,
                              CpfParte = pt.CpfParte,
                              IdeeventoNrrecibo = es2500.IdeeventoNrrecibo,
                              ExclusaoNrrecibo = es2500.ExclusaoNrrecibo,
                              OkSemRecibo = es2500.OkSemRecibo,
                          }).FirstOrDefaultAsync(ct);
        }

        public async Task<bool> ExisteFormularioPorIdAsync(int codigoFormulario, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500.AsNoTracking().AnyAsync(x => x.IdF2500 == codigoFormulario, ct);
        }

        public async Task<bool> ExisteFormularioComStatusPorIdAsync(int codigoFormulario, EsocialStatusFormulario status, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario == status.ToByte(), ct);
        }

        public async Task<bool> FormularioPodeSerSalvoPorIdAsync(int codigoFormulario, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario && x.StatusFormulario == EsocialStatusFormulario.Rascunho.ToByte(), ct);
        }

        public async Task<EsF2500?> RetornaFormularioPorIdAsync(int codigoFormulario, CancellationToken ct)
        {
            return await _eSocialDbContext.EsF2500.AsNoTracking().FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);
        }

        public async Task<(bool alteradoComSucesso, string MensagemErro)> AlteraNumeroReciboAsync(int codigoFormulario, string? numeroRecibo, ClaimsPrincipal? user, CancellationToken ct)
        {
            var formulario2500 = await _eSocialDbContext.EsF2500.FirstOrDefaultAsync(x => x.IdF2500 == codigoFormulario, ct);

            if (formulario2500!.StatusFormulario != EsocialStatusFormulario.RetornoOkSemRecibo.ToByte() && (formulario2500.OkSemRecibo == "N" || string.IsNullOrEmpty(formulario2500.OkSemRecibo)))
            {
                return (false, "O Número do recibo não pode ser alterado pois o formulário não se encontra no status correto.");
            }

            if (formulario2500 is not null)
            {
                if (!string.IsNullOrEmpty(numeroRecibo))
                {
                    formulario2500.StatusFormulario = EsocialStatusFormulario.RetornoESocialOk.ToByte();
                    formulario2500.IdeeventoNrrecibo = numeroRecibo;
                    formulario2500.OkSemRecibo = "S";
                    formulario2500.LogCodUsuario = user!.Identity!.Name;
                    formulario2500.LogDataOperacao = DateTime.Now;
                }
                else
                {
                    formulario2500.StatusFormulario = EsocialStatusFormulario.RetornoOkSemRecibo.ToByte();
                    formulario2500.IdeeventoNrrecibo = null;
                    formulario2500.LogCodUsuario = user!.Identity!.Name;
                    formulario2500.LogDataOperacao = DateTime.Now;
                }

            }

            if (formulario2500!.StatusFormulario != EsocialStatusFormulario.RetornoOkSemRecibo.ToByte() && (formulario2500.OkSemRecibo == "N" || string.IsNullOrEmpty(formulario2500.OkSemRecibo)))
            {
                return (false, "O Número do recibo não pode ser alterado pois o formulário não se encontra no status correto.");
            }

            await _eSocialDbContext.SaveChangesAsync(ct);

            return (true, string.Empty);
        }


        #endregion

        private async Task<IEnumerable<string>> FilhosInvalido(EsF2500 formulario2500, CancellationToken ct)
        {
            var mensagensErro = new List<string>();

            if (!await _eSocialDbContext.EsF2500Infocontrato.AnyAsync(x => x.IdF2500 == formulario2500.IdF2500, ct))
            {
                mensagensErro.Add("É necessário que o formulário tenha pelo menos um registro de informações de contrato.");
            }

            var listaContratos = await _eSocialDbContext.EsF2500Infocontrato.Where(x => x.IdF2500 == formulario2500.IdF2500).ToListAsync(ct);
            foreach (var contrato in listaContratos)
            {
                mensagensErro.AddRange((await ESocialF2500InfoContratoService.ValidaErrosGlobais(_eSocialDbContext, contrato, formulario2500!, ct)).ToList());

                var listaPeriodos = await _eSocialDbContext.EsF2500Ideperiodo.Where(x => x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato).ToListAsync(ct);

                if (contrato.InfocontrIndcontr == "N"
                    && contrato.InfocontrCodcateg == 111)
                {
                    foreach (var item in listaPeriodos)
                    {
                        var infoInterm = await _eSocialDbContext.EsF2500Infointerm.AnyAsync(x => x.IdEsF2500Ideperiodo == item.IdEsF2500Ideperiodo, ct);
                        if (!infoInterm)
                        { 
                        var dtPeriodo = new DateTime(int.Parse(item.IdeperiodoPerref.Substring(0, 4)), int.Parse(item.IdeperiodoPerref.Substring(5, 2)), 1);
                        mensagensErro.Add($"Quando o  campo \"Possui Inf. Evento Admissão/Início (Bloco D)\" for igual a \"Não\" e a Categoria informada (Bloco D) for igual a \"111 – EMPREGADO – CONTRATO DE TRABALHO INTERMITENTE\" o grupo \"Informações referentes ao Trabalho Intermitente (Bloco K)\" é de preenchimento obrigatório para o periodo {dtPeriodo.ToString("MM/yyyy")}. Caso não tenha havido trabalho no mês, informe 0 (zero).");
                        }
                    }
                }

                if (contrato.InfocontrIndcontr == "N"
                    && contrato.InfocontrCodcateg == 111
                    && !listaPeriodos.Any())
                {
                     mensagensErro.Add($"Quando o  campo \"Possui Inf. Evento Admissão/Início (Bloco D)\" for igual a \"Não\" e a Categoria informada (Bloco D) for igual a \"111 – EMPREGADO – CONTRATO DE TRABALHO INTERMITENTE\" o grupo \"Informações referentes ao Trabalho Intermitente (Bloco K)\" é de preenchimento obrigatório. Caso não tenha havido trabalho no mês, informe 0 (zero).");
                }

                if (contrato.InfocontrIndcontr != "N"
                   || contrato.InfocontrCodcateg != 111)
                {
                    foreach (var item in listaPeriodos)
                    {
                        var infoInterm = await _eSocialDbContext.EsF2500Infointerm.AnyAsync(x => x.IdEsF2500Ideperiodo == item.IdEsF2500Ideperiodo, ct);
                        if (infoInterm)
                        {
                        var dtPeriodo = new DateTime(int.Parse(item.IdeperiodoPerref.Substring(0, 4)), int.Parse(item.IdeperiodoPerref.Substring(5, 2)), 1);
                        mensagensErro.Add($"Quando o  campo \"Possui Inf. Evento Admissão/Início (Bloco D)\" for diferente de \"Não\" e a Categoria informada (Bloco D) for diferente de \"111 – EMPREGADO – CONTRATO DE TRABALHO INTERMITENTE\" o grupo \"Informações referentes ao Trabalho Intermitente (Bloco K)\" não deve ser preenchido para o periodo {dtPeriodo.ToString("MM/yyyy")}."); 
                        }
                    }
                }

                if (contrato.InfovlrIndreperc == ESocialRepercussaoProcesso_v1_2.DecisaoComPagamento.ToByte() || contrato.InfovlrIndreperc == ESocialRepercussaoProcesso_v1_2.DecisaoTributaria.ToByte())
                {
                    var formulario2500Ideperiodo = await _eSocialDbContext.EsF2500Ideperiodo.AnyAsync(x => x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato, ct);

                    if (!formulario2500Ideperiodo) {
                        mensagensErro.Add("Se o campo Repercussão do Processo (Bloco J)  estiver preenchido com \"1 - Decisão com repercussão tributária e/ou FGTS com rendimentos informados em S-2501\" ou \"5 - Decisão com repercussão tributária e/ou FGTS com pagamento através de depósito judicial\" será necessário informar os campos do Bloco K.");
                    }
                }

            }

            if (!await _eSocialDbContext.EsF2500Infocontrato.AnyAsync(x => x.IdF2500 == formulario2500.IdF2500 && x.InfocontrIndcontr == "N", ct)
               && await _eSocialDbContext.EsF2500Dependente.AnyAsync(x => x.IdF2500 == formulario2500.IdF2500, ct))
            {
                mensagensErro.Add($"Não deve ser informado o grupo \"Dependentes\" (Bloco C) caso o campo \"Possui Inf. Evento Admissão/Início\" (Bloco D) esteja preenchido com \"Sim\" em todos os contratos.");
            }

            var qtdContratosRespInd = _eSocialDbContext.EsF2500Infocontrato.Where(x => x.IdF2500 == formulario2500!.IdF2500).Count();
            if (!string.IsNullOrEmpty(formulario2500!.IderespNrinsc)
                && (!_eSocialDbContext.EsF2500Infocontrato.Any(x => x.IdF2500 == formulario2500!.IdF2500 && x.InfocontrTpcontr == ESocialTipoContratoTSVE.ResponsabilidadeIndireta.ToByte() || x.InfocontrTpcontr == ESocialTipoContratoTSVE.TrabSemVinculoTSVESemReconhecimentoVinculo.ToByte()) || qtdContratosRespInd == 0 || qtdContratosRespInd > 1))
            {
                mensagensErro.Add("Quando o campo Real Empregador (bloco A) é preenchido, é obrigatório o registro de um e somente um contrato (bloco D) e esse deve ter os campos abaixo preenchidos com os seguintes conteúdos: <br/> Tipo de Contrato: 8 - RESPONSABILIDADE INDIRETA ou 6 - TRABALHADOR SEM VÍNCULO DE EMPREGO/ESTATUTÁRIO (TSVE), SEM RECONHECIMENTO DE VÍNCULO EMPREGATÍCIO <br/> Possui Inf. Evento Admissão/Início: Não");
            }

            

            return mensagensErro;
        }
    }
}
