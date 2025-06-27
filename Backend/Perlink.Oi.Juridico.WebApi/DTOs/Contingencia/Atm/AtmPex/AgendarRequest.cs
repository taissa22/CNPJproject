using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Contingencia.Atm.AtmPex
{
    public class AgendarRequest
    {
        [Required]
        public string CodEstado { get; set; }
        [Required]
        public short CodIndice { get; set; }
    }
}
