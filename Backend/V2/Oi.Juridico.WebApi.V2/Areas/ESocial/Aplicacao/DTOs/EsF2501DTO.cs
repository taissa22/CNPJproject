namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs
{
    public class EsF2501DTO
    {
        public int IdF2501 { get; set; }
        public long CodProcesso { get; set; }
        public int CodParte { get; set; }
        public byte StatusFormulario { get; set; }
        public string LogCodUsuario { get; set; } = string.Empty;
        public DateTime LogDataOperacao { get; set; }
        public byte? IdeeventoIndretif { get; set; }
        public byte? IdeempregadorTpinsc { get; set; }
        public string IdeempregadorNrinsc { get; set; } = string.Empty;
        public string IdeprocNrproctrab { get; set; } = string.Empty;
        public string IdeprocPerapurpgto { get; set; } = string.Empty;
        public string IdeprocObs { get; set; } = string.Empty;
        public int? ParentIdF2501 { get; set; }
        public string IdetrabCpftrab { get; set; } = string.Empty;
        public string IdeeventoNrrecibo { get; set; } = string.Empty;
        public string ExclusaoNrrecibo { get; set; } = string.Empty;
        public DateTime? InfoircomplemDtlaudo { get; set; }
        public string? OkSemRecibo { get; set; }
    }
}
