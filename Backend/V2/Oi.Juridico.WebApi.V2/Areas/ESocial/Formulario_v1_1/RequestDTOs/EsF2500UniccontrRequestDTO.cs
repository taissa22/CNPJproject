namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs
{
    public class EsF2500UniccontrRequestDTO
    {
        public string? UniccontrMatunic { get; set; }
        public short? UniccontrCodcateg { get; set; }
        public DateTime? UniccontrDtinicio { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            if (this.UniccontrMatunic is not null && this.UniccontrMatunic.Length > 30)
            {
                mensagensErro.Add("A matrícula incorporada deve conter no máximo 30 caracteres");
            }

            if (this.UniccontrMatunic is null && ((!this.UniccontrCodcateg.HasValue) || (this.UniccontrCodcateg.HasValue && this.UniccontrCodcateg.Value < 0)))
            {
                mensagensErro.Add("O campo código da categoria é obrigatório quando o campo matrícula incorporada não foi informado.");
            }

            if (this.UniccontrMatunic is not null && ((this.UniccontrCodcateg.HasValue) || (this.UniccontrCodcateg.HasValue && this.UniccontrCodcateg.Value > 0)))
            {
                mensagensErro.Add("O Código da Categoria não deve ser informado quando o campo 'Matrícula Incorporada' estiver preenchido.");
            }

            if (this.UniccontrMatunic is null && !this.UniccontrDtinicio.HasValue)
            {
                mensagensErro.Add("O campo data de início é obrigatório quando o campo matrícula incorporada não foi informado.");
            }

            if (this.UniccontrMatunic is not null && this.UniccontrDtinicio.HasValue)
            {
                mensagensErro.Add("A Data de Início não deve ser informada quando o campo 'Matrícula Incorporada' estiver preenchido.");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
