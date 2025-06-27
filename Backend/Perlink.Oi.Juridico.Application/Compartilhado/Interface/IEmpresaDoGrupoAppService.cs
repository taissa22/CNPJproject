using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Interface
{
    public interface IEmpresaDoGrupoAppService: IBaseCrudAppService<EmpresaDoGrupoViewModel, Parte, long>
    {
        Task<IResultadoApplication<IEnumerable<EmpresaDoGrupoListaViewModel>>> RecuperarEmpresaDoGrupo();
    }
}
