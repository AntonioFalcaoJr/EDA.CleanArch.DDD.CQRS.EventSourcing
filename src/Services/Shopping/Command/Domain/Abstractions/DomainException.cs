using Contracts.Abstractions.Messages;

namespace Domain.Abstractions;

public interface IDomainException;

public abstract class DomainException<TException>(string message) : InvalidOperationException(message), IDomainException
    where TException : DomainException<TException>, new()
{
    public static TException New() => new();

    public static void ThrowIf(bool condition)
    {
        if (condition) throw new TException();
    }

    public static void ThrowIfNull<T>(T t)
    {
        if (t is null) throw new TException();
    }

    public static IDomainEvent Throw() => throw new TException();
}