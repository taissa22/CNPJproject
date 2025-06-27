using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;
using Perlink.Oi.Juridico.Application.Security;
using System.Security.Claims;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2500IdePeriodoService
    {

        private readonly ParametroJuridicoContext _parametroJuridicoDbContext;

        public ESocialF2500IdePeriodoService(ParametroJuridicoContext parametroJuridicoDbContext)
        {
            _parametroJuridicoDbContext = parametroJuridicoDbContext;
        }

        public IEnumerable<string> ValidaInclusaoIdePeriodo(EsF2500IdeperiodoRequestDTO requestDTO, EsF2500Infocontrato contrato, ESocialDbContext _eSocialDbContext)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, contrato, _parametroJuridicoDbContext, _eSocialDbContext, false).ToList());
            listaErros.AddRange(ValidaErrosInclusao(requestDTO, contrato, _eSocialDbContext, false).ToList());

            return listaErros;

        }

        public IEnumerable<string> ValidaAlteracaoIdePeriodo(EsF2500IdeperiodoRequestDTO requestDTO, EsF2500Infocontrato contrato, ESocialDbContext _eSocialDbContext, long codigoIdePeriodo)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, contrato, _parametroJuridicoDbContext, _eSocialDbContext, false).ToList());
            listaErros.AddRange(ValidaErrosAlteracao(requestDTO, contrato, _eSocialDbContext, codigoIdePeriodo, false).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2500Ideperiodo periodo, EsF2500Infocontrato contrato, ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext _eSocialDbContext)
        {
            if (periodo == null || periodo == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação períodos de base de cálculo.");
            }

            var requestDTO = new EsF2500IdeperiodoRequestDTO()
            {
                IdeperiodoPerref = new DateTime(int.Parse(periodo.IdeperiodoPerref.Substring(0, 4)), int.Parse(periodo.IdeperiodoPerref.Substring(5, 2)), 1),
                BasecalculoVrbccpmensal = periodo.BasecalculoVrbccpmensal,
                BasecalculoVrbccp13 = periodo.BasecalculoVrbccp13,
                BasemudcategCodcateg = periodo.BasemudcategCodcateg,
                BasemudcategVrbccprev = periodo.BasemudcategVrbccprev,
                InfoagnocivoGrauexp = periodo.InfoagnocivoGrauexp
            };

            return ValidaErrosGlobais(requestDTO, contrato, parametroJuridicoDbContext, _eSocialDbContext, true);
        }

        public async Task LimpaPeriodo(int IdF2500, ESocialDbContext _eSocialDbContext, ClaimsPrincipal? user, CancellationToken ct)
        {
            var listaContratos = await _eSocialDbContext.EsF2500Infocontrato.Where(x => x.IdF2500 == IdF2500).ToListAsync(ct);

            foreach (var contrato in listaContratos)
            {
                if (contrato.InfovlrIndreperc != 1 && contrato.InfovlrIndreperc != 5)
                {
                    var formulario2500IdeperiodoExclusao = await _eSocialDbContext.EsF2500Ideperiodo.Where(x => x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato).ToListAsync(ct);

                    foreach (var formularioIdePeriodo in formulario2500IdeperiodoExclusao)
                    {
                        var infoInterm = await _eSocialDbContext.EsF2500Infointerm.Where(x => x.IdEsF2500Ideperiodo == formularioIdePeriodo.IdEsF2500Ideperiodo).ToListAsync(ct);

                        if (infoInterm is not null)
                        {
                            foreach (var item in infoInterm)
                            {
                                item.LogCodUsuario = user!.Identity!.Name;
                                item.LogDataOperacao = DateTime.Now;
                                _eSocialDbContext.Remove(item);
                            }
                        }

                        formularioIdePeriodo.LogCodUsuario = user!.Identity!.Name;
                        formularioIdePeriodo.LogDataOperacao = DateTime.Now;
                        _eSocialDbContext.Remove(formularioIdePeriodo);

                    }
                }
                
            }

        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2500IdeperiodoRequestDTO requestDTO, EsF2500Infocontrato contrato, ParametroJuridicoContext parametroJuridicoDbContext, ESocialDbContext _eSocialDbContext, bool validacaoExterna = false)
        {
            var listaErrosGlobais = new List<string>();

            var sufixo = validacaoExterna ? $"Período de referência {requestDTO.IdeperiodoPerref.ToString("MM/yyyy")} (Bloco K) - " : string.Empty;

            DateTime? dtIni = contrato.InfovlrCompini is not null && contrato.InfovlrCompini != string.Empty ?
                 new DateTime(int.Parse(contrato.InfovlrCompini.Substring(0, 4)), int.Parse(contrato.InfovlrCompini.Substring(4, 2)), 1)
                 : null;

            DateTime? dtFim = contrato.InfovlrCompfim is not null && contrato.InfovlrCompfim != string.Empty ?
                new DateTime(int.Parse(contrato.InfovlrCompfim.Substring(0, 4)), int.Parse(contrato.InfovlrCompfim.Substring(4, 2)), 1)
                : null;

            if (dtIni.HasValue && dtFim.HasValue && (requestDTO.IdeperiodoPerref.Date < dtIni || requestDTO.IdeperiodoPerref.Date > dtFim))
            {
                listaErrosGlobais.Add($"{sufixo}O campo \"Período de Referência\" (Bloco K) informado deve estar compreendido compreendido entre os períodos da \"Competência Inicial\" e \"Competência Final\" (Bloco J).");
            }

            if (contrato.InfocontrIndcateg != null && contrato.InfocontrIndcateg == "S" && !requestDTO.BasemudcategCodcateg.HasValue)
            {
                listaErrosGlobais.Add($"{sufixo}O campo \"Categoria\" (Bloco K) deve ser preenchido se o campo \"Indicativo de Reconhecimento de Categoria do Trabalhador Diferente da Cadastrada\" (Bloco D) estiver preenchido com \"Sim\".");
            }

            if (contrato.InfocontrIndcateg != null && contrato.InfocontrIndcateg == "S" && !requestDTO.BasemudcategVrbccprev.HasValue)
            {
                listaErrosGlobais.Add($"{sufixo}O campo \"Valor Base Cálc INSS Mud Cat.\" (Bloco K) deve ser preenchido se o campo \"Indicativo de Reconhecimento de Categoria do Trabalhador Diferente da Cadastrada\" (Bloco D) estiver preenchido com \"Sim\"");
            }

            if (!requestDTO.InfoagnocivoGrauexp.HasValue && contrato.InfocontrCodcateg.HasValue && (Enumerable.Range(100, 399).Contains(contrato.InfocontrCodcateg.Value)
                           || contrato.InfocontrCodcateg.Value == 731
                           || contrato.InfocontrCodcateg.Value == 734
                           || contrato.InfocontrCodcateg.Value == 738))
            {
                listaErrosGlobais.Add($"{sufixo}O campo \"Grau de Exposição\" (Bloco K) é de preenchimento obrigatório, quando o campo \"Categoria\" (Bloco D) for igual a [1XX, 2XX, 3XX, 731, 734, 738].");
            }

            if (requestDTO.InfoagnocivoGrauexp.HasValue && contrato.InfocontrCodcateg.HasValue && (!Enumerable.Range(100, 399).Contains(contrato.InfocontrCodcateg.Value)
                           && contrato.InfocontrCodcateg.Value != 731
                           && contrato.InfocontrCodcateg.Value != 734
                           && contrato.InfocontrCodcateg.Value != 738))
            {
                listaErrosGlobais.Add($"{sufixo}O campo \"Grau de Exposição\" (Bloco K) não deve ser preenchido, quando o campo \"Categoria\" (Bloco D) for diferente de [1XX, 2XX, 3XX, 731, 734, 738].");
            }
            var parametro = parametroJuridicoDbContext.RecuperaConteudoParametroJuridicoPorId("ES_INICIO_FGTS_DIGITAL").Result;
            var dataParametro = parametro.Split("/");
            var dataInicioFGTS = new DateTime(int.Parse(dataParametro[1]), int.Parse(dataParametro[0]), 1);
            if (requestDTO.IdeperiodoPerref > dataInicioFGTS && requestDTO.InfofgtsVrbcfgtsdecant > 0)
            {
                listaErrosGlobais.Add($"{sufixo}O campo \"Base Cálculo declarada no eSocial e ainda não recolhida\" só deve ser informado quando o período de referência for anterior a {parametro}");

            }           

            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(EsF2500IdeperiodoRequestDTO requestDTO, EsF2500Infocontrato contrato, ESocialDbContext _eSocialDbContext, bool validacaoExterna = false)
        {
            var listaErrosInclusao = new List<string>();

            var sufixo = validacaoExterna ? $"Período de referência {requestDTO.IdeperiodoPerref.ToString("MM/yyyy")} (Bloco K) - " : string.Empty;

            if (_eSocialDbContext.EsF2500Ideperiodo.Any(x => x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato && x.IdeperiodoPerref == requestDTO.IdeperiodoPerref.ToString("yyyy-MM")))
            {
                listaErrosInclusao.Add($"{sufixo}Período de Referência já cadastrado para esse mês/ano.");
            }

            if (!requestDTO.InfofgtsVrbcfgtsproctrab.HasValue && (requestDTO.InfofgtsVrbcfgtssefip.HasValue || requestDTO.InfofgtsVrbcfgtsdecant.HasValue))
            {
                listaErrosInclusao.Add($"{sufixo}Caso um dos campos referentes \"Informações referentes a bases de cálculo de FGTS\" esteja preenchido, o campo \"Base Cálculo ainda não declarada em SEFIP ou no eSocial\" deve ser preenchido.");
            }

            return listaErrosInclusao;
        }

        private static IEnumerable<string> ValidaErrosAlteracao(EsF2500IdeperiodoRequestDTO requestDTO, EsF2500Infocontrato contrato, ESocialDbContext _eSocialDbContext, long codigoIdePeriodo, bool validacaoExterna = false)
        {
            var listaErrosAlteracao = new List<string>();

            var sufixo = validacaoExterna ? $"Período de referência {requestDTO.IdeperiodoPerref.ToString("MM/yyyy")} (Bloco K) - " : string.Empty;

            if (_eSocialDbContext.EsF2500Ideperiodo.Any(x => x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato && x.IdEsF2500Ideperiodo != codigoIdePeriodo && x.IdeperiodoPerref == requestDTO.IdeperiodoPerref.ToString("yyyy-MM")))
            {
                listaErrosAlteracao.Add($"{sufixo}Período de Referência já cadastrado para esse mês/ano.");
            }

            return listaErrosAlteracao;
        }

    }
}
