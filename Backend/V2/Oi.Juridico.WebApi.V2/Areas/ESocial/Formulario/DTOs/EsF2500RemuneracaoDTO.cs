namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs
{
    public class EsF2500RemuneracaoDTO
    {
        public long IdEsF2500Infocontrato { get; set; }
        public DateTime? RemuneracaoDtremun { get; set; }
        public decimal? RemuneracaoVrsalfx { get; set; }
        public byte? RemuneracaoUndsalfixo { get; set; }
        public string RemuneracaoDscsalvar { get; set; } = string.Empty;
        public DateTime? LogDataOperacao { get; set; }
        public string LogCodUsuario { get; set; } = string.Empty;
        public long IdEsF2500Remuneracao { get; internal set; }
        public string DescricaoUnidadePagamento { get; set; }
    }
}
