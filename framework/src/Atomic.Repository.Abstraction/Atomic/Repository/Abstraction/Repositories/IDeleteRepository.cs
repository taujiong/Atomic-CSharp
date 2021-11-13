using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Atomic.Repository.Abstraction.Entities;
using JetBrains.Annotations;

namespace Atomic.Repository.Abstraction.Repositories
{
    public interface IDeleteRepository<in TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        /// <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
        /// </param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        Task DeleteAsync(
            [NotNull] TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deletes multiple entities.
        /// </summary>
        /// <param name="entities">Entities to be deleted.</param>
        /// <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
        /// </param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>Awaitable <see cref="Task"/>.</returns>
        Task DeleteManyAsync(
            [NotNull] IEnumerable<TEntity> entities,
            bool autoSave = false,
            CancellationToken cancellationToken = default
        );
    }

    public interface IDeleteRepository<in TEntity, in TKey> : IDeleteRepository<TEntity>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity</param>
        /// <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
        /// </param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        Task<bool> DeleteAsync(
            TKey id,
            bool autoSave = false,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deletes multiple entities by primary keys.
        /// </summary>
        /// <param name="ids">Primary keys of the each entity.</param>
        /// <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
        /// </param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>Awaitable <see cref="Task"/>.</returns>
        Task DeleteManyAsync(
            [NotNull] IEnumerable<TKey> ids,
            bool autoSave = false,
            CancellationToken cancellationToken = default
        );
    }
}