using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface ITipoVaraService : IBaseCrudService<TipoVara, long>
    {
        Task<IEnumerable<TipoVara>> RecuperarPorVaraEComarca(long codigoComarca, long codigoVara);
    }
}
