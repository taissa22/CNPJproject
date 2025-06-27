namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs
{
    public class EsF2500IdeperiodoDTO
    {
        public long IdEsF2500Infocontrato { get; set; }
        public string IdeperiodoPerref { get; set; } = string.Empty;
        public string LogCodUsuario { get; set; } = string.Empty;
        public DateTime? LogDataOperacao { get; set; }
        public decimal? BasecalculoVrbccpmensal { get; set; }
        public decimal? BasecalculoVrbccp13 { get; set; }
        public byte? InfoagnocivoGrauexp { get; set; }
        public string? InfoagnocivoGrauexpDesc { get; set; }
        public short? BasemudcategCodcateg { get; set; }
        public string? BasemudcategCodcategDesc { get; set; }
        public decimal? BasemudcategVrbccprev { get; set; }
        public long IdEsF2500Ideperiodo { get; internal set; }
        public decimal? InfofgtsVrbcfgtsproctrab { get; set; }
        public decimal? InfofgtsVrbcfgtssefip { get; set; }
        public decimal? InfofgtsVrbcfgtsdecant { get; set; }
    }
}
