using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class StatusPagamentoService : BaseCrudService<StatusPagamento, long>, IStatusPagamentoService
    {
        private readonly IStatusPagamentoRepository repository;

        public StatusPagamentoService(IStatusPagamentoRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<StatusPagamento>> RecuperarStatusPagamentoParaFiltroLote()
        {
            return await repository.RecuperarStatusPagamentoParaFiltroLote();
        }
    }
}