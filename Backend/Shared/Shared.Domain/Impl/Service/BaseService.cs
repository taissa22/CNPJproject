using Shared.Domain.Interface.Entity;
using Shared.Domain.Interface.Repository;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Domain.Impl.Service
{
    public abstract class BaseService<TEntity, TTYpe> : IBaseService<TEntity, TTYpe>
        where TEntity : class, IEntity<TEntity, TTYpe>
    {
        private readonly IBaseRepository<TEntity, TTYpe> repository;

        public BaseService(IBaseRepository<TEntity, TTYpe> repository)
        {
            this.repository = repository;
        }

        public virtual IQueryable<TEntity> Pesquisar(List<Expression<Func<TEntity, bool>>> predicate)
        {
            return repository.Pesquisar(predicate);
        }

        public virtual IQueryable<TEntity> Pesquisar(Expression<Func<TEntity, bool>> predicate) => repository.Pesquisar(predicate);

        public virtual IQueryable<TEntity> Pesquisar()
        {
            return repository.Pesquisar();
        }

        public int getTotalFromSearch(Expression<Func<TEntity, bool>> predicate) => repository.getTotalFromSearch(predicate);

        public virtual async Task<TEntity> RecuperarPorId(TTYpe id)
        {
            return await repository.RecuperarPorId(id);
        }

        public virtual async Task<ICollection<TEntity>> RecuperarTodos()
        {
            return await repository.RecuperarTodos();
        }

        public int Commit()
        {
            return repository.Commit();
        }
    }
}
