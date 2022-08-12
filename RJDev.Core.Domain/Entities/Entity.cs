using System;
using System.Collections.Generic;
using RJDev.Core.Domain.Events;

namespace RJDev.Core.Domain.Entities
{
    public abstract class EntityBase<TKey> : IEntity<TKey> 
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly List<IEvent> events = new();

        /// <inheritdoc />
        public abstract TKey Id { get; }

        /// <summary>
        /// Collection of domain and integration events.
        /// </summary>
        public IReadOnlyCollection<IEvent> Events => this.events.AsReadOnly();

        /// <summary>
        /// Raise domain or integration event.
        /// </summary>
        /// <param name="event"></param>
        protected void Raise(IEvent @event)
        {
            events.Add(@event);
        }
    }
}