namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs
{
    public class ProcessoDTO
    {
        public long CodProcesso { get; set; }
        public string NroProcessoCartorio { get; set; } = string.Empty;
        public string NomeComarca { get; set; } = string.Empty;
        public string NomeVara { get; set; } = string.Empty;
        public string UfVara { get; set; } = string.Empty;
        public string IndAtivo { get; set; } = string.Empty;
        public string NomeEmpresaGrupo { get; set; } = string.Empty;
        public string IndProprioTerceiro { get; set; } = string.Empty;

    }
}
