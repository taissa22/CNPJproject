using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface ICredorCompromissoService : IBaseCrudService<CredorCompromisso, long> {
        Task<CredorCompromisso> ObterCredorCompromisso(long codigoProcesso, long codigoCredorCompromisso);
        Task AtualizarCredorCompromisso(CredorCompromisso credorCompromisso);
    }
}
