using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface ICategoriaFinalizacaoRepository : IBaseCrudRepository<CategoriaFinalizacao, long>
    {
        Task<bool> ExisteCategoriaFinalizacao(long codCategoriaPagamento);
    }
}
