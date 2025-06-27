using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Atm
{
    public class AgendarRequest
    {
        [Required]
        public string Estado { get; set; }
        [Required]
        public short Indice { get; set; }
    }
}
