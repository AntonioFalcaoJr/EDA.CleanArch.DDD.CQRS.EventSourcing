using Contracts.Abstractions.Messages;
using Domain.Abstractions.Entities;
using Domain.Abstractions.Identities;
using static Domain.Exceptions;
using Version = Domain.ValueObjects.Version;

namespace Domain.Abstractions.Aggregates;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId> 
    where TId : IIdentifier, new()
{
    private readonly Queue<IDomainEvent> _events = new();
    public Version Version { get; protected set; } = Version.Zero;

    public void LoadFromStream(List<IDomainEvent> events) => events.ForEach(ApplyEvent);
    public bool TryDequeueEvent(out IDomainEvent @event) => _events.TryDequeue(out @event!);
    private void EnqueueEvent(IDomainEvent @event) => _events.Enqueue(@event);

    protected void RaiseEvent(IDomainEvent @event)
    {
        AggregateIsDeleted.ThrowIf(IsDeleted);
        ApplyEvent(@event);
        EnqueueEvent(@event);
    }

    protected abstract void ApplyEvent(IDomainEvent @event);
}