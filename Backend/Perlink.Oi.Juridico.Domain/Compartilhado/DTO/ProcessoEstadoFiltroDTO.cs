using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.DTO
{
    public class ProcessoEstadoFiltroDTO
    {
        public string NumeroProcesso { get; set; }
        public string Comarca { get; set; }
        public string Vara { get; set; }
        public string TipoVara { get; set; }
        public string Estado { get; set; }

        public ProcessoEstadoFiltroDTO(ProcessoDTO processoDTO)
        {
            this.NumeroProcesso = processoDTO.NumeroProcesso;
            this.Comarca = processoDTO.Comarca;
            this.Vara = processoDTO.Vara;
            this.TipoVara = processoDTO.TipoVara;
            this.Estado = processoDTO.Estado;
        }
    }
}
