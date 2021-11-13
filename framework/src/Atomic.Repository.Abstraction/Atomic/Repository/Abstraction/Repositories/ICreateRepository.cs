using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Atomic.Repository.Abstraction.Entities;
using JetBrains.Annotations;

namespace Atomic.Repository.Abstraction.Repositories
{
    public interface ICreateRepository<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
        /// </param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <param name="entity">Inserted entity</param>
        Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts multiple new entities.
        /// </summary>
        /// <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
        /// </param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <param name="entities">Entities to be inserted.</param>
        /// <returns>Awaitable <see cref="Task"/>.</returns>
        Task InsertManyAsync(
            [NotNull] IEnumerable<TEntity> entities,
            bool autoSave = false,
            CancellationToken cancellationToken = default
        );
    }
}