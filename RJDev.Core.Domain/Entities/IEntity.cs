using System;
using System.Collections.Generic;
using RJDev.Core.Domain.Events;

namespace RJDev.Core.Domain.Entities
{
    public interface IEntity<out TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Entity identifier.
        /// </summary>
        TKey Id { get; }
        
        /// <summary>
        /// Collection of raised events.
        /// </summary>
        IReadOnlyCollection<IEvent> Events { get; }
    }
}