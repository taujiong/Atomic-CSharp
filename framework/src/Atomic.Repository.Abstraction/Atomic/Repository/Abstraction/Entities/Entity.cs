using System;

namespace Atomic.Repository.Abstraction.Entities
{
    /// <inheritdoc/>
    [Serializable]
    public abstract class Entity : IEntity
    {
        public abstract object[] GetKeys();

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Keys = {string.Join(", ", GetKeys())}";
        }

        public bool EntityEquals(IEntity other)
        {
            return EntityHelper.EntityEquals(this, other);
        }
    }

    /// <inheritdoc cref="IEntity{TKey}" />
    [Serializable]
    public abstract class Entity<TKey> : Entity, IEntity<TKey>
    {
        protected Entity()
        {
        }

        protected Entity(TKey id)
        {
            Id = id;
        }

        /// <inheritdoc/>
        public virtual TKey Id { get; protected set; }

        public override object[] GetKeys()
        {
            return new object[] { Id };
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Id = {Id}";
        }
    }
}