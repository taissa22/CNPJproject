using Flunt.Notifications;
using Shared.Domain.Interface.Entity;

namespace Shared.Domain.Impl.Entity
{
    public abstract class Entity<TEntity, TType> : IEntity<TEntity, TType> where TEntity : class
    {
        public TType Id { get; set; }
    }

    public abstract class Entity : Notifiable
    {
        public long Id { get; set; }
    }
}
