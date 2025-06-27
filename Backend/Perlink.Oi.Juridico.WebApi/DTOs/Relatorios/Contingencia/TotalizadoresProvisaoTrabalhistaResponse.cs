using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Relatorios.Contingencia
{
    public class TotalizadoresProvisaoTrabalhistaResponse
    {
        public decimal IfptrIdItemFechProvTrab { get; set; }
        public int QtdRegistros { get; set; }
        public decimal QtePedidosProvavelP { get; set; }
        public decimal? ExpectativaPerdaP { get; internal set; }
        public decimal? ValProvContPrincipalP { get; internal set; }
        public decimal? ValProvContJurosP { get; internal set; }
        public decimal? ValProvContPrincipalH { get; internal set; }
        public decimal? ValProvContJurosH { get; internal set; }
        public decimal? ValProvContPrincipal { get; internal set; }
        public decimal? ValProvContJuros { get; internal set; }
        public decimal? ExpectativaPerdaH { get; internal set; }
        public decimal QtePedidosProvavelH { get; set; }
    }
}
