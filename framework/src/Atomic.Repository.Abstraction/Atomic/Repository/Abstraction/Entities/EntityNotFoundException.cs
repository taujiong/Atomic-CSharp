using System;

namespace Atomic.Repository.Abstraction.Entities
{
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="EntityNotFoundException"/> object.
        /// </summary>
        public EntityNotFoundException()
        {
        }

        /// <summary>
        /// Creates a new <see cref="EntityNotFoundException"/> object.
        /// </summary>
        public EntityNotFoundException(Type entityType, object id = null, Exception innerException = null)
            : base(
                id == null
                    ? $"There is no such an entity given id. Entity type: {entityType.FullName}"
                    : $"There is no such an entity. Entity type: {entityType.FullName}, id: {id}",
                innerException)
        {
            EntityType = entityType;
            Id = id;
        }

        /// <summary>
        /// Creates a new <see cref="EntityNotFoundException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="EntityNotFoundException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Type of the entity.
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// Id of the Entity.
        /// </summary>
        public object Id { get; }
    }
}