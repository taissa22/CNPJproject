using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class MotivoSuspensaoCancelamentoParcelaService : BaseCrudService<MotivoSuspensaoCancelamentoParcela, long>, IMotivoSuspensaoCancelamentoParcelaService {
        private readonly IMotivoSuspensaoCancelamentoParcelaRepository repository;

        public MotivoSuspensaoCancelamentoParcelaService(IMotivoSuspensaoCancelamentoParcelaRepository repository) : base(repository) {
            this.repository = repository;
        }
    }
}
