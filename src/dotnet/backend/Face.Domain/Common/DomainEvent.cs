using System;
using MediatR;

namespace Face.Domain.Common
{
    /// <summary>
    /// Marker interface for all domain events in the domain model.
    /// This is used to keep domain events out of EF Core's entity model.
    /// </summary>
    public interface IDomainEvent : INotification
    {
    }

    /// <summary>
    /// Base type for domain events.
    /// Note: EF Core should NOT treat this as an entity.
    /// </summary>
    public abstract class DomainEvent : IDomainEvent
    {
        public Guid Id { get; } = Guid.NewGuid();

        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
