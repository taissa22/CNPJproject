namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2500UniccontrRequestDTO
    {
        public string? UniccontrMatunic { get; set; }
        public short? UniccontrCodcateg { get; set; }
        public DateTime? UniccontrDtinicio { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            if (UniccontrMatunic is not null && UniccontrMatunic.Length > 30)
            {
                mensagensErro.Add("A matrícula incorporada deve conter no máximo 30 caracteres");
            }

            if (UniccontrMatunic is null && (!UniccontrCodcateg.HasValue || UniccontrCodcateg.HasValue && UniccontrCodcateg.Value < 0))
            {
                mensagensErro.Add("O campo código da categoria é obrigatório quando o campo matrícula incorporada não foi informado.");
            }

            if (UniccontrMatunic is not null && (UniccontrCodcateg.HasValue || UniccontrCodcateg.HasValue && UniccontrCodcateg.Value > 0))
            {
                mensagensErro.Add("O Código da Categoria não deve ser informado quando o campo 'Matrícula Incorporada' estiver preenchido.");
            }

            if (UniccontrMatunic is null && !UniccontrDtinicio.HasValue)
            {
                mensagensErro.Add("O campo data de início é obrigatório quando o campo matrícula incorporada não foi informado.");
            }

            if (UniccontrMatunic is not null && UniccontrDtinicio.HasValue)
            {
                mensagensErro.Add("A Data de Início não deve ser informada quando o campo 'Matrícula Incorporada' estiver preenchido.");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
