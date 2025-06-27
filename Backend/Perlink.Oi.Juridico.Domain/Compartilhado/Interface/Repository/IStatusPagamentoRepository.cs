using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IStatusPagamentoRepository : IBaseCrudRepository<StatusPagamento, long>
    {
        Task<IEnumerable<StatusPagamento>> RecuperarStatusPagamentoParaFiltroLote();
    }
}
