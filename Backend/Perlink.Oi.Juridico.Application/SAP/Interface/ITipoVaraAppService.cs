using Perlink.Oi.Juridico.Application.SAP.ViewModel.VariosContextos;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface
{
    public interface ITipoVaraAppService : IBaseCrudAppService<ComboboxViewModel<long>, TipoVara, long>
    {
        Task<IResultadoApplication<IEnumerable<ComboboxViewModel<long>>>> RecuperarPorVaraEComarca(long codigoComarca, long codigoVara);
    }
}
