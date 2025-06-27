namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs
{
    public class ESocialDashboardDTO    {
        
        public string DatPeriodoVigente { get; set; } = string.Empty;
        public short IdEsEmpresaAgrupadora { get; set; }
        public int? NumTotGeral { get; set; }
        public int? NumTotPendentes { get; set; }
        public int? NumTotPendNaoAnalisados { get; set; }
        public int? NumTotPendNaoIniciados { get; set; }
        public int? NumTotPendRascunhoEsc { get; set; }
        public int? NumTotPendRascunhoCont { get; set; }
        public int? NumTotPendRascunhoEnv { get; set; }
        public int? NumTotPendRetornoCritica { get; set; }
        public int? NumTotEnviados { get; set; }
        public int? NumTotEnvRetornoOk { get; set; }
        public int? NumTotEnvAguardRetorno { get; set; }
        public int? NumTotEnvSemRecibo { get; set; }
        public int? NumTotRetificados { get; set; }
        public int? NumTotExcluidos { get; set; }
        public int? NumSlaVencidos { get; set; }
        public int? NumSlaVencendo { get; set; }
        public string? TipoPrazoSlaVencendo { get; set; }
        public int? NumSlaNaoAnalisado { get; set; }
        public int? NumSlaNaoIniciados { get; set; }
        public int? NumSlaRascunhoEsc { get; set; }
        public int? NumSlaRascunhoCon { get; set; }
        public int? NumSlaRascunhoEnv { get; set; }
        public int? NumSlaAguardRetorno { get; set; }
        public int? NumSlaRetornoCritica { get; set; }
        public int? NumSlaSemRecibo { get; set; }
        public DateTime? DatUltimaAtualizacao { get; set; }
    }
}
