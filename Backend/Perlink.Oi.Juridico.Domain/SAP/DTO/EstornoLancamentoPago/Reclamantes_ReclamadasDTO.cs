using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.DTO
{
    public class Reclamantes_ReclamadasDTO
    {
        public long CodigoProcesso { get; set; }
        public long CodigoParte { get; set; }
        public string CodigoTipoParticipacao { get; set; }
        public string NomeParte { get; set; }
        public string CGCParte { get; set; }
        public string CPF { get; set; }
        public long? CarteiradeTrabalho { get; set; }

    }
}
