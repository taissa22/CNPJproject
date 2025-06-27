using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class TipoVaraService : BaseCrudService<TipoVara, long>, ITipoVaraService
    {
        private readonly ITipoVaraRepository repository;

        public TipoVaraService(ITipoVaraRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<TipoVara>> RecuperarPorVaraEComarca(long codigoComarca, long codigoVara)
        {
            return await repository.RecuperarPorVaraEComarca(codigoComarca, codigoVara);
        }
    }
}