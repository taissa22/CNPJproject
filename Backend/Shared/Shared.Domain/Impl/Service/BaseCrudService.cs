using Shared.Domain.Impl.Validator;
using Shared.Domain.Interface.Entity;
using Shared.Domain.Interface.Repository;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Domain.Impl.Service
{
    public abstract class BaseCrudService<TEntity, TType> : BaseService<TEntity, TType>, IBaseCrudService<TEntity, TType>
        where TEntity : class, IEntityCrud<TEntity, TType>
    {
        private readonly IBaseCrudRepository<TEntity, TType> repository;

        public BaseCrudService(IBaseCrudRepository<TEntity, TType> repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<IDictionary<string, string>> RecuperarDropDown()
        {
            return await repository.RecuperarDropDown();
        }

        public virtual async Task<ResultadoValidacao> Atualizar(TEntity model)
        {
            var entidade = await base.RecuperarPorId(model.Id);

            entidade.PreencherDados(model);

            var validate = entidade.Validar();

            if (validate.IsValid)
                await repository.Atualizar(entidade);

            return validate;
        }

        public virtual async Task<ResultadoValidacao> Inserir(TEntity model)
        {
            var validate = model.Validar();

            if (validate.IsValid)
                await repository.Inserir(model);

            return validate;
        }

        public virtual async Task RemoverPorId(TType id)
        {
            await repository.RemoverPorId(id);
        }
        public virtual async Task Remover(TEntity entity)
        {
            await repository.Remover(entity);
        }
    }
}
