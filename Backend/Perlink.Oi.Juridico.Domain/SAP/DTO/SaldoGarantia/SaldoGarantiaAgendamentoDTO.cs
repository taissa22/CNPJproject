using Perlink.Oi.Juridico.Domain.SAP.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia
{
    public class SaldoGarantiaAgendamentoDTO
    {
        #region Critérios principal
        public string NomeAgendamento { get; set; }
        public int TipoProcesso { get; set; }
        public int StatusDoProcesso { get; set; }
        public DateTime? DataFinalizacaoContabilInicio { get; set; }
        public DateTime? DataFinalizacaoContabilFim { get; set; }
        public decimal? ValorDepositoInicio { get; set; }
        public decimal? ValorDepositoFim { get; set; }
        public decimal? ValorBloqueioInicio { get; set; }
        public decimal? ValorBloqueioFim { get; set; }
        public bool UmBloqueio { get; set; }
        public IEnumerable<long?> TipoGarantia { get; set; }
        public IEnumerable<long?> RiscoPerda { get; set; }
        public string NumeroAgencia { get; set; }
        public string NumeroConta { get; set; }
        public int? ConsiderarMigrados { get; set; }

        #endregion Critérios principal

        public IEnumerable<long?> IdsBanco { get; set; }
        public IEnumerable<long?> IdsEmpresaGrupo { get; set; }
        public IEnumerable<string> IdsEstado { get; set; }
        public IEnumerable<long?> IdsProcesso { get; set; }
    }
}
