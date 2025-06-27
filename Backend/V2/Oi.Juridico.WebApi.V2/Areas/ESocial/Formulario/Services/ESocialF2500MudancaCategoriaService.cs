using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2500MudancaCategoriaService
    {
        public IEnumerable<string> ValidaInclusaoMudancaCategoria(ESocialDbContext context, EsF2500MudcategativRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(context, requestDTO, contrato).ToList());
            listaErros.AddRange(ValidaErrosInclusao(context, requestDTO, contrato).ToList());

            return listaErros;

        }

        public IEnumerable<string> ValidaAlteracaoMudancaCategoria(ESocialDbContext context, EsF2500MudcategativRequestDTO requestDTO, EsF2500Infocontrato contrato, int codigoMudCategoria)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(context, requestDTO, contrato).ToList());
            listaErros.AddRange(ValidaErrosAlteracao(context, requestDTO, contrato, codigoMudCategoria).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(ESocialDbContext context, EsF2500Mudcategativ mudancaCategoria, EsF2500Infocontrato contrato)
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

            return ValidaErrosGlobais(context, requestDTO, contrato);
        }

        private static IEnumerable<string> ValidaErrosGlobais(ESocialDbContext context, EsF2500MudcategativRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErrosGlobais = new List<string>();

            if ((contrato.InfocontrIndcateg == "S" || contrato.InfocontrIndnatativ == "S") && (!requestDTO.MudcategativCodcateg.HasValue || !requestDTO.MudcategativDtmudcategativ.HasValue) )
            {
                listaErrosGlobais.Add("Se o valor dos campos \"Categoria Diferente Contrato\" (Bloco D) ou \"Natureza Diferente Contrato\" (Bloco D) for igual a \"Sim\", é obrigatório informar os campos do Grupo \"Mudança de categoria e/ou natureza da atividade\".");
            }

            if (contrato.InfocontrIndcateg == "N" && contrato.InfocontrIndnatativ == "N")
            {
                listaErrosGlobais.Add("Não deve ser informado o grupo \"Mudança de categoria e/ou natureza da atividade\" (Bloco H) caso os campos \"Categoria Diferente Contrato\" (Bloco D) e \"Natureza Diferente Contrato\" estejam ambos preenchidos com \"Não\".");
            }

            if ((contrato.InfocontrIndcontr == "N") && (requestDTO.MudcategativCodcateg == contrato.InfocontrCodcateg))
            {
                listaErrosGlobais.Add("Se o valor do campo \"Possui Inf. Evento Admissão/Início\" (Bloco D) for igual a \"Não\", o código informado no campo \"Categoria\" (Bloco H) não pode ser igual ao informado no campo \"Categoria\" (Bloco D).");
            }

            if ((contrato.InfocontrIndcontr == "N") && (requestDTO.MudcategativNatatividade == contrato.InfocomplNatatividade))
            {
                listaErrosGlobais.Add("Se o valor do campo \"Possui Inf. Evento Admissão/Início\" (Bloco D) for igual a \"Não\", o valor do campo \"Natureza da Atividade\" (Bloco H) não pode ser igual ao informado no campo \"Natureza da Atividade\" (Bloco D).");
            }

            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(ESocialDbContext context, EsF2500MudcategativRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErrosInclusao = new List<string>();

            if (context.EsF2500Mudcategativ.AsNoTracking().Any(x => x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato && x.MudcategativDtmudcategativ!.Value.Date == requestDTO.MudcategativDtmudcategativ!.Value.Date))
            {
                listaErrosInclusao.Add("Data de Mudança de Categoria já existe.");
            }

            return listaErrosInclusao;
        }

        private static IEnumerable<string> ValidaErrosAlteracao(ESocialDbContext context, EsF2500MudcategativRequestDTO requestDTO, EsF2500Infocontrato contrato, int codigoMudCategoria)
        {
            var listaErrosAlteracao = new List<string>();

            if (context.EsF2500Mudcategativ.AsNoTracking().Any(x => x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato && x.MudcategativDtmudcategativ!.Value.Date == requestDTO.MudcategativDtmudcategativ!.Value.Date && x.IdEsF2500Mudcategativ != codigoMudCategoria))
            {
                listaErrosAlteracao.Add("Data de Mudança de Categoria já existe.");
            }

            return listaErrosAlteracao;
        }
    }
}
