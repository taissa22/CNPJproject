using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class TipoLancamentoService : BaseCrudService<TipoLancamento, long>, ITipoLancamentoService {
        private readonly ITipoLancamentoRepository repository;

        public TipoLancamentoService(ITipoLancamentoRepository repository) : base(repository) {
            this.repository = repository;
        }

        public async Task<IEnumerable<TipoLancamento>> RecuperarTipoLancamentoParaFiltroLote()
        {
            return await  repository.RecuperarTipoLancamentoParaFiltroLote();
        }
    }
}
