using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface ITipoLancamentoRepository : IBaseCrudRepository<TipoLancamento, long>
    {
        Task<IEnumerable<TipoLancamento>> RecuperarTipoLancamentoParaFiltroLote();
        Task<IEnumerable<ComboboxDTO>> RecuperarTodosSAP(long codigoTipoProcesso);
    }
}
