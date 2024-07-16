using Grpc.Core;
using MassTransit;

namespace WebAPI.Abstractions;

public interface IRequest
{
    CancellationToken CancellationToken { get; }
    bool IsValid(out IDictionary<string, string[]> errors);
}

public interface INewRequest
{
    CancellationToken Token { get; }
}

public interface ICommand<out TCommand> : IRequest
    where TCommand : class
{
    TCommand Command { get; }
    IBus Bus { get; }
}

public interface INewCommand<out TClient> : INewRequest
    where TClient : ClientBase
{
    TClient Client { get; }
}

public interface IQuery<out TClient> : IRequest
    where TClient : ClientBase
{
    TClient Client { get; }
}