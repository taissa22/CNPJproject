using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface ICompromissoProcessoRepository : IBaseCrudRepository<CompromissoProcesso, long> {
        Task AtualizarCompromisso(long codigoProcesso, long codigoCompromisso);
    }
}
