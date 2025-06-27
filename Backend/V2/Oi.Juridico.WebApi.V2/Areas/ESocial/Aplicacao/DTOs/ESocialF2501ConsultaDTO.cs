using Newtonsoft.Json;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs
{
    public class ESocialF2501ConsultaDTO
    {
        public int IdF2501 { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? CodProcesso { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CodParte { get; set; }
        public byte StatusFormulario { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IndRetificado { get; set; } = false;
        public string NroRecibo { get; set; } = string.Empty;
        public string PeriodoApuracao { get; set; } = string.Empty;
        public string LogCodUsuario { get; set; } = string.Empty;
        public string NomeUsuario { get; set; } = string.Empty;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? FinalizadoEscritorio { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? FinalizadoContador { get; set; }
        public DateTime LogDataOperacao { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ESocialF2501ConsultaDTO>? Historico { get; set; }
        public bool ExibirRetorno2501 { get; set; }
        public string? VersaoEsocial { get; set; }
    }
}
