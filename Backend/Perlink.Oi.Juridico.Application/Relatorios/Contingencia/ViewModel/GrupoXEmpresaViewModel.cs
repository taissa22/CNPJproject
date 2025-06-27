using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Relatorios.Contingencia.ViewModel
{
    public class GrupoXEmpresaViewModel
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string NomeAnterior { get; set; }
        public bool Persistido { get; set; }
        public int QtdEmpresasIniciaisControle { get; set; }
        public IEnumerable<EmpresaViewModel> EmpresasGrupo { get; set; }
        public bool? Recuperanda { get; set; }
        public bool? RecuperandaAnterior { get; set; }
    }
}