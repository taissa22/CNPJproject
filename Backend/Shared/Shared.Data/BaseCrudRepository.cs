using Microsoft.EntityFrameworkCore;
using Shared.Domain.Interface;
using Shared.Domain.Interface.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Data
{
    public abstract class BaseCrudRepository<TEntity, TType> : BaseRepository<TEntity, TType>, IBaseCrudRepository<TEntity, TType>
        where TEntity : class, IEntityCrud<TEntity, TType>
    {
        public BaseCrudRepository(DbContext context, IAuthenticatedUser user) : base(context, user)
        {
        }

        public abstract Task<IDictionary<string, string>> RecuperarDropDown();

        async Task IBaseCrudRepository<TEntity, TType>.Inserir(TEntity entity)
        {
            await base.Inserir(entity);
        }

        async Task IBaseCrudRepository<TEntity, TType>.Atualizar(TEntity entity)
        {
            await base.Atualizar(entity);
        }

        async Task IBaseCrudRepository<TEntity, TType>.Remover(TEntity entity)
        {
            await base.Remover(entity);
        }

        async Task IBaseCrudRepository<TEntity, TType>.RemoverPorId(TType id)
        {
            await base.RemoverPorId(id);
        }

    }
}
