﻿using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Atomic.Repository.Abstraction.Entities;

namespace Atomic.Repository.Abstraction.Repositories
{
    public interface IRepository<TEntity> : IBasicRepository<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Get a single entity by the given <paramref name="predicate"/>.
        /// <para>
        /// It returns null if there is no entity with the given <paramref name="predicate"/>.
        /// It throws <see cref="InvalidOperationException"/> if there are multiple entities with the given <paramref name="predicate"/>.
        /// </para>
        /// </summary>
        /// <param name="predicate">A condition to find the entity</param>
        /// <param name="includeDetails">Set true to include all children of this entity</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        Task<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Get a single entity by the given <paramref name="predicate"/>.
        /// <para>
        /// It throws <see cref="EntityNotFoundException"/> if there is no entity with the given <paramref name="predicate"/>.
        /// It throws <see cref="InvalidOperationException"/> if there are multiple entities with the given <paramref name="predicate"/>.
        /// </para>
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="includeDetails">Set true to include all children of this entity</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deletes many entities by the given <paramref name="predicate"/>.
        /// <para>
        /// Please note: This may cause major performance problems if there are too many entities returned for a
        /// given predicate and the database provider doesn't have a way to efficiently delete many entities.
        /// </para>
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
        /// </param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        Task DeleteAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool autoSave = false,
            CancellationToken cancellationToken = default
        );
    }

    public interface IRepository<TEntity, in TKey> : IRepository<TEntity>, IBasicRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
    }
}