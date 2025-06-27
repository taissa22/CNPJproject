using Perlink.Oi.Juridico.Application.SAP.ViewModel.VariosContextos;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Interface
{
    public interface IVaraAppService
    {
        Task<IResultadoApplication<IEnumerable<ComboboxViewModel<long>>>> RecuperarVaraPorComarca(long codigoComarca);
    }
}
