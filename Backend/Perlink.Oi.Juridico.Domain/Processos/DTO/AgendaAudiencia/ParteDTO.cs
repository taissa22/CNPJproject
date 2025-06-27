using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia
{
    public class PartesDTO
    {
        public long CodigoParte { get; set; }
        public string NomeParte { get; set; }
        public string CgcParte { get; set; }
        public string CPF { get; set; }
        public long? CarteiraTrabalhoParte { get; set; }        



    }
}
