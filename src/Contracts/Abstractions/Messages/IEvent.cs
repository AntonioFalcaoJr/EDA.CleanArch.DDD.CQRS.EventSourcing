using MassTransit;
using MediatR;

namespace Contracts.Abstractions.Messages;

[ExcludeFromTopology]
public interface IEvent : IMessage, IRequest;

[ExcludeFromTopology]
public interface IDelayedEvent : IEvent;

[ExcludeFromTopology]
public interface IVersionedEvent : IEvent
{
    ulong Version { get; }
}

[ExcludeFromTopology]
public interface IDomainEvent : IVersionedEvent;

[ExcludeFromTopology]
public interface ISummaryEvent : IVersionedEvent;