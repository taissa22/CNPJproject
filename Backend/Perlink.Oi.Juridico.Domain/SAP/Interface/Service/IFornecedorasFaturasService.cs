using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{
    public interface IFornecedorasFaturasService : IBaseCrudService<FornecedorasFaturas, long>
    {
        Task<bool> ExisteFornecedorasFaturasComEmpresaSap(long codigoEmpresaSap);
    }
}
