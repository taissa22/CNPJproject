namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.DTOs
{
    public class EsF2500HeaderDTO
    {
        //Processo
        public long CodProcesso { get; set; }
        public string NroProcessoCartorio { get; set; } = string.Empty;
        public string NomeComarca { get; set; } = string.Empty;
        public string NomeVara { get; set; } = string.Empty;
        public string UfVara { get; set; } = string.Empty;
        public string IndAtivo { get; set; } = string.Empty;
        public string NomeEmpresaGrupo { get; set; } = string.Empty;
        public string IndProprioTerceiro { get; set; } = string.Empty;
        //Formulário 2500
        public int? CodParte { get; set; }
        public string NomeParte { get; set; } = string.Empty;
        public string CpfParte { get; set; } = string.Empty;
        public byte StatusFormulario { get; set; }
        public string LogCodUsuario { get; set; } = string.Empty;
        public string NomeUsuario { get; set; } = string.Empty;
        public DateTime LogDataOperacao { get; set; }
        public string IdeeventoNrrecibo { get; set; } = string.Empty;
        public string ExclusaoNrrecibo { get; set; } = string.Empty;
    }
}
