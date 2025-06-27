using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.ParametrizacaoClosing.Dtos
{
    public class AtualizarRequest
    {
        [Required]
        public byte CodTipoProcesso { get; set; }
        public string IndClosingHibrido { get; set; } = "";
        public decimal? PercResponsabilidade { get; set; }
        public byte? ClassificaoClosing { get; set; }
        public int? IdEscritorioPadrao { get; set; }
        public string IndClosingHibridoClientCO { get; set; } = "";
        public byte? ClassificaoClosingClientCO { get; set; }
    }
}
