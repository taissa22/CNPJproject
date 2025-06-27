using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services
{
    public class ESocialF2500UnicContrService
    {
        public IEnumerable<string> ValidaInclusaoUnicidadeContratual(ESocialDbContext context, EsF2500UniccontrRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(context, requestDTO, contrato).ToList());
            listaErros.AddRange(ValidaErrosInclusao(context, requestDTO, contrato).ToList());


            return listaErros;

        }

        public IEnumerable<string> ValidaAlteracaoUnicidadeContratual(ESocialDbContext context, EsF2500UniccontrRequestDTO requestDTO, EsF2500Infocontrato contrato, int codigoUnicidade)
        {
            var listaErros = new List<string>();

            listaErros.AddRange(ValidaErrosGlobais(context, requestDTO, contrato).ToList());
            listaErros.AddRange(ValidaErrosAlteracao(context, requestDTO, contrato, codigoUnicidade).ToList());

            return listaErros;

        }

        public static IEnumerable<string> ValidaErrosGlobais(ESocialDbContext context, EsF2500Uniccontr unicidadeContratual, EsF2500Infocontrato contrato)
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

            return ValidaErrosGlobais(context, requestDTO, contrato);
        }

        private static IEnumerable<string> ValidaErrosGlobais(ESocialDbContext context, EsF2500UniccontrRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErrosGlobais = new List<string>();

            if (contrato.InfocontrCodcateg.HasValue && requestDTO.UniccontrCodcateg.HasValue && contrato.InfocontrCodcateg == requestDTO.UniccontrCodcateg)
            {
                listaErrosGlobais.Add("Se o \"Código Categoria\" (Bloco I) for informado, deve ser diferente do \"Código da Categoria\" (Bloco D).");
            }

            if (!contrato!.InfocontrTpcontr.HasValue || contrato!.InfocontrTpcontr != ESocialTipoContratoTSVE.TrabComUnicidadeContratual.ToByte())
            {
                listaErrosGlobais.Add("Nenhum campo do \"Bloco I\" deve ser preenchido, quando o valor informado no campo \"Tipo de Contrato\"  (Bloco D) for diferente a \"9 - TRABALHADOR CUJOS CONTRATOS FORAM UNIFICADOS (UNICIDADE CONTRATUAL)\" ou se não for informado.");
            }

            return listaErrosGlobais;
        }

        private static IEnumerable<string> ValidaErrosInclusao(ESocialDbContext context, EsF2500UniccontrRequestDTO requestDTO, EsF2500Infocontrato contrato)
        {
            var listaErrosInclusao = new List<string>();

            if (!string.IsNullOrEmpty(requestDTO.UniccontrMatunic))
            {
                if (context.EsF2500Uniccontr.AsNoTracking().Any(x => x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato && x.UniccontrMatunic == requestDTO.UniccontrMatunic))
                {
                    listaErrosInclusao.Add("Matricula Incorporada já informada.");
                }
            }

            if (requestDTO.UniccontrCodcateg.HasValue && requestDTO.UniccontrDtinicio.HasValue)
            {
                if (context.EsF2500Uniccontr.AsNoTracking().Any(x => x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato && x.UniccontrCodcateg == requestDTO.UniccontrCodcateg && x.UniccontrDtinicio!.Value.Date == requestDTO.UniccontrDtinicio.Value.Date))
                {
                    listaErrosInclusao.Add("Código da Categoria e Data de Início já foram informado.");
                }
            }

            return listaErrosInclusao;
        }

        private static IEnumerable<string> ValidaErrosAlteracao(ESocialDbContext context, EsF2500UniccontrRequestDTO requestDTO, EsF2500Infocontrato contrato, int codigoUnicidade)
        {
            var listaErrosAlteracao = new List<string>();

            if (!string.IsNullOrEmpty(requestDTO.UniccontrMatunic))
            {
                if (context.EsF2500Uniccontr.AsNoTracking().Any(x => x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato && x.UniccontrMatunic == requestDTO.UniccontrMatunic && x.IdEsF2500Uniccontr != codigoUnicidade))
                {
                    listaErrosAlteracao.Add("Matricula Incorporada já informada.");
                }
            }

            if (requestDTO.UniccontrCodcateg.HasValue && requestDTO.UniccontrDtinicio.HasValue)
            {
                if (context.EsF2500Uniccontr.AsNoTracking().Any(x => x.IdEsF2500Infocontrato == contrato.IdEsF2500Infocontrato && x.UniccontrCodcateg == requestDTO.UniccontrCodcateg && x.UniccontrDtinicio!.Value.Date == requestDTO.UniccontrDtinicio.Value.Date && x.IdEsF2500Uniccontr != codigoUnicidade))
                {
                    listaErrosAlteracao.Add("Código da Categoria e Data de Início já foram informado.");
                }
            }

            return listaErrosAlteracao;
        }
    }
}
