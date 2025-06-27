using Shared.Domain.Interface.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Domain.Interface.Repository
{
    public interface IBaseCrudRepository<TEntity, TType> : IBaseRepository<TEntity, TType>
       where TEntity : class, IEntityCrud<TEntity, TType>
    {
        Task Inserir(TEntity entity);

        Task Atualizar(TEntity entity);

        Task Remover(TEntity entity);

        Task RemoverPorId(TType id);

        Task<IDictionary<string, string>> RecuperarDropDown();
    }
}
