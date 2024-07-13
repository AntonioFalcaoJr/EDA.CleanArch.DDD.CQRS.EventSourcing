using Contracts.Abstractions.Messages;
using Domain.Abstractions.Entities;
using Domain.Abstractions.Identities;
using Version = Domain.ValueObjects.Version;

namespace Domain.Abstractions.Aggregates;

public interface IAggregateRoot<out TId> : IEntity<TId>
    where TId : IIdentifier, new()
{
    Version Version { get; }
    void LoadFromStream(List<IDomainEvent> events);
    bool TryDequeueEvent(out IDomainEvent @event);
}