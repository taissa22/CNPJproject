using Shared.Domain.Impl.Validator;
using Shared.Domain.Interface.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Domain.Interface.Service
{
    public interface IBaseCrudService<TEntity, TType> : IBaseService<TEntity, TType> where TEntity : class, IEntity<TEntity, TType>
    {
        Task<ResultadoValidacao> Inserir(TEntity model);

        Task<ResultadoValidacao> Atualizar(TEntity model);

        Task RemoverPorId(TType id);
        Task Remover(TEntity entity);

        Task<IDictionary<string, string>> RecuperarDropDown();
    }
}
