using Shared.Domain.Interface.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Application.Interface
{
    public interface IBaseCrudAppService<TViewModel, TEntity, TType> : IBaseAppService<TViewModel, TEntity, TType>
         where TEntity : class, IEntity<TEntity, TType>
    {
        Task<IResultadoApplication> Inserir(TViewModel viewModel);
        Task<IResultadoApplication> Atualizar(TViewModel viewModel);
        Task<IResultadoApplication> RemoverPorId(TType id);
        Task<IResultadoApplication<IDictionary<string, string>>> RecuperarDropdown();
    }
}
