using Oi.Juridico.Shared.V2.Enums;

namespace Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.DTOs
{
    public class VAgendaTrabalhistaExportarDTO
    {
        public int? CodProcesso { get; set; }
        public DateTime? DateAudiencia { get; set; }
        public DateTime? HorarioAudiencia { get; set; }
        public string? TipoAudiencia { get; set; }
        public string? NumeroProcesso { get; set; }
        public string? ClassificacaoHierarquica { get; set; }
        public string? DescricaoClassificHierarquica {
            get
            {
                var desc = !string.IsNullOrEmpty(ClassificacaoHierarquica) ? ClassificacaoHierarquica == "U" ? "Único" : ClassificacaoHierarquica == "P" ? "Primário" : "Secundário" : string.Empty;

                return desc;
            }
        }
        public string? ProcessoAtivo { get; set; }
        public string? EscritorioProcesso { get; set; }
        public string? Estado { get; set; }
        public string? DescricaoEstado { get; set; }
        public string? Comarca { get; set; }
        public short? CodVara { get; set; }
        public short? CodTipoVara { get; set; }
        public string? TipoVara { get; set; }
        public string? UsuarioUltAteracao { get; set; }
        public DateTime? DataUltAtualizacao { get; set; }
        public string? ClassificacaoProcesso { get; set; }
        public string? DescrcaoClassificacaoProcesso {
            get
            {
                var desc = !string.IsNullOrEmpty(ClassificacaoProcesso) ? ClassificacaoProcesso == "P" ? "Próprio" : "Terceiro" : string.Empty;

                return desc;
            }
        }
        public string? Estrategico { get; set; }
        public int? SeqAudiencia { get; set; }
        public string? Reclamantes { get; set; }
        public string? Reclamadas { get; set; }
        public string? Preposto { get; set; }
        public string? DescModalidade { get; set; }
        public string? ModalidadeAtivo { get; set; }
        public string? DescLocalidade { get; set; }
        public string? LocalidadeAtivo { get; set; }
        public string? Link { get; set; }
    }
}
