using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.WebApi.DTOs.Seguranca
{
    public class SegurancaResponse
    {
       public int IdAgendamento { get; set; }
       public DateTime? DatSolicitacao { get; set; }
       public DateTime? DatInicioExecucao { get; set; }
       public DateTime? DatFimExecucao { get; set; }
       public byte? Status { get; set; }
       public string NomeUsuario { get; set; }
        public string MensagemErro { get; set; }
    }
}
