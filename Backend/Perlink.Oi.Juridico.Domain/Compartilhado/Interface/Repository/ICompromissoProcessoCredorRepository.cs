using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface ICompromissoProcessoCredorRepository : IBaseCrudRepository<CompromissoProcessoCredor, long> {
        Task<CompromissoProcessoCredor> ObterCompromissoProcessoCredor(long codigoProcesso, long codigoCompromisso);
    }
}
