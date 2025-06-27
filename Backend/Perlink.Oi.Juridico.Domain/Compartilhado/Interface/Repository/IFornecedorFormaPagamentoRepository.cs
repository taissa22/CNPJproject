using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IFornecedorFormaPagamentoRepository : IBaseCrudRepository<FornecedorFormaPagamento, long> {
        Task<bool> ExisteFormaPagamentoComFornecedor(long codigoFornecedor);
    }
}
