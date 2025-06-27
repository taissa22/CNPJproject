using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services
{
    public class ESocialF2500IdePeriodoService
    {
        public IEnumerable<string> ValidaInclusaoIdePeriodo(EsF2500IdeperiodoRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, contrato).ToList());

            return listaErros;
           
        }

        public IEnumerable<string> ValidaAlteracaoIdePeriodo(EsF2500IdeperiodoRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErros = new List<string>();            

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, contrato).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2500Ideperiodo periodo, EsF2500Infocontrato contrato)
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
                BasecalculoVrbcfgts = periodo.BasecalculoVrbcfgts,
                BasecalculoVrbcfgts13 = periodo.BasecalculoVrbcfgts13,
                InfofgtsVrbcfgtsguia = periodo.InfofgtsVrbcfgtsguia,
                InfofgtsVrbcfgts13guia = periodo.InfofgtsVrbcfgts13guia,
                InfofgtsPagdireto = periodo.InfofgtsPagdireto,
                BasemudcategCodcateg = periodo.BasemudcategCodcateg,
                BasemudcategVrbccprev = periodo.BasemudcategVrbccprev,
                InfoagnocivoGrauexp = periodo.InfoagnocivoGrauexp             
            };

            return ValidaErrosGlobais(requestDTO, contrato, true);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2500IdeperiodoRequestDTO requestDTO, EsF2500Infocontrato contrato, bool validacaoExterna = false)
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
                     

            return listaErrosGlobais;
        }

       
    }
}
