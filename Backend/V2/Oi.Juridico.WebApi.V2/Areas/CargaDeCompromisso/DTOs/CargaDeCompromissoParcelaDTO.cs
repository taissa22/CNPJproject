using Oi.Juridico.Contextos.V2.CargaDeCompromissoContext.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.CargaDeCompromisso.DTOs
{
    public class CargaDeCompromissoParcelaDTO
    {
        public decimal Id { get; set; }
        public decimal IdCompromisso { get; set; }
        public decimal? SeqLancamento { get; set; }
        public decimal? NroParcela { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? Vencimento { get; set; }
        public decimal? Status { get; set; }
        public string? MotivoExclusao { get; set; }
        public string? Deletado { get; set; }
        public DateTime? DeletadoDatahora { get; set; }
        public string? DeletadoLogin { get; set; }
        public short? CodLancamento { get; set; }
        public int CodProcesso { get; set; }
        public string? MotivoCancelamento { get; set; }
        public string? ComentarioCancelamento { get; set; }
        public string? ComentarioEstorno { get; set; }
        public string? UsrSolicCancelamento { get; set; }
        public string? DataSolicCancelamento { get; set; }
        public long? NumeroPedidoSAP { get; set; }

    }
}
