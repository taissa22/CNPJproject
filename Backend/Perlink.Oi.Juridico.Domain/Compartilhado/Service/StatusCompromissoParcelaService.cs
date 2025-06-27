using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class StatusCompromissoParcelaService : BaseCrudService<StatusCompromissoParcela, long>, IStatusCompromissoParcelaService {
        private readonly IStatusCompromissoParcelaRepository repository;

        public StatusCompromissoParcelaService(IStatusCompromissoParcelaRepository repository) : base(repository) {
            this.repository = repository;
        }
    }
}
