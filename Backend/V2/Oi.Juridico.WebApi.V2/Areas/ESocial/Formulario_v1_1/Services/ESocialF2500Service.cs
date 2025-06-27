using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums.Functions;
using Oi.Juridico.Shared.V2.Enums;
using Perlink.Oi.Juridico.Infra.Constants;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using System.Security.Claims;
using Oi.Juridico.WebApi.V2.Services;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services
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

            var listaErrosFilhos = await FilhosInvalido(formulario2500!, ct);

            listaErrosGlobal.AddRange(listaErrosFilhos.ToList());

            return listaErrosGlobal;
        }

        #region Métodos Auxiliares

        public async Task<bool> FormularioProcessadoAsync(int codigoFormulario, byte statusFormulario, CancellationToken ct)
        {
            var listaStatusFormularioNaoPermiteAlteracao = ESocialStatusFormularioEnumFunctions.ListaStatusNaoPermitemAlteracaoStatusFormulario();

            return (await _eSocialDbContext.EsF2500.AnyAsync(x => x.IdF2500 == codigoFormulario
            && x.StatusFormulario != statusFormulario
            && listaStatusFormularioNaoPermiteAlteracao.Select(x => x.ToByte()).Contains(x.StatusFormulario), ct));
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
                formulario2500!.IdetrabNmtrab = requestDTO.IdetrabNmtrab;
                formulario2500!.IdetrabDtnascto = requestDTO.IdetrabDtnascto.HasValue ? requestDTO!.IdetrabDtnascto.Value.Date : null;

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
                FinalizadoEscritorio = formulario2500!.FinalizadoEscritorio
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

                if (formulario2500DependenteExclusao.Any())
                {
                    foreach (var formulario2500Dependente in formulario2500DependenteExclusao)
                    {
                        _eSocialDbContext.Remove(formulario2500Dependente);
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
                              ExclusaoNrrecibo = es2500.ExclusaoNrrecibo
                          }).FirstOrDefaultAsync(ct);
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
            }

            if (!await _eSocialDbContext.EsF2500Infocontrato.AnyAsync(x => x.IdF2500 == formulario2500.IdF2500 && x.InfocontrIndcontr == "N", ct)
               && await _eSocialDbContext.EsF2500Dependente.AnyAsync(x => x.IdF2500 == formulario2500.IdF2500, ct))
            {
                mensagensErro.Add($"Não deve ser informado o grupo \"Dependentes\" (Bloco C) caso o campo \"Possui Inf. Evento Admissão/Início\" (Bloco D) esteja preenchido com \"Sim\" em todos os contratos.");
            }

            return mensagensErro;
        }
    }
}
