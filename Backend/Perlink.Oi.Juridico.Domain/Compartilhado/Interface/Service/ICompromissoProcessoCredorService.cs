using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface ICompromissoProcessoCredorService : IBaseCrudService<CompromissoProcessoCredor, long> {
        Task<CompromissoProcessoCredor> ObterCompromissoProcessoCredor(long codigoProcesso, long codigoCompromisso);
    }
}
