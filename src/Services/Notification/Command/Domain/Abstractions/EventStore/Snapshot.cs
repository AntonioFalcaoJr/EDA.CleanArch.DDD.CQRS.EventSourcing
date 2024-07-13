using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Identities;
using Version = Domain.ValueObjects.Version;

namespace Domain.Abstractions.EventStore;

public record Snapshot<TAggregate, TId>(TId AggregateId, TAggregate Aggregate, Version Version, DateTimeOffset Timestamp)
    where TAggregate : IAggregateRoot<TId>
    where TId : IIdentifier, new()
{
    public static Snapshot<TAggregate, TId> Create(TAggregate aggregate, StoreEvent<TAggregate, TId> @event)
        => new(aggregate.Id, aggregate, @event.Version, @event.Timestamp);
}