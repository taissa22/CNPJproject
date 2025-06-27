using Shared.Domain.Impl.Service;
using System.Threading.Tasks;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class CompromissoProcessoCredorService : BaseCrudService<CompromissoProcessoCredor, long>, ICompromissoProcessoCredorService
    {
        private readonly ICompromissoProcessoCredorRepository repository;

        public CompromissoProcessoCredorService(ICompromissoProcessoCredorRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<CompromissoProcessoCredor> ObterCompromissoProcessoCredor(long codigoProcesso, long codigoCompromisso) {
            return await repository.ObterCompromissoProcessoCredor(codigoProcesso, codigoCompromisso);
        }
    }
}
