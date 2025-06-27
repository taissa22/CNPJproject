namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2501RequestDTO
    {
        public int CodProcesso { get; set; }
        public int CodParte { get; set; }
        public byte? IdeeventoIndretif { get; set; }
        public byte? IdeempregadorTpinsc { get; set; }
        public string? IdeempregadorNrinsc { get; set; } = string.Empty;
        public string? IdeprocNrproctrab { get; set; } = string.Empty;
        public DateTime? IdeprocPerapurpgto { get; set; }
        public string? IdeprocObs { get; set; } = string.Empty;
        public string? IdetrabCpftrab { get; set; } = string.Empty;
        public DateTime? InfoircomplemDtlaudo { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            if (CodParte <= 0)
            {
                mensagensErro.Add("O Código da Parte deve ser maior que 0");
            }

            if (CodProcesso <= 0)
            {
                mensagensErro.Add("O Código do Processo deve ser maior que 0");
            }

            if (IdeprocNrproctrab == string.Empty || IdeprocNrproctrab is null || Regex.Replace(IdeprocNrproctrab, "[^0-9]+", "").Length > 20)
            {
                mensagensErro.Add("O número do processo é obrigatório e deve contar no máximo 20 caracteres");
            }

            if (IdeprocObs is not null && IdeprocObs.Length > 999)
            {
                mensagensErro.Add("O número do processo é obrigatório e deve contar no máximo 20 caracteres");
            }


            return (mensagensErro.Count > 0, mensagensErro);
        }

        public (bool Invalido, IEnumerable<string> ListaErros) ValidarRascunho()
        {
            var mensagensErro = new List<string>();

            if (CodParte <= 0)
            {
                mensagensErro.Add("O Código da Parte deve ser maior que 0");
            }

            if (CodProcesso <= 0)
            {
                mensagensErro.Add("O Código do Processo deve ser maior que 0");
            }

            if (IdeprocNrproctrab is not null && Regex.Replace(IdeprocNrproctrab, "[^0-9]+", "").Length > 20)
            {
                mensagensErro.Add("O número do processo deve contar no máximo 20 caracteres");
            }

            if (IdeprocNrproctrab is not null && IdeprocNrproctrab.Length > 999)
            {
                mensagensErro.Add("O número do processo é obrigatório e deve contar no máximo 20 caracteres");
            }

            return (mensagensErro.Any(), mensagensErro);
        }
    }
}
