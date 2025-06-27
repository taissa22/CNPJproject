namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs
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

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            if (this.CodParte <= 0)
            {
                mensagensErro.Add("O Código da Parte deve ser maior que 0");
            }

            if (this.CodProcesso <= 0)
            {
                mensagensErro.Add("O Código do Processo deve ser maior que 0");
            }

            if (this.IdeprocNrproctrab == string.Empty || this.IdeprocNrproctrab is null || Regex.Replace(this.IdeprocNrproctrab, "[^0-9]+", "").Length > 20)
            {
                mensagensErro.Add("O número do processo é obrigatório e deve contar no máximo 20 caracteres");
            }

            if (this.IdeprocObs is not null && this.IdeprocObs.Length > 999)
            {
                mensagensErro.Add("O número do processo é obrigatório e deve contar no máximo 20 caracteres");
            }


            return (mensagensErro.Count > 0, mensagensErro);
        }

        public (bool Invalido, IEnumerable<string> ListaErros) ValidarRascunho()
        {
            var mensagensErro = new List<string>();

            if (this.CodParte <= 0)
            {
                mensagensErro.Add("O Código da Parte deve ser maior que 0");
            }

            if (this.CodProcesso <= 0)
            {
                mensagensErro.Add("O Código do Processo deve ser maior que 0");
            }

            if (this.IdeprocNrproctrab is not null && Regex.Replace(this.IdeprocNrproctrab, "[^0-9]+", "").Length > 20)
            {
                mensagensErro.Add("O número do processo deve contar no máximo 20 caracteres");
            }

            if (this.IdeprocNrproctrab is not null && this.IdeprocNrproctrab.Length > 999)
            {
                mensagensErro.Add("O número do processo é obrigatório e deve contar no máximo 20 caracteres");
            }

            return (mensagensErro.Any(), mensagensErro);
        }
    }
}
