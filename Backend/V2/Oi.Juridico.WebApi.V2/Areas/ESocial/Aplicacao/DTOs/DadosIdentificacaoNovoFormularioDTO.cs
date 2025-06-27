namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs
{
    public class DadosIdentificacaoNovoFormularioDTO
    {
        public long CodigoProcesso { get; set; }
        public string NroProcessoCartorio { get; set; } = string.Empty;
        public int CodigoParte { get; set; }
        public string? NomeParte { get; set; }
        public string CpfParte { get; set; } = string.Empty;
        public string UfComarca { get; set; } = string.Empty;
        public short? IdVara { get; set; }
        public int? CodigoMunicipioComarca { get; set; }
        public string VersaoSisjur { get; set; } = string.Empty;
        public DateTime? DataEventoTransito { get; set; }
        public string CnpjEmpregador { get; set; } = string.Empty;
        public byte TipoAmbiente { get; set; }
        public string VersaoEsocial { get; set; } = string.Empty;
    }
}
