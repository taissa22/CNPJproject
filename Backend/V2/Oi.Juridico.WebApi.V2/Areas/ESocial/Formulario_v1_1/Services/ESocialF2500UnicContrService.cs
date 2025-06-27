using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services
{
    public class ESocialF2500UnicContrService
    {
        public IEnumerable<string> ValidaInclusaoUnicidadeContratual(EsF2500UniccontrRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, contrato).ToList());

            return listaErros;
           
        }

        public IEnumerable<string> ValidaAlteracaoUnicidadeContratual(EsF2500UniccontrRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, contrato).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2500Uniccontr unicidadeContratual, EsF2500Infocontrato contrato)
        {
            if (unicidadeContratual == null || contrato == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação de unicidade contratual.");
            }

            var requestDTO = new EsF2500UniccontrRequestDTO()
            {
                UniccontrCodcateg = unicidadeContratual.UniccontrCodcateg,
                UniccontrDtinicio = unicidadeContratual.UniccontrDtinicio,
                UniccontrMatunic = unicidadeContratual.UniccontrMatunic
            };

            return ValidaErrosGlobais(requestDTO, contrato);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2500UniccontrRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErrosGlobais = new List<string>();

            if (contrato.InfocontrCodcateg.HasValue && requestDTO.UniccontrCodcateg.HasValue && contrato.InfocontrCodcateg == requestDTO.UniccontrCodcateg)
            {
                listaErrosGlobais.Add("Se o \"Código Categoria\" (Bloco I) for informado, deve ser diferente do \"Código da Categoria\" (Bloco D).");
            }

            if (string.IsNullOrEmpty(contrato!.InfocontrIndunic) || contrato!.InfocontrIndunic == "N")
            {
                listaErrosGlobais.Add("Nenhum campo do \"Bloco I\" deve ser preenchido, quando o valor informado no campo \"Unicidade contratual\"  (Bloco D) for igual a \"Não\" ou se não for informado.");
            }

            return listaErrosGlobais;
        }
    }
}
