using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services
{
    public class ESocialF2500MudancaCategoriaService
    {
        public IEnumerable<string> ValidaInclusaoMudancaCategoria(EsF2500MudcategativRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, contrato).ToList());

            return listaErros;
           
        }

        public IEnumerable<string> ValidaAlteracaoMudancaCategoria(EsF2500MudcategativRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErros = new List<string>();            

            listaErros.AddRange(ValidaErrosGlobais(requestDTO, contrato).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(EsF2500Mudcategativ mudancaCategoria, EsF2500Infocontrato contrato)
        {
            if (mudancaCategoria == null || contrato == null)
            {
                throw new ArgumentNullException("Dados insuficientes para validação da mundança de categoria.");
            }

            var requestDTO = new EsF2500MudcategativRequestDTO()
            {
                MudcategativCodcateg = mudancaCategoria.MudcategativCodcateg,
                MudcategativDtmudcategativ = mudancaCategoria.MudcategativDtmudcategativ,
                MudcategativNatatividade = mudancaCategoria.MudcategativNatatividade
            };

            return ValidaErrosGlobais(requestDTO, contrato);
        }

        private static IEnumerable<string> ValidaErrosGlobais(EsF2500MudcategativRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErrosGlobais = new List<string>();

            if (contrato.InfocontrIndcateg == "N" && contrato.InfocontrIndnatativ == "N")
            {
                listaErrosGlobais.Add("Não deve ser informado o grupo \"Mudança de categoria e/ou natureza da atividade\" (Bloco H) caso os campos \"Categoria Diferente Contrato\" (Bloco D) e \"Natureza Diferente Contrato\" estejam ambos preenchidos com \"Não\".");
            }

            return listaErrosGlobais;
        }
    }
}
