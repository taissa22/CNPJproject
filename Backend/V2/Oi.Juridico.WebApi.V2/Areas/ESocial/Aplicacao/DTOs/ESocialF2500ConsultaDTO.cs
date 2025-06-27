using Newtonsoft.Json;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs
{
    public class ESocialF2500ConsultaDTO
    {
        public int IdF2500 { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? CodProcesso { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CodParte { get; set; }
        public byte StatusFormulario { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IndRetificado { get; set; } = false;
        public string NroRecibo { get; set; } = string.Empty;
        public string LogCodUsuario { get; set; } = string.Empty;
        public string NomeUsuario { get; set; } = string.Empty;
        public DateTime LogDataOperacao { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? FinalizadoEscritorio { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? FinalizadoContador { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DataRetornoOK { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DataRetornoExclusao { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ESocialF2500ConsultaDTO>? Historico { get; set; }
        public bool ExibirRetorno2500 { get; set; }
        public string? VersaoEsocial { get; set; }
    }
}
