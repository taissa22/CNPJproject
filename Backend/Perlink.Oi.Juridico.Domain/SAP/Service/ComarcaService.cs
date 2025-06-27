using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class ComarcaService : BaseCrudService<Comarca, long>, IComarcaService
    {
        private readonly IComarcaRepository repository;
        public ComarcaService(IComarcaRepository repository) : base(repository)
        {
            this.repository = repository;
        }
        public async Task<Comarca> RecuperarComarca(long Cod)
        {
            return await repository.RecuperarComarca(Cod);
        }
        public async Task<bool> ExisteBBComarcaAssociadoComarca(long codigo)
        {
            return await repository.ExisteBBComarcaAssociadoComarca(codigo);
        }

        public async Task<ICollection<ComarcaDTO>> RecuperarTodasComarca()
        {
            return await repository.RecuperarTodasComarca();
        }

        public async Task<ICollection<ComarcaDTO>> RecuperarComarcaPorEstado(string estado)
        {
            return await repository.RecuperarComarcaPorEstado(estado);
        }

    }
}
