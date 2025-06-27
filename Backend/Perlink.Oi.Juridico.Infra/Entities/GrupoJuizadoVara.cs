using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class GrupoJuizadoVara
    {
        public int GrupoJuizadoId { get; private set; }
        public int ComarcaId { get; private set; }
        public int VaraId { get; private set; }
        public int TipoVaraId { get; private set; }
    }
}
