using Shared.Domain.Interface.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Domain.Interface.Repository {
    public interface IBaseRepository<TEntity, TType> where TEntity : class, IEntity<TEntity, TType> {
        Task<TEntity> RecuperarPorId(TType id);

        Task<ICollection<TEntity>> RecuperarTodos();
        IQueryable<TEntity> Pesquisar(List<Expression<Func<TEntity, bool>>> predicate);

        IQueryable<TEntity> Pesquisar(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Pesquisar();

        int getTotalFromSearch(Expression<Func<TEntity, bool>> predicate);

        int Commit();

        Task<int> CommitAsync();

        void ResetContextState();
    }
}
