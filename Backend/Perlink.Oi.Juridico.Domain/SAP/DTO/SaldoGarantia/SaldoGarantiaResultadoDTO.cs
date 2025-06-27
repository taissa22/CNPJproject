using System;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia {
    public class SaldoGarantiaResultadoDTO {
        public long IdProcesso { get; set; }
        public string NumeroProcesso { get; set; }
        public string CodigoEstado { get; set; }
        public string DescricaoComarca { get; set; }
        public long CodigoVara { get; set; }
        public string DescricaoTipoVara { get; set; }
        public long CodigoProfissional { get; set; }
        public string DescricaoEmpresaGrupo { get; set; }
        public bool? Ativo { get; set; }
        public string DescricaoBanco { get; set; }
        public string DescricaoEscritorio { get; set; }
        public bool Estrategico { get; set; }
        public DateTime? DataFinalizacaoContabil { get; set; }
        public string DescricaoTipoGarantia { get; set; }
        public decimal ValorPrincipal { get; set; }
        public decimal ValorCorrecaoPrincipal { get; set; }
        public decimal ValorAjusteCorrecao { get; set; }
        public decimal ValorJurosPrincipal { get; set; }
        public decimal ValorAjusteJuros { get; set; }
        public decimal ValorPagamentoPrincipal { get; set; }
        public decimal ValorPagamentoCorrecao { get; set; }
        public decimal ValorPagamentosJuros { get; set; }
        public decimal ValorLevantadoPrincipal { get; set; }
        public decimal ValorLevantadoCorrecao { get; set; }
        public decimal ValorLevantadoJuros { get; set; }
        public decimal ValorSaldoPrincipal { get; set; }
        public decimal ValorSaldoCorrecao { get; set; }
        public decimal ValorSaldoJuros { get; set; }
        public decimal SaldoDepositoBloqueio { get; set; }
        public decimal SaldoGarantia { get; set; }
        public decimal ValorTotalPagoGarantia { get; set; }

        // Tributario ADM
        public string NomeOrgao { get; set;  }
        public string NomeCompetencia { get; set; }
        public string NomeMunicipio { get; set; }
        //public string CodigoRiscoPerda { get; set; }
        //public string NumeroAgencia { get; set; }
    }
}
