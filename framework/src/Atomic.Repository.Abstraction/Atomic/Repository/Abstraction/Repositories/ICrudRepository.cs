using Atomic.Repository.Abstraction.Entities;

namespace Atomic.Repository.Abstraction.Repositories
{
    public interface ICrudRepository<TEntity>
        : IReadOnlyRepository<TEntity>,
            ICreateRepository<TEntity>,
            IUpdateRepository<TEntity>,
            IDeleteRepository<TEntity>
        where TEntity : class, IEntity
    {
    }

    public interface ICrudRepository<TEntity, in TKey>
        : ICrudRepository<TEntity>,
            IReadOnlyRepository<TEntity, TKey>,
            IDeleteRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
    }
}