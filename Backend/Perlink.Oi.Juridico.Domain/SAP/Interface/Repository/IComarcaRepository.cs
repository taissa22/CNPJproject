using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository
{
    public interface IComarcaRepository : IBaseCrudRepository<Comarca, long>
    {
        Task<Comarca> RecuperarComarca(long Cod);
        Task<bool> ExisteBBComarcaAssociadoComarca(long codigo);
        Task<ICollection<ComarcaDTO>> RecuperarTodasComarca();

        Task<ICollection<ComarcaDTO>> RecuperarComarcaPorEstado(string estado);
    }
}
