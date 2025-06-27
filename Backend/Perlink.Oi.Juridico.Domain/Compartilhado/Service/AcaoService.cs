using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class AcaoService : BaseCrudService<Acao, long>, IAcaoService
    {
        private readonly IAcaoRepository repository;

        public AcaoService(IAcaoRepository repository) : base(repository) {
            this.repository = repository;
        }

        public async Task<bool> ExisteBBNaturezaAcaoAssociadoAcao(long id)
        {
            return await repository.ExisteBBNaturezaAcaoAssociadoAcao(id);
        }
    }
}
