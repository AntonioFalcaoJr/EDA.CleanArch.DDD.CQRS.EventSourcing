using Contracts.Abstractions.Messages;
using Contracts.Abstractions.Protobuf;
using FluentValidation;
using Grpc.Core;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.AspNetCore.Http.TypedResults;
using NoContent = Microsoft.AspNetCore.Http.HttpResults.NoContent;
using NotFound = Microsoft.AspNetCore.Http.HttpResults.NotFound;
using Accepted = Microsoft.AspNetCore.Http.HttpResults.Accepted;
using Ok = Microsoft.AspNetCore.Http.HttpResults.Ok;
using IMessage = Google.Protobuf.IMessage;

namespace WebAPI.Abstractions;

public static class ApplicationApi
{
    public static async Task<Results<Accepted, ValidationProblem>> SendCommandAsync<TCommand>(ICommand<TCommand> request)
        where TCommand : class, ICommand
    {
        return request.IsValid(out var errors) ? await OnSendAsync() : ValidationProblem(errors);

        async Task<Accepted> OnSendAsync()
        {
            var endpoint = await request.Bus.GetSendEndpoint(Address<TCommand>());
            await endpoint.Send<TCommand>(request, request.CancellationToken);
            return Accepted("");
        }
    }

    public static async Task<Results<Ok, Created<Identifier>, NotFound, ValidationProblem, Problem>> SendAsync<TClient, TValidator>
        (IVeryNewCommand<TClient, TValidator> command, Func<TClient, CancellationToken, AsyncUnaryCall<CommandResponse>> sendAsync)
        where TClient : ClientBase
        where TValidator : IValidator, new()
    {
        return command.IsValid(out var errors) ? await OnSendAsync() : ValidationProblem(errors);

        async Task<Results<Ok, Created<Identifier>, NotFound, ValidationProblem, Problem>> OnSendAsync()
        {
            var response = await sendAsync(command.Client, command.Token);

            return response.OneOfCase switch
            {
                CommandResponse.OneOfOneofCase.Ok => Ok(),
                CommandResponse.OneOfOneofCase.Created => Created("", response.Created.Id),
                CommandResponse.OneOfOneofCase.NotFound => NotFound(),
                _ => new Problem("Unexpected response")
            };
        }
    }

    public static async Task<Results<Ok<TResponse>, NotFound, ValidationProblem, Problem>> GetAsync<TClient, TResponse>
        (IQuery<TClient> query, Func<TClient, CancellationToken, AsyncUnaryCall<GetResponse>> getAsync)
        where TClient : ClientBase<TClient>
        where TResponse : IMessage, new()
    {
        return query.IsValid(out var errors) ? await OnGetAsync() : ValidationProblem(errors);

        async Task<Results<Ok<TResponse>, NotFound, ValidationProblem, Problem>> OnGetAsync()
        {
            var response = await getAsync(query.Client, query.CancellationToken);

            return response.OneOfCase switch
            {
                GetResponse.OneOfOneofCase.NotFound => NotFound(),
                GetResponse.OneOfOneofCase.Projection when response.Projection.TryUnpack<TResponse>(out var result) => Ok(result),
                _ => new Problem("Unexpected response")
            };
        }
    }

    public static async Task<Results<Ok<PagedResult<TResponse>>, NoContent, ValidationProblem, Problem>> ListAsync<TClient, TResponse>
        (IQuery<TClient> query, Func<TClient, CancellationToken, AsyncUnaryCall<ListResponse>> listAsync)
        where TClient : ClientBase<TClient>
        where TResponse : IMessage, new()
    {
        return query.IsValid(out var errors) ? await OnListAsync() : ValidationProblem(errors);

        async Task<Results<Ok<PagedResult<TResponse>>, NoContent, ValidationProblem, Problem>> OnListAsync()
        {
            var response = await listAsync(query.Client, query.CancellationToken);

            return response.OneOfCase switch
            {
                ListResponse.OneOfOneofCase.NoContent => NoContent(),
                ListResponse.OneOfOneofCase.PagedResult => Ok<PagedResult<TResponse>>(response.PagedResult),
                _ => new Problem("Unexpected response")
            };
        }
    }

    private static Uri Address<T>()
        => new($"exchange:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(typeof(T).Name)}");
}