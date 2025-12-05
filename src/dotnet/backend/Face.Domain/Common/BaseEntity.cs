using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Face.Domain.Common
{
    /// <summary>
    /// Base class for all aggregate roots / entities.
    /// Domain events are kept purely in memory and are explicitly
    /// excluded from EF Core mapping.
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        [NotMapped]
        private readonly List<IDomainEvent> _domainEvents = new();

        [NotMapped]
        public IReadOnlyCollection<IDomainEvent> DomainEvents =>
            new ReadOnlyCollection<IDomainEvent>(_domainEvents);

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
        _domainEvents.Clear();
        }
    }
}
