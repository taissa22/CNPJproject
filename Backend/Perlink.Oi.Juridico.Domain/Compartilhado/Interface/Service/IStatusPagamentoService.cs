using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface IStatusPagamentoService : IBaseCrudService<StatusPagamento, long>
    {
        Task<IEnumerable<StatusPagamento>> RecuperarStatusPagamentoParaFiltroLote();
    }
}
