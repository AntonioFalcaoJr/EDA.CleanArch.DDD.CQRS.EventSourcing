using Contracts.Abstractions.Messages;
using Domain.Abstractions.Entities;

namespace Domain.Abstractions.Aggregates;

public interface IAggregateRoot : IEntity
{
    string Name => GetType().Name;
    long Version { get; }
    IEnumerable<IEvent> Events { get; }
    IAggregateRoot Load(List<IEvent> events);
    void Handle(ICommandWithId command);
}