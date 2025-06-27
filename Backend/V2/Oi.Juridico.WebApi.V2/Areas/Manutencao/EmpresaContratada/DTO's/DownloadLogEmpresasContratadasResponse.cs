namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.EmpresaContratada.DTO_s
{
    public class DownloadLogEmpresasContratadasResponse
    {
        public int? CodEmpTerceiroContratada { get; set; }
        public string Operacao { get; set; } = string.Empty;
        public DateTime? DatLog { get; set; }
        public string CodUsuario { get; set; } = string.Empty;
        public int? CodEmpresaContratadaA { get; set; }
        public int? CodEmpresaContratadaD { get; set; }
        public int? CodTerceiroContratadoA { get; set; }
        public int? CodTerceiroContratadoD { get; set; }
        public string NomEmpresaContratadaA { get; set; } = string.Empty;
        public string NomEmpresaContratadaD { get; set; } = string.Empty;
        public string LoginTerceiroA { get; set; } = string.Empty;
        public string LoginTerceiroD { get; set; } = string.Empty;
    }
}
