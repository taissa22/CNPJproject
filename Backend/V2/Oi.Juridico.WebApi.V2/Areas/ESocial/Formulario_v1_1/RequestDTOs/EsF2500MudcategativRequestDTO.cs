using Oi.Juridico.Contextos.V2.ESocialContext.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs
{
    public class EsF2500MudcategativRequestDTO
    {
        public short? MudcategativCodcateg { get; set; }
        public byte? MudcategativNatatividade { get; set; }
        public DateTime? MudcategativDtmudcategativ { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar(EsF2500Infocontrato contrato)
        {
            var mensagensErro = new List<string>();

            if (this.MudcategativCodcateg.HasValue && this.MudcategativCodcateg.Value < 0)
            {
                mensagensErro.Add("O campo código da categoria é obrigatório.");
            }

            if (this.MudcategativNatatividade.HasValue && this.MudcategativNatatividade.Value <= 0 && this.MudcategativNatatividade.Value > 2)
            {
                mensagensErro.Add("O campo natureza da atividade inválido.");
            }

            if (this.MudcategativCodcateg.HasValue && (
                        this.MudcategativCodcateg.Value == 721 ||
                        this.MudcategativCodcateg.Value == 722 ||
                        this.MudcategativCodcateg.Value == 771 ||
                        this.MudcategativCodcateg.Value == 901 
                        ) &&
                this.MudcategativNatatividade.HasValue)
            {
                mensagensErro.Add("Não informe a Natureza da Atividade, se a Categoria (Bloco H) for igual a 721, 722, 771 e 901.");
            }

            if (this.MudcategativCodcateg.HasValue && (
                        this.MudcategativCodcateg.Value == 104
                        ) &&
                this.MudcategativNatatividade.HasValue  && this.MudcategativNatatividade.Value == 2)
            {
                mensagensErro.Add("Se o 'Código da Categoria' for igual a 104, a Natureza da Atividade não pode ser '2-Trabalho Rural'.");
            }

            if (this.MudcategativCodcateg.HasValue && (
                        this.MudcategativCodcateg.Value == 102
                        ) &&
                this.MudcategativNatatividade.HasValue && this.MudcategativNatatividade.Value == 1)
            {
                mensagensErro.Add("Se a Categoria (Bloco H) for igual a 102,  a Natureza da Atividade não pode ser '1-Trabalho Urbano'.");
            }

            if (this.MudcategativDtmudcategativ.HasValue && (contrato.InfocontrDtinicio.HasValue || contrato.InfovincDtadm.HasValue) &&
                (
                    (
                        (!contrato.InfocontrDtinicio.HasValue) &&
                        (contrato.InfovincDtadm.HasValue && this.MudcategativDtmudcategativ.Value < contrato.InfovincDtadm)
                        ||
                        (!contrato.InfovincDtadm.HasValue) &&
                        (contrato.InfocontrDtinicio.HasValue && this.MudcategativDtmudcategativ.Value < contrato.InfocontrDtinicio)
                    )
                )
                )
            {
                mensagensErro.Add("A Data de Alteração deve ser maior ou igual a Data de Admissão (ou de Início).");
            }

            if (this.MudcategativDtmudcategativ.HasValue && contrato.InfocontrDtinicio.HasValue && contrato.InfovincDtadm.HasValue &&
               (
                    ( this.MudcategativDtmudcategativ.Value < contrato.InfocontrDtinicio) &&
                    ( this.MudcategativDtmudcategativ.Value < contrato.InfovincDtadm)
               )
               )
            {
                mensagensErro.Add("A Data de Alteração deve ser maior ou igual a Data de Admissão (ou de Início).");
            }

            if (this.MudcategativDtmudcategativ.HasValue && contrato.InfodesligDtdeslig.HasValue &&
                (
                    this.MudcategativDtmudcategativ.Value > contrato.InfodesligDtdeslig
                )
                )
            {
                mensagensErro.Add("A Data de Alteração deve ser menor ou igual a Data do Desligamento.");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
