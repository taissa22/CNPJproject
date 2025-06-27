using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services
{
    public class ESocialF2500RemuneracaoService
    {
        public IEnumerable<string> ValidaInclusaoRemuneracao(EsF2500RemuneracaoRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, contrato).ToList());
            listaErros.AddRange(ValidaErrosInclusao(requestDTO, contrato).ToList());

            return listaErros;
           
        }

        public IEnumerable<string> ValidaAlteracaoMudancaCategoria(EsF2500RemuneracaoRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErros = new List<string>();            

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, contrato).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2500Remuneracao remuneracao, EsF2500Infocontrato contrato)
        {
            if (remuneracao == null || contrato == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação da remuneração.");
            }

            var requestDTO = new EsF2500RemuneracaoRequestDTO()
            {
                RemuneracaoDscsalvar = remuneracao.RemuneracaoDscsalvar,
                RemuneracaoDtremun = remuneracao.RemuneracaoDtremun,
                RemuneracaoUndsalfixo = remuneracao.RemuneracaoUndsalfixo,
                RemuneracaoVrsalfx  = remuneracao.RemuneracaoVrsalfx
            };

            return ValidaErrosGlobais(requestDTO, contrato);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2500RemuneracaoRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErrosGlobais = new List<string>();

            DateTime? dtIni = contrato.InfovincDtadm.HasValue ? contrato.InfovincDtadm : null;

            DateTime? dtFim = contrato.InfodesligDtdeslig.HasValue ? contrato.InfodesligDtdeslig : null;

            if (contrato.InfocontrTpcontr.HasValue && contrato.InfocontrTpcontr != ESocialTipoContratoTSVE.TrabSemVinculoTSVESemReconhecimentoVinculo.ToByte() && contrato.InfovincTpregtrab.HasValue && contrato.InfovincTpregtrab == ESocialRegimeTrabalhista.Estatutario.ToByte())
            {
                listaErrosGlobais.Add("Não deve ser informado o grupo \"Remuneração\" (Bloco G), caso o campo \"Tipo Contrato\" (Bloco D) esteja preenchido com um valor diferente de \"6 - Trabalhador sem vínculo de emprego/estatutário (TSVE), sem reconhecimento de vínculo empregatício\" e o campo \"Tipo de Regime Trabalhista\" esteja preenchido com \"Estatutário/legislações específicas\".");
            }

            if (requestDTO.RemuneracaoDtremun.HasValue && dtIni.HasValue && dtFim.HasValue && (requestDTO.RemuneracaoDtremun!.Value.Date < dtIni.Value.Date || requestDTO.RemuneracaoDtremun!.Value.Date > dtFim.Value.Date))
            {
                listaErrosGlobais.Add("A Data de Vigência deve ser maior ou igual a Data de Admissão (ou de Início) e menor ou igual a Data de Desligamento (ou de Término), se informada.");
            }               

            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(EsF2500RemuneracaoRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErrosInclusao = new List<string>();

            if (contrato.InfocontrIndcontr == "S")
            {
                listaErrosInclusao.Add($"Não deve ser informado o grupo \"Remuneração\" (Bloco G) caso o campo \"Possui Inf. Evento Admissão/Início\" (Bloco D) esteja preenchido com \"Sim\".");
            }

            return listaErrosInclusao;
        }
    }
}
