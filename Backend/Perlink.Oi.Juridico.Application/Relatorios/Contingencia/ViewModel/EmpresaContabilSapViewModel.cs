using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Relatorios.Entity;
using Shared.Application.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Relatorios.Contingencia.ViewModel
{
    public class EmpresaContabilSapViewModel : BaseViewModel<long>
    {
        public string NomeEmpresa { get; set; }
        public bool Selecionada { get; set; }

        public IList<EmpresaContabilSapViewModel> ToViewModel(IList<Parte> empresas, GrupoEmpresaContabilSap grupo)
        {
            IList<EmpresaContabilSapViewModel> lstVm = new List<EmpresaContabilSapViewModel>();

            foreach (Parte empresa in empresas)
            {
                var vm = new EmpresaContabilSapViewModel()
                {
                    Id = empresa.Id,
                    NomeEmpresa = empresa.Nome,
                    Selecionada = grupo != null && grupo.GrupoEmpresaContabilSapParte.Select(y => y.EmpresaId).Any(x => empresa.Id == x)
                };
                lstVm.Add(vm);
            }
            return lstVm;
        }
    }
}