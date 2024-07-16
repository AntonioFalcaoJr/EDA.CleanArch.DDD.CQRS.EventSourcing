using Application.Abstractions;
using Contracts.Abstractions.Messages;
using Domain.Abstractions.Aggregates;
using Domain.Abstractions.EventStore;
using Domain.Abstractions.Identities;
using static Domain.Exceptions;
using Version = Domain.ValueObjects.Version;

namespace Application.Services;

public class ApplicationService(IEventStoreGateway eventStore, IEventBusGateway eventBus, IUnitOfWork unitOfWork) : IApplicationService
{
    public async Task<TAggregate> LoadAggregateAsync<TAggregate, TId>(TId id, CancellationToken token)
        where TAggregate : class, IAggregateRoot<TId>, new()
        where TId : IIdentifier, new()
    {
        var snapshot = await eventStore.GetSnapshotAsync<TAggregate, TId>(id, token);
        var events = await eventStore.GetStreamAsync<TAggregate, TId>(id, snapshot?.Version ?? Version.Zero, token);

        AggregateNotFound.ThrowIf(snapshot is null && events.Count is 0);

        var aggregate = snapshot?.Aggregate ?? new();
        aggregate.LoadFromStream(events);

        AggregateIsDeleted.ThrowIf(aggregate.IsDeleted);

        return aggregate;
    }

    public Task AppendEventsAsync<TAggregate, TId>(TAggregate aggregate, CancellationToken token)
        where TAggregate : IAggregateRoot<TId>
        where TId : IIdentifier, new()
        => unitOfWork.ExecuteAsync(operationAsync: async ct =>
            {
                while (aggregate.TryDequeueEvent(out var @event))
                {
                    var storeEvent = StoreEvent<TAggregate, TId>.Create(aggregate, @event);
                    await eventStore.AppendAsync(storeEvent, ct);

                    if (storeEvent.Version % Version.Number(5))
                    {
                        var snapshot = Snapshot<TAggregate, TId>.Create(aggregate, storeEvent);
                        await eventStore.AppendAsync(snapshot, ct);
                    }

                    await eventBus.PublishAsync(@event, ct);
                }
            },
            cancellationToken: token);

    public IAsyncEnumerable<TId> StreamAggregatesId<TAggregate, TId>()
        where TAggregate : IAggregateRoot<TId>
        where TId : IIdentifier, new() 
        => eventStore.StreamAggregatesId<TAggregate, TId>();

    public Task PublishEventAsync(IEvent @event, CancellationToken cancellationToken) 
        => eventBus.PublishAsync(@event, cancellationToken);

    public Task SchedulePublishAsync(IDelayedEvent @event, DateTimeOffset scheduledTime, CancellationToken cancellationToken) 
        => eventBus.SchedulePublishAsync(@event, scheduledTime, cancellationToken);
}