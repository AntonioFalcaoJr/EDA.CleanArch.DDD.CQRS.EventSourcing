using Application.UseCases.Catalogs.Commands;
using Contracts.Abstractions.Protobuf;
using Contracts.Services.Cataloging.Command.Protobuf;
using Domain.Aggregates;
using Domain.Aggregates.Catalogs;
using Domain.ValueObjects;
using Grpc.Core;
using MediatR;

namespace GrpcService;

public class CatalogingGrpcCommandService(ISender sender) : CatalogingCommandService.CatalogingCommandServiceBase
{
    public override async Task<CommandResponse> CreateCatalog(CreateCatalogCommand cmd, ServerCallContext context)
    {
        // TODO: Get AppId from context
        var appId = AppId.Undefined;

        CreateCatalog create = new(appId, (Title)cmd.Title, (Description)cmd.Description /*, cmd.ImageUrl*/);
        var catalogId = await sender.Send(create, context.CancellationToken);
        return Response.Created(catalogId);
    }

    public override async Task<CommandResponse> DeleteCatalog(DeleteCatalogCommand cmd, ServerCallContext context)
    {
        DeleteCatalog delete = new((CatalogId)cmd.CatalogId);
        await sender.Send(delete, context.CancellationToken);
        return Response.Accepted();
    }

    public override async Task<CommandResponse> ActivateCatalog(ActivateCatalogCommand cmd, ServerCallContext context)
    {
        ActivateCatalog activate = new((CatalogId)cmd.CatalogId);
        await sender.Send(activate, context.CancellationToken);
        return Response.Accepted();
    }

    public override async Task<CommandResponse> DeactivateCatalog(DeactivateCatalogCommand cmd, ServerCallContext context)
    {
        DeactivateCatalog deactivate = new((CatalogId)cmd.CatalogId);
        await sender.Send(deactivate, context.CancellationToken);
        return Response.Accepted();
    }

    public override async Task<CommandResponse> ChangeCatalogTitle(ChangeCatalogTitleCommand cmd, ServerCallContext context)
    {
        ChangeCatalogTitle changeTitle = new((CatalogId)cmd.CatalogId, (Title)cmd.Title);
        await sender.Send(changeTitle, context.CancellationToken);
        return Response.Accepted();
    }

    public override async Task<CommandResponse> ChangeCatalogDescription(ChangeCatalogDescriptionCommand cmd, ServerCallContext context)
    {
        ChangeCatalogDescription changeDescription = new((CatalogId)cmd.CatalogId, (Description)cmd.Description);
        await sender.Send(changeDescription, context.CancellationToken);
        return Response.Accepted();
    }
}

public static class Response
{
    public static CommandResponse Accepted() => new() { Accepted = new() };
    public static CommandResponse Created(string id) => new() { Created = new() { Id = new() { Id = id } } };
    public static CommandResponse NoContent() => new() { NoContent = new() };
    public static CommandResponse NotFound() => new() { NotFound = new() };
    public static CommandResponse Ok() => new() { Ok = new() };
}