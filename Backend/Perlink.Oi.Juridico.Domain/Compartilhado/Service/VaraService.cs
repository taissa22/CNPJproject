using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class VaraService : BaseCrudService<Vara, long>, IVaraService {
        private readonly IVaraRepository repository;

        public VaraService(IVaraRepository repository) : base(repository) {
            this.repository = repository;
        }

        public async Task<bool> ExisteBBOrgaoVinculado(long idBBOrgao) {
            return await repository.ExisteBBOrgaoVinculado(idBBOrgao);
        }
        
    }
}
