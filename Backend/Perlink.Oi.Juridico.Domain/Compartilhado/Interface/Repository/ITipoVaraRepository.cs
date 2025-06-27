using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface ITipoVaraRepository : IBaseCrudRepository<TipoVara, long>
    {
        Task<IEnumerable<TipoVara>> RecuperarPorVaraEComarca(long codigoComarca, long codigoVara);
    }
}