namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs
{
    public class EsF2500DTO
    {
        public int IdF2500 { get; set; }
        public long CodProcesso { get; set; }
        public int CodParte { get; set; }
        public byte StatusFormulario { get; set; }
        public string LogCodUsuario { get; set; } = string.Empty;
        public DateTime LogDataOperacao { get; set; }
        public string EvtproctrabId { get; set; } = string.Empty;
        public byte? IdeeventoIndretif { get; set; }
        public string IdeeventoNrrecibo { get; set; } = string.Empty;
        public byte? IdeeventoTpamb { get; set; }
        public byte? IdeeventoProcemi { get; set; }
        public string IdeeventoVerproc { get; set; } = string.Empty;
        public byte? IdeempregadorTpinsc { get; set; }
        public string IdeempregadorNrinsc { get; set; } = string.Empty;
        public byte? IderespTpinsc { get; set; }
        public string IderespNrinsc { get; set; } = string.Empty;
        public byte? InfoprocessoOrigem { get; set; }
        public string InfoprocessoNrproctrab { get; set; } = string.Empty;
        public string InfoprocessoObsproctrab { get; set; } = string.Empty;
        public DateTime? InfoprocjudDtsent { get; set; }
        public string InfoprocjudUfvara { get; set; } = string.Empty;
        public int? InfoprocjudCodmunic { get; set; }
        public short? InfoprocjudIdvara { get; set; }
        public DateTime? InfoccpDtccp { get; set; }
        public byte? InfoccpTpccp { get; set; }
        public string InfoccpCnpjccp { get; set; } = string.Empty;
        public string IdetrabCpftrab { get; set; } = string.Empty;
        public string IdetrabNmtrab { get; set; } = string.Empty;
        public DateTime? IdetrabDtnascto { get; set; }
        public string? FinalizadoEscritorio { get; internal set; }
        public string? FinalizadoContador { get; internal set; }
        public string ExclusaoNrrecibo { get; set; } = string.Empty;
        public string? OkSemRecibo { get; set; }
        public DateTime? IderespDtadmrespdir { get; set; }
        public string? IderespMatrespdir { get; set; }


        public (bool invalido, IEnumerable<string> listaErros) Validar()
        {
            var mensagensErro = new List<string>();
            if (IdF2500 <= 0)
            {
                mensagensErro.Add("O ID do formulário deve ser maior que 0");
            }

            if (CodParte <= 0)
            {
                mensagensErro.Add("O Código da Parte deve ser maior que 0");
            }

            if (CodProcesso <= 0)
            {
                mensagensErro.Add("O Código do Processo deve ser maior que 0");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }

}
