using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Movimentacoes.MovimentacoesPex
{
    public class IncluirMovimentacoesPex
    {
        public int FechamentoPexMediaIniCodSolic { get; set; }
        public int FechamentoPexMediaFimCodSolic { get; set; }
        public DateTime DatFechamentoIni { get; set; }
        public DateTime DatFechamentoFim { get; set; }
    }
}
