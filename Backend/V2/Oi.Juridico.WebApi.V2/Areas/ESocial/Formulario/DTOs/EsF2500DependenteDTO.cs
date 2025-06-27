namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs
{
    public class EsF2500DependenteDTO
    {
        public long IdEsF2500Dependente { get; set; }
        public int IdF2500 { get; set; }
        public string DependenteCpfdep { get; set; } = string.Empty;
        public string DependenteTpdep { get; set; } = string.Empty;
        public string DependenteDescdep { get; set; } = string.Empty;
        public string LogCodUsuario { get; set; } = string.Empty;
        public DateTime? LogDataOperacao { get; set; }
        public string DescricaoTipoDependente { get; set; }

    }
}
