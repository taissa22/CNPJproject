using Oi.Juridico.Contextos.V2.ESocialContext.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2500MudcategativRequestDTO
    {
        public short? MudcategativCodcateg { get; set; }
        public byte? MudcategativNatatividade { get; set; }
        public DateTime? MudcategativDtmudcategativ { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar(EsF2500Infocontrato contrato)
        {
            var mensagensErro = new List<string>();

            if (MudcategativCodcateg.HasValue && MudcategativCodcateg.Value < 0)
            {
                mensagensErro.Add("O campo código da categoria é obrigatório.");
            }

            if (MudcategativNatatividade.HasValue && MudcategativNatatividade.Value <= 0 && MudcategativNatatividade.Value > 2)
            {
                mensagensErro.Add("O campo natureza da atividade inválido.");
            }

            if (MudcategativCodcateg.HasValue && (
                        MudcategativCodcateg.Value == 721 ||
                        MudcategativCodcateg.Value == 722 ||
                        MudcategativCodcateg.Value == 771 ||
                        MudcategativCodcateg.Value == 901
                        ) &&
                MudcategativNatatividade.HasValue)
            {
                mensagensErro.Add("Não informe a Natureza da Atividade, se a Categoria (Bloco H) for igual a 721, 722, 771 e 901.");
            }

            if (MudcategativCodcateg.HasValue &&
                        MudcategativCodcateg.Value == 104
                         &&
                MudcategativNatatividade.HasValue && MudcategativNatatividade.Value == 2)
            {
                mensagensErro.Add("Se o 'Código da Categoria' for igual a 104, a Natureza da Atividade não pode ser '2-Trabalho Rural'.");
            }

            if (MudcategativCodcateg.HasValue &&
                        MudcategativCodcateg.Value == 102
                         &&
                MudcategativNatatividade.HasValue && MudcategativNatatividade.Value == 1)
            {
                mensagensErro.Add("Se a Categoria (Bloco H) for igual a 102,  a Natureza da Atividade não pode ser '1-Trabalho Urbano'.");
            }

            if (contrato.InfocontrIndcontr != "N" && MudcategativDtmudcategativ.HasValue && (contrato.InfocontrDtinicio.HasValue || contrato.InfovincDtadm.HasValue) &&
                (

                        !contrato.InfocontrDtinicio.HasValue &&
                        contrato.InfovincDtadm.HasValue && MudcategativDtmudcategativ.Value < contrato.InfovincDtadm
                        ||
                        !contrato.InfovincDtadm.HasValue &&
                        contrato.InfocontrDtinicio.HasValue && MudcategativDtmudcategativ.Value < contrato.InfocontrDtinicio

                )
                )
            {
                mensagensErro.Add("A Data de Mudança de Categoria deve ser maior ou igual a Data de Admissão (ou de Início).");
            }

            if (contrato.InfocontrIndcontr != "N" && MudcategativDtmudcategativ.HasValue && contrato.InfocontrDtinicio.HasValue && contrato.InfovincDtadm.HasValue &&

                    MudcategativDtmudcategativ.Value < contrato.InfocontrDtinicio &&
                    MudcategativDtmudcategativ.Value < contrato.InfovincDtadm

               )
            {
                mensagensErro.Add("A Data de Mudança de Categoria deve ser maior ou igual a Data de Admissão (ou de Início).");
            }

            if (MudcategativDtmudcategativ.HasValue && contrato.InfodesligDtdeslig.HasValue &&

                    MudcategativDtmudcategativ.Value > contrato.InfodesligDtdeslig

                )
            {
                mensagensErro.Add("A \"Data de Mudança de Categoria\" deve ser menor ou igual a Data do Desligamento.");
            }
            var difDtAdm = contrato.InfovincDtadm.HasValue && MudcategativDtmudcategativ.HasValue ? DateTime.Compare(MudcategativDtmudcategativ!.Value, (DateTime)contrato.InfovincDtadm!) : -1;
            var difDtInicio = contrato.InfocontrDtinicio.HasValue && MudcategativDtmudcategativ.HasValue ? DateTime.Compare(MudcategativDtmudcategativ!.Value, (DateTime)contrato.InfocontrDtinicio!) : -1;

            if (contrato.InfocontrIndcontr == "N" &&

                    MudcategativDtmudcategativ.HasValue && (contrato.InfocontrDtinicio.HasValue || contrato.InfovincDtadm.HasValue) &&
                (
                     !contrato.InfocontrDtinicio.HasValue &&
                        contrato.InfovincDtadm.HasValue && (difDtAdm < 0 || difDtAdm == 0)
                        ||
                        !contrato.InfovincDtadm.HasValue &&
                        contrato.InfocontrDtinicio.HasValue && (difDtInicio < 0 || difDtInicio == 0)
                )
                )
            {
                mensagensErro.Add("A \"Data de Mudança de Categoria\" deve ser maior que a \"Data de Admissão ou Início\" (Bloco E) caso tenha sido informado \"Não\" no campo \"Possui Inf. Evento Admissão/Início\" do (Bloco D).");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
