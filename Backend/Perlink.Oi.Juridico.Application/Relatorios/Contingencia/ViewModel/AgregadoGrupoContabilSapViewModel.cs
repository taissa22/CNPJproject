using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Relatorios.Contingencia.ViewModel
{
    public class AgregadoGrupoContabilSapViewModel
    {
        public List<GrupoXEmpresaViewModel> GrupoXEmpresas { get; set; }

        public List<EmpresaViewModel> EmpresasDisponiveis { get; set; }
    }
}