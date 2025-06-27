using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IVaraRepository : IBaseCrudRepository<Vara, long> {
        Task<bool> ExisteBBOrgaoVinculado(long idBBOrgao);
        Task<bool> ExisteCodigoBBOrgaoVinculado(long codigoBBOrgao);
    }
}
