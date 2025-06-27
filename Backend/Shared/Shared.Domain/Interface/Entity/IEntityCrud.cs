using Shared.Domain.Impl.Validator;

namespace Shared.Domain.Interface.Entity
{
    public interface IEntityCrud<TEntity, TType> : IEntity<TEntity, TType> where TEntity : class
    {
        ResultadoValidacao Validar();
        void PreencherDados(TEntity data);
    }
}
