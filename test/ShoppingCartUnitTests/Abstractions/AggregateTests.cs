using Contracts.Abstractions.Messages;
using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Identities;
using FluentAssertions;

namespace ShoppingCartUnitTests.Abstractions;

public abstract class AggregateTests<TAggregate, TId>
    where TAggregate : IAggregateRoot<TId>, new()
    where TId : IIdentifier, new()
{
    protected TAggregate Aggregate = new();
    private Action<TAggregate> _command = _ => { };

    protected AggregateTests<TAggregate, TId> Given(params IDomainEvent[] stream)
    {
        Aggregate.LoadFromStream(stream.ToList());
        return this;
    }

    public AggregateTests<TAggregate, TId> When(TAggregate aggregate)
    {
        Aggregate = aggregate;
        return this;
    }

    public AggregateTests<TAggregate, TId> When(Action<TAggregate> command)
    {
        _command = command;
        return this;
    }

    public void Then<TEvent>(params Action<TEvent>[] assertions) where TEvent : class, IDomainEvent
    {
        _command(Aggregate);
        Aggregate.TryDequeueEvent(out var @event).Should().BeTrue();
        @event.Should().BeOfType<TEvent>();
        foreach (var assertion in assertions) assertion(@event.As<TEvent>());
    }

    public void ThenNothing()
    {
        _command(Aggregate);
        Aggregate.TryDequeueEvent(out _).Should().BeFalse();
    }

    public void ThenThrows<TException>(params Action<TException>[] assertions) where TException : Exception
    {
        var command = () => _command(Aggregate);
        var @throw = command.Should().Throw<TException>();
        var exception = @throw.Subject.First();
        foreach (var assertion in assertions) assertion(exception);
    }
}