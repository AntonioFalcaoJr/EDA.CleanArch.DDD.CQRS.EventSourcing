﻿using Application.Abstractions.EventStore;
using Application.Abstractions.Notifications;
using Contracts.Abstractions.Messages;
using Domain.Abstractions.Aggregates;
using Domain.Abstractions.StoreEvents;
using Infrastructure.EventStore.DependencyInjection.Options;
using Infrastructure.EventStore.UnitsOfWork;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Infrastructure.EventStore.Abstractions;

public abstract class EventStoreService<TAggregate, TStoreEvent, TSnapshot> : IEventStoreService<TAggregate>
    where TAggregate : IAggregateRoot, new()
    where TStoreEvent : StoreEvent<TAggregate>, new()
    where TSnapshot : Snapshot<TAggregate>, new()
{
    private readonly INotificationContext _notificationContext;
    private readonly EventStoreOptions _options;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IEventStoreRepository<TAggregate, TStoreEvent, TSnapshot> _repository;
    private readonly IUnitOfWork _unitOfWork;

    protected EventStoreService(
        IPublishEndpoint publishEndpoint,
        IEventStoreRepository<TAggregate, TStoreEvent, TSnapshot> repository,
        INotificationContext notificationContext,
        IOptionsSnapshot<EventStoreOptions> options,
        IUnitOfWork unitOfWork)
    {
        _publishEndpoint = publishEndpoint;
        _notificationContext = notificationContext;
        _unitOfWork = unitOfWork;
        _options = options.Value;
        _repository = repository;
    }

    public async Task AppendAsync(TAggregate aggregate, CancellationToken cancellationToken)
    {
        if (await aggregate.IsValidAsync) await OnAppendEventsAsync(aggregate, cancellationToken);
        else _notificationContext.AddErrors(aggregate.Errors);
    }

    public async Task<TAggregate> LoadAsync(Guid aggregateId, CancellationToken ct)
    {
        var snapshot = await _repository.GetSnapshotAsync(aggregateId, ct) ?? new();
        var events = await _repository.GetStreamAsync(aggregateId, snapshot.AggregateVersion, ct);
        snapshot.AggregateState.LoadEvents(events);
        return snapshot.AggregateState;
    }

    private Task OnAppendEventsAsync(TAggregate aggregate, CancellationToken cancellationToken)
        => _unitOfWork.ExecuteAsync(
            operationAsync: async ct =>
            {
                await AppendEventsWithSnapshotControlAsync(aggregate, ct);
                await PublishEventsAsync(aggregate.Events, ct);
            },
            cancellationToken: cancellationToken);

    private Task AppendEventsWithSnapshotControlAsync(TAggregate aggregate, CancellationToken cancellationToken)
        => _repository.AppendEventsAsync(
            events: ToStoreEvents(aggregate),
            onEventStored: (version, ct) => AppendSnapshotAsync(aggregate, version, ct),
            cancellationToken: cancellationToken);

    private async Task AppendSnapshotAsync(TAggregate aggregate, long version, CancellationToken cancellationToken)
    {
        if (version % _options.SnapshotInterval is not 0) return;

        TSnapshot snapshot = new()
        {
            AggregateId = aggregate.Id,
            AggregateState = aggregate,
            AggregateVersion = version
        };

        await _repository.AppendSnapshotAsync(snapshot, cancellationToken);
    }

    private Task PublishEventsAsync(IEnumerable<IEvent> events, CancellationToken ct)
        => Task.WhenAll(events.Select(@event => _publishEndpoint.Publish(@event, @event.GetType(), ct)));

    private static IEnumerable<TStoreEvent> ToStoreEvents(TAggregate aggregate)
        => aggregate.Events.Select(@event
            => new TStoreEvent
            {
                Version = aggregate.Version,
                AggregateId = aggregate.Id,
                DomainEvent = @event,
                DomainEventName = @event.GetType().Name
            });
}