using System;

namespace RJDev.Core.Domain.Entities
{
    public interface IAggregateRoot<out TKey> : IEntity<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
    }
}