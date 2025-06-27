using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface ITipoProcessoRepository : IBaseCrudRepository<TipoProcesso, long> {
        Task<IEnumerable<TipoProcesso>> RecuperarTodosSAP(long[] codigoTipoProcessoSAP);
    }
}