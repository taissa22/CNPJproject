using Microsoft.EntityFrameworkCore;
using Shared.Domain.Interface;
using Shared.Domain.Interface.Entity;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Data
{
    public abstract class BaseRepository<TEntity, TType> : IBaseRepository<TEntity, TType>
         where TEntity : class, IEntity<TEntity, TType>
    {
        protected readonly DbContext context;
        protected readonly IAuthenticatedUser user;

        public BaseRepository(DbContext context, IAuthenticatedUser user)
        {
            this.context = context;
            this.user = user;
        }

        public virtual IQueryable<TEntity> Pesquisar(List<Expression<Func<TEntity, bool>>> predicate)
        {
            var retorno = context.Set<TEntity>().AsQueryable();
            foreach (var expression in predicate)
                retorno = retorno.Where(expression);
            return retorno;
        }

        public virtual IQueryable<TEntity> Pesquisar(Expression<Func<TEntity, bool>> predicate) => context.Set<TEntity>()
                                                                                                          .Where(predicate)
                                                                                                          .AsNoTracking()
                                                                                                          .AsQueryable();

        public virtual IQueryable<TEntity> Pesquisar()
        {
            return context.Set<TEntity>().AsQueryable();
        }

        public int getTotalFromSearch(Expression<Func<TEntity, bool>> predicate) => context.Set<TEntity>().Where(predicate).Count();

        public virtual async Task<TEntity> RecuperarPorId(TType id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<ICollection<TEntity>> RecuperarTodos()
        {
            return await context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public virtual async Task Inserir(TEntity entity)
        {
            //await context.Database.ExecuteSqlCommandAsync(string.Format("call jur.SP_ACA_INSERE_LOG_USUARIO('{0}', 'N')", user.Login));

            await context.Set<TEntity>().AddAsync(entity);
        }

        protected async Task Atualizar(TEntity entity)
        {
            //await context.Database.ExecuteSqlCommandAsync(string.Format("call jur.SP_ACA_INSERE_LOG_USUARIO('{0}', 'N')", user.Login));
            context.Entry(entity).State = EntityState.Modified;

            await Task.CompletedTask;
        }

        protected async Task Remover(TEntity entity)
        {
           // await context.Database.ExecuteSqlCommandAsync(string.Format("call jur.SP_ACA_INSERE_LOG_USUARIO('{0}', 'N')", user.Login));
            context.Set<TEntity>().Remove(entity);

            await Task.CompletedTask;
        }

        protected async Task RemoverPorId(TType id)
        {
            var entity = await RecuperarPorId(id);

            await Remover(entity);
        }

        public int Commit()
        {
            return context.SaveChanges();
        }

        public async Task<int> CommitAsync() {
            return await context.SaveChangesAsync();
        }

        public void ResetContextState() {
            context.ChangeTracker.Entries()
                        .Where(e => e.Entity != null).ToList()
                        .ForEach(e => e.State = EntityState.Detached);
        }
    }
}
