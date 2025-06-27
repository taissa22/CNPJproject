using Shared.Domain.Interface.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Application.Interface
{
    public interface IBaseAppService<TViewModel, TEntity, TType> where TEntity : class, IEntity<TEntity, TType>
    {
        Task<IResultadoApplication<ICollection<TViewModel>>> RecuperarTodos();
        Task<IResultadoApplication<TViewModel>> RecuperarPorId(TType id);
    }
}
