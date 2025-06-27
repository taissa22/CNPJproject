using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia
{
    public class AgendamentoResultadoDTO
    {
        public long Id { get; set; }
        public string NomeAgendamento { get; set; }
        public string StatusAgendamento { get; set; }
        public long CodigoStatusAgendamento { get; set; }
        public DateTime DataAgendamento { get; set; }
        public string DataFinalizacao { get; set; }
        public string MensagemErro { get; set; }
        public string NomeArquivo { get; set; }
        public long TipoProcesso { get; set; }
    }
}
