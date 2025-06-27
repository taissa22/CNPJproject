using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface ICredorCompromissoRepository : IBaseCrudRepository<CredorCompromisso, long> {
        Task<CredorCompromisso> ObterCredorCompromisso(long codigoProcesso, long codigoCredorCompromisso);
        Task<CredorCompromisso> ObterCredorCompromisso(long codigoProcesso);
    }
}
