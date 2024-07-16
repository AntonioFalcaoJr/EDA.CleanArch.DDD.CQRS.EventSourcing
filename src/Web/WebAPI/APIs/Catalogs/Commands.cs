using Contracts.Services.Cataloging.Command.Protobuf;
using WebAPI.Abstractions;
using WebAPI.APIs.Catalogs.Validators;
using static Contracts.Services.Cataloging.Command.Protobuf.CatalogingCommandService;

namespace WebAPI.APIs.Catalogs;

public static class Commands
{
    public record CreateCatalog(CatalogingCommandServiceClient Client, Payloads.CreateCatalog Payload, CancellationToken Token)
        : IVeryNewCommand<CatalogingCommandServiceClient, CreateCatalogValidator>
    {
        // TODO: This is a hack to get the app id. We need to find a better way to do this.
        public static implicit operator CreateCatalogCommand(CreateCatalog request)
            => new() { AppId = Guid.NewGuid().ToString(), Title = request.Payload.Title, Description = request.Payload.Description };
    }

    public record DeleteCatalog(CatalogingCommandServiceClient Client, string CatalogId, CancellationToken Token)
        : IVeryNewCommand<CatalogingCommandServiceClient, DeleteCatalogValidator>
    {
        public static implicit operator DeleteCatalogCommand(DeleteCatalog request) 
            => new() { CatalogId = request.CatalogId };
    }


    // public record AddCatalogItem(IBus Bus, Guid CatalogId, Payloads.AddCatalogItem Payload, Token Token)
    //     : Validatable<AddCatalogItemValidator>, ICommand<Command.AddCatalogItem>
    // {
    //     public Command.AddCatalogItem Command
    //         => new(CatalogId, Payload.InventoryId, Payload.Product, Payload.UnitPrice, Payload.Sku, Payload.Quantity);
    // }
    //
    // public record DeleteCatalog(IBus Bus, Guid CatalogId, Token Token)
    //     : Validatable<DeleteCatalogValidator>, ICommand<Command.DeleteCatalog>
    // {
    //     public Command.DeleteCatalog Command => new(CatalogId);
    // }
    //
    // public record ActivateCatalog(IBus Bus, Guid CatalogId, Token Token)
    //     : Validatable<ActivateCatalogValidator>, ICommand<Command.ActivateCatalog>
    // {
    //     public Command.ActivateCatalog Command => new(CatalogId);
    // }
    //
    // public record DeactivateCatalog(IBus Bus, Guid CatalogId, Token Token)
    //     : Validatable<DeactivateCatalogValidator>, ICommand<Command.InactivateCatalog>
    // {
    //     public Command.InactivateCatalog Command => new(CatalogId);
    // }
    //
    // public record ChangeCatalogDescription(IBus Bus, Guid CatalogId, string Description, Token Token)
    //     : Validatable<ChangeCatalogDescriptionValidator>, ICommand<Command.ChangeCatalogDescription>
    // {
    //     public Command.ChangeCatalogDescription Command => new(CatalogId, Description);
    // }
    //
    // public record ChangeCatalogTitle(IBus Bus, Guid CatalogId, string Title, Token Token)
    //     : Validatable<ChangeCatalogTitleValidator>, ICommand<Command.ChangeCatalogTitle>
    // {
    //     public Command.ChangeCatalogTitle Command => new(CatalogId, Title);
    // }
    //
    // public record RemoveCatalogItem(IBus Bus, Guid CatalogId, Guid ItemId, Token Token)
    //     : Validatable<RemoveCatalogItemValidator>, ICommand<Command.RemoveCatalogItem>
    // {
    //     public Command.RemoveCatalogItem Command => new(CatalogId, ItemId);
    // }
}