namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs
{
    public class EsF2500UniccontrDTO
    {
        public long IdEsF2500Infocontrato { get; set; }
        public DateTime? LogDataOperacao { get; set; }
        public string LogCodUsuario { get; set; } = string.Empty;
        public string UniccontrMatunic { get; set; } = string.Empty;
        public short? UniccontrCodcateg { get; set; }
        public DateTime? UniccontrDtinicio { get; set; }
        public long IdEsF2500Uniccontr { get; internal set; }
        public string UniccontrDesccateg { get; set; } = string.Empty;
    }
}
