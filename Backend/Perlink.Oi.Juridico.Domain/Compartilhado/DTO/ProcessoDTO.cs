using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.DTO
{
    public class ProcessoDTO
    {
        public long Id { get; set; }
        public string NumeroProcesso { get; set; }
        public string Estado { get; set; }
        public string Comarca { get; set; }
        public string Vara { get; set; }
        public string TipoVara { get; set; }
        public string EmpresaGrupo { get; set; }
        
    }
}
