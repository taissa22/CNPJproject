using Oi.Juridico.Contextos.V2.CargaDeCompromissoContext.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.CargaDeCompromisso.DTOs
{
    public class CargaDeCompromissoDTO
    {
        public decimal Id { get; set; }
        public decimal? CodAgendCargaComp { get; set; }
        public int? CodProcesso { get; set; }
        public decimal? CodTipoProcesso { get; set; }
        public decimal? CodCatPagamento { get; set; }
        public string? DocAutor { get; set; }
        public decimal? QtdParcelas { get; set; }
        public string? MotivoExclusao { get; set; }
        public string? NomeBeneficiario { get; set; }
        public DateTime? DataPrimeiraParcela { get; set; }
        public decimal? NroGuia { get; set; }
        public decimal? CodBancoArrecadador { get; set; }
        public decimal? CodFornecedor { get; set; }
        public decimal? CodFormaPgto { get; set; }
        public decimal? CodCentroCusto { get; set; }
        public string? ComentarioLancamento { get; set; }
        public string? ComentarioSap { get; set; }
        public string? BorderoBeneficiario { get; set; }
        public string? BorderoDoc { get; set; }
        public decimal? BorderoBanco { get; set; }
        public decimal? BorderoBancoDv { get; set; }
        public decimal? BorderoAgencia { get; set; }
        public decimal? BorderoAgenciaDv { get; set; }
        public decimal? BorderoCc { get; set; }
        public decimal? BorderoCcDv { get; set; }
        public decimal? BorderoValor { get; set; }
        public string? BorderoCidade { get; set; }
        public string? BorderoHistorico { get; set; }
        public decimal? ValorTotal { get; set; }
        public decimal? CodigoCredor { get; set; }
        public string? NomeCredor { get; set; }
        public string? DocCredor { get; set; }
        public string? StatusProcesso { get; set; }
        public string? ClasseCredito { get; set; }
        public long? NumeroPedidoSAP { get; set; }


        public List<CargaDeCompromissoParcelaDTO> cargaCompromissoParcela { get; set; }
    }
}
