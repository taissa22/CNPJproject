using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.DTO
{
    public class ExportacaoPrePosRJDTO
    {
        public long Id { get; set; }
        public DateTime DataExtracao { get; set; }
        public DateTime DataExecucao { get; set; }
        public bool Expurgar { get; set; }
        public string ArquivoJec { get; set; }
        public string ArquivoTrabalhista { get; set; }
        public string ArquivoCivelConsumidor { get; set; }
        public string ArquivoCivelEstrategico { get; set; }

        public string ArquivoPex { get; set; }
        public string ArquivoTributarioJudicial { get; set; }
        public string ArquivoAdministrativo { get; set; }

    }
}
