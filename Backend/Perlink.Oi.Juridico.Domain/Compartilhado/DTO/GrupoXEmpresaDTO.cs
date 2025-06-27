using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.DTO
{
    public class GrupoXEmpresaDTO
    {
        public long Id { get; set; }

        public string NomeGrupo { get; set; }

        public IEnumerable<EmpresaDoGrupoDTO> EmpresasGrupo { get; set; }
    }
}