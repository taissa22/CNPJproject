using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository
{
    public interface IFornecedorasFaturasRepository : IBaseCrudRepository<FornecedorasFaturas, long>
    {
        Task<bool> ExisteFornecedorasFaturasComEmpresaSap(long codigoEmpresaSap);
    }
}
