namespace Perlink.Oi.Juridico.Domain.SAP.DTO.EstornoLancamentoPago {
    public class DadosCompromissoEstornoDTO {
        public decimal ValorCompromisso { get; set; }
        public int QuantidadeCredores { get; set; }
        public long CodigoCompromisso { get; set; }
        public long CodigoParcela { get; set; }
        public decimal ValorParcela { get; set; }
    }
}