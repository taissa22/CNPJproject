using FluentValidation;
using Shared.Domain.Impl.Validator;
using Shared.Domain.Interface.Entity;

namespace Shared.Domain.Impl.Entity
{
    public abstract class EntityCrud<TEntity, TType> : Entity<TEntity, TType>, IEntityCrud<TEntity, TType> where TEntity : class
    {
        public abstract AbstractValidator<TEntity> Validator { get; }

        public abstract void PreencherDados(TEntity data);
        public abstract ResultadoValidacao Validar();

        protected virtual ResultadoValidacao ExecutarValidacaoPadrao(TEntity entity)
        {
            return Validator.Validate(entity).Transformar();
        }
    }
}
