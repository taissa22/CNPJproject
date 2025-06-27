using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class CredorCompromissoService : BaseCrudService<CredorCompromisso, long>, ICredorCompromissoService {

        private readonly ICredorCompromissoRepository repository;

        public CredorCompromissoService(ICredorCompromissoRepository repository) : base(repository) {
            this.repository = repository;
        }

        public Task AtualizarCredorCompromisso(CredorCompromisso credorCompromisso) {
            return repository.Atualizar(credorCompromisso);
        }

        public async Task<CredorCompromisso> ObterCredorCompromisso(long codigoProcesso, long codigoCredorCompromisso) {
            return await repository.ObterCredorCompromisso(codigoProcesso, codigoCredorCompromisso);
        }
    }
}
