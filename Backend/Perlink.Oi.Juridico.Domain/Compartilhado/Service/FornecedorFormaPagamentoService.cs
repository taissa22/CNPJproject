using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class FornecedorFormaPagamentoService : BaseCrudService<FornecedorFormaPagamento, long>, IFornecedorFormaPagamentoService {
        private readonly IFornecedorFormaPagamentoRepository repository;

        public FornecedorFormaPagamentoService(IFornecedorFormaPagamentoRepository repository) : base(repository) {
            this.repository = repository;
        }

        public async Task<bool> ExisteFormaPagamentoComFornecedor(long codigoFornecedor) {
            return await repository.ExisteFormaPagamentoComFornecedor(codigoFornecedor);
        }
    }
}
