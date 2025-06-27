namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.EmpresaContratada.DTO_s
{
    public class EmpresaContratadaRequest
    {
        public int CodEmpresaContratada { get; set; }
        public string NomEmpresaContratada { get; set; } = string.Empty;
        public List<TerceiroContratadoResponse> Matriculas { get; set; } = new List<TerceiroContratadoResponse>();
    }
}
