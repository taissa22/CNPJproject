using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface ITipoLancamentoService : IBaseCrudService<TipoLancamento, long>
    {
        Task<IEnumerable<TipoLancamento>> RecuperarTipoLancamentoParaFiltroLote();
    }
}
