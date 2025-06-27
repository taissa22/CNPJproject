using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class EstadoService : BaseCrudService<Estado, string>, IEstadoService {
        private readonly IEstadoRepository repository;

        public EstadoService(IEstadoRepository repository) : base(repository) {
            this.repository = repository;
        }

        public async Task<IEnumerable<EstadoDTO>> RecuperarListaEstados() {
            return await repository.RecuperarListaEstados();
        }
    }
}
