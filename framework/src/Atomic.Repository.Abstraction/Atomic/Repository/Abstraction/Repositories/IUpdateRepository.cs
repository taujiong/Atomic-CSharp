using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Atomic.Repository.Abstraction.Entities;
using JetBrains.Annotations;

namespace Atomic.Repository.Abstraction.Repositories
{
    public interface IUpdateRepository<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
        /// </param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <param name="entity">Entity</param>
        [NotNull]
        Task<TEntity> UpdateAsync(
            [NotNull] TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Updates multiple entities.
        /// </summary>
        /// <param name="entities">Entities to be updated.</param>
        /// <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>Awaitable <see cref="Task"/>.</returns>
        Task UpdateManyAsync(
            [NotNull] IEnumerable<TEntity> entities,
            bool autoSave = false,
            CancellationToken cancellationToken = default
        );
    }
}