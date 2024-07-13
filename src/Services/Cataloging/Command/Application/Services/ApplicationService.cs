using System.Linq.Expressions;
using Application.Abstractions;
using Contracts.Abstractions.Messages;
using Domain.Abstractions.Aggregates;
using Domain.Abstractions.EventStore;
using Domain.Abstractions.Identities;
using Version = Domain.ValueObjects.Version;

namespace Application.Services;

public class ApplicationService(
    IEventStoreGateway eventStore,
    EventStoreOptions options,
    IEventBusGateway eventBus,
    IUnitOfWork unitOfWork)
    : IApplicationService
{
    public async Task<TAggregate> LoadAggregateAsync<TAggregate, TId>(TId id, CancellationToken cancellationToken)
        where TAggregate : class, IAggregateRoot<TId>, new()
        where TId : IIdentifier, new()
    {
        var snapshot = await eventStore.GetSnapshotAsync<TAggregate, TId>(id, cancellationToken);
        var events = await eventStore.GetStreamAsync<TAggregate, TId>(id, snapshot?.Version ?? Version.Zero, cancellationToken);
        return LoadAggregateAsync(snapshot, events);
    }
    
    public async Task<TAggregate> LoadAggregateAsync<TAggregate, TId>(Expression<Func<TAggregate, bool>> predicate, CancellationToken cancellationToken)
        where TAggregate : class, IAggregateRoot<TId>, new()
        where TId : IIdentifier, new()
    {
        var snapshotPredicate = BuildExpression<TAggregate, Snapshot<TAggregate, TId>, bool>(predicate);
        var storeEventPredicate = BuildExpression<TAggregate, StoreEvent<TAggregate, TId>, bool>(predicate);

        var snapshot = await eventStore.GetSnapshotAsync(snapshotPredicate, cancellationToken);
        var events = await eventStore.GetStreamAsync(storeEventPredicate, snapshot?.Version ?? Version.Zero, cancellationToken);

        return LoadAggregateAsync(snapshot, events);
    }
    
    public Task ExecuteAsync(Func<CancellationToken, Task> operationAsync, CancellationToken cancellationToken) 
        => unitOfWork.ExecuteAsync(operationAsync, cancellationToken);

    public Task AppendEventsAsync<TAggregate, TId>(TAggregate aggregate, CancellationToken token)
        where TAggregate : IAggregateRoot<TId>
        where TId : IIdentifier, new()
        => unitOfWork.ExecuteAsync(operationAsync: async ct =>
            {
                while (aggregate.TryDequeueEvent(out var @event))
                {
                    var storeEvent = StoreEvent<TAggregate, TId>.Create(aggregate, @event);
                    await eventStore.AppendAsync(storeEvent, ct);

                    if (storeEvent.Version % options.SnapshotInterval)
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

    private static TAggregate LoadAggregateAsync<TAggregate, TId>(Snapshot<TAggregate, TId>? snapshot, List<IDomainEvent> events)
        where TAggregate : class, IAggregateRoot<TId>, new()
        where TId : IIdentifier, new()
    {
        if (snapshot is null && events is { Count: 0 })
            throw new InvalidOperationException($"Aggregate {typeof(TAggregate).Name} not found.");

        var aggregate = snapshot?.Aggregate ?? new();
        aggregate.LoadFromStream(events);

        return aggregate is { IsDeleted: false }
            ? aggregate
            : throw new InvalidOperationException($"Aggregate {typeof(TAggregate).Name} is deleted.");
    }
    
    private static Expression<Func<TNewEntity, TResult>> BuildExpression<TOldEntity, TNewEntity, TResult>(Expression<Func<TOldEntity, TResult>> oldExpression)
    {
        var newParameter = Expression.Parameter(typeof(TNewEntity), "entity");
        var newExpressionBody = Expression.Invoke(oldExpression, Expression.Convert(newParameter, typeof(TOldEntity)));
        return Expression.Lambda<Func<TNewEntity, TResult>>(newExpressionBody, newParameter);
    }
}

public record EventStoreOptions
{
    public Version SnapshotInterval { get; init; }
}