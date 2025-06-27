using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Extensions;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2500RequestDTO
    {
        public long CodProcesso { get; set; }
        public int CodParte { get; set; }
        public byte? IdeempregadorTpinsc { get; set; }
        public string? IdeempregadorNrinsc { get; set; }
        public byte? IderespTpinsc { get; set; }
        public string? IderespNrinsc { get; set; }
        public byte? InfoprocessoOrigem { get; set; }
        public string? InfoprocessoNrproctrab { get; set; }
        public string? InfoprocessoObsproctrab { get; set; }
        public DateTime? InfoprocjudDtsent { get; set; }
        public string? InfoprocjudUfvara { get; set; }
        public int? InfoprocjudCodmunic { get; set; }
        public short? InfoprocjudIdvara { get; set; }
        public DateTime? InfoccpDtccp { get; set; }
        public byte? InfoccpTpccp { get; set; }
        public string? InfoccpCnpjccp { get; set; }
        public string? IdetrabCpftrab { get; set; }
        public string? IdetrabNmtrab { get; set; }
        public DateTime? IdetrabDtnascto { get; set; }
        public DateTime? IderespDtadmrespdir { get; set; }
        public string? IderespMatrespdir { get; set; }


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

            if (InfoprocessoNrproctrab == string.Empty || InfoprocessoNrproctrab is null || Regex.Replace(InfoprocessoNrproctrab, "[^0-9]+", "").Length > 20)
            {
                mensagensErro.Add("O número do processo é obrigatório e deve contar no máximo 20 caracteres");
            }

            if (InfoprocessoObsproctrab is not null && InfoprocessoObsproctrab.Length > 999)
            {
                mensagensErro.Add("A Observação do processo deve contar no máximo 999 caracteres");
            }

            if (!IdeempregadorNrinsc.CNPJValido())
            {
                mensagensErro.Add("CNPJ do Empregador inválido.");
            }

            if (IderespNrinsc is not null && IderespTpinsc is not null && IderespTpinsc == 1 && !IderespNrinsc.CNPJValido())
            {
                mensagensErro.Add("CNPJ do Responsável pelo empregado inválido.");
            }

            if (IderespNrinsc is not null && IderespTpinsc is not null && IderespTpinsc == 2 && !IderespNrinsc.CPFValido())
            {
                mensagensErro.Add("CPF do Responsável pelo empregado inválido.");
            }

            if (!IdetrabCpftrab.CPFValido())
            {
                mensagensErro.Add("CNPJ do Responsável pelo empregado inválido.");
            }

            if (InfoccpCnpjccp is not null && !InfoccpCnpjccp.CNPJValido())
            {
                mensagensErro.Add("CNPJ do Responsável pelo empregado inválido.");
            }

            if (!InfoprocjudIdvara.HasValue)
            {
                mensagensErro.Add("O Campo Id da Vara é obrigatório.");
            }

            if (InfoprocjudIdvara.HasValue && InfoprocjudIdvara.Value > 9999)
            {
                mensagensErro.Add("O Campo Id da Vara não pode conter mais do que 4 caracteres.");
            }


            return (mensagensErro.Any(), mensagensErro);
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

            if (InfoprocessoNrproctrab is not null && Regex.Replace(InfoprocessoNrproctrab, "[^0-9]+", "").Length > 20)
            {
                mensagensErro.Add("O número do processo deve contar no máximo 20 caracteres");
            }

            if (InfoprocessoObsproctrab is not null && InfoprocessoObsproctrab.Length > 999)
            {
                mensagensErro.Add("A Observação do processo deve contar no máximo 999 caracteres");
            }

            if (InfoprocjudIdvara.HasValue && InfoprocjudIdvara.Value > 9999)
            {
                mensagensErro.Add("O Campo Id da Vara não pode conter mais do que 4 caracteres.");
            }


            return (mensagensErro.Any(), mensagensErro);
        }
    }

}
