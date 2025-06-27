using Shared.Domain.Interface.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Domain.Interface.Service
{
    public interface IBaseService<TEntity, TTYpe> where TEntity : class, IEntity<TEntity, TTYpe>
    {
        Task<TEntity> RecuperarPorId(TTYpe id);

        Task<ICollection<TEntity>> RecuperarTodos();

        IQueryable<TEntity> Pesquisar(List<Expression<Func<TEntity, bool>>> predicate);

        IQueryable<TEntity> Pesquisar(Expression<Func<TEntity, bool>> predicate);
        
        IQueryable<TEntity> Pesquisar();

        int getTotalFromSearch(Expression<Func<TEntity, bool>> predicate);

        int Commit();
    }
}
