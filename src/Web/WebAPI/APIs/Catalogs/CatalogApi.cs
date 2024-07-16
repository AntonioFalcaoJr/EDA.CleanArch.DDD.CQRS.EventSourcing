using Asp.Versioning.Builder;
using WebAPI.Abstractions;

namespace WebAPI.APIs.Catalogs;

public static class CatalogApi
{
    private const string BaseUrl = "/api/v{version:apiVersion}/catalogs/";

    public static IVersionedEndpointRouteBuilder MapCatalogApiV1(this IVersionedEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(BaseUrl).HasApiVersion(1);

        // group.MapPost("/start-shopping/", ([AsParameters] Commands.StartShopping2 startShopping)
        //     => ApplicationApi.SendAsync<ShoppingCommandServiceClient, StartShoppingPayloadValidator, CommandResponse>
        //         (startShopping, (client, ct) => client.StartShoppingAsync(startShopping, cancellationToken: ct)));

        group.MapPost("/", ([AsParameters] Commands.CreateCatalog createCatalog)
            => ApplicationApi.SendAsync(createCatalog, (client, token)
                => client.CreateCatalogAsync(createCatalog, cancellationToken: token)));
        
        group.MapDelete("/{catalogId}", ([AsParameters] Commands.DeleteCatalog deleteCatalog)
            => ApplicationApi.SendAsync(deleteCatalog, (client, token) 
                => client.DeleteCatalogAsync(deleteCatalog, cancellationToken: token)));


        // group.MapGet("/grid-items", ([AsParameters] Queries.ListCatalogsGridItems query)
        //     => ApplicationApi.ListAsync<CatalogService.CatalogServiceClient, CatalogGridItem>
        //         (query, (client, ct) => client.ListCatalogsGridItemsAsync(query, cancellationToken: ct)));
        //
        // group.MapDelete("/{catalogId:guid}", ([AsParameters] Commands.DeleteCatalog deleteCatalog)
        //     => ApplicationApi.SendCommandAsync(deleteCatalog));
        //
        // group.MapPut("/{catalogId:guid}/activate", ([AsParameters] Commands.ActivateCatalog activateCatalog)
        //     => ApplicationApi.SendCommandAsync(activateCatalog));
        //
        // group.MapPut("/{catalogId:guid}/deactivate", ([AsParameters] Commands.DeactivateCatalog deactivateCatalog)
        //     => ApplicationApi.SendCommandAsync(deactivateCatalog));
        //
        // group.MapPut("/{catalogId:guid}/description", ([AsParameters] Commands.ChangeCatalogDescription changeCatalogDescription)
        //     => ApplicationApi.SendCommandAsync(changeCatalogDescription));
        //
        // group.MapPut("/{catalogId:guid}/title", ([AsParameters] Commands.ChangeCatalogTitle changeCatalogTitle)
        //     => ApplicationApi.SendCommandAsync(changeCatalogTitle));
        //
        // group.MapPost("/{catalogId:guid}/items", ([AsParameters] Commands.AddCatalogItem addCatalogItem)
        //     => ApplicationApi.SendCommandAsync(addCatalogItem));
        //
        // group.MapGet("/{catalogId:guid}/items/list-items", ([AsParameters] Queries.ListCatalogItemsListItems query)
        //     => ApplicationApi.ListAsync<CatalogService.CatalogServiceClient, CatalogItemListItem>
        //         (query, (client, ct) => client.ListCatalogItemsListItemsAsync(query, cancellationToken: ct)));
        //
        // group.MapGet("/{catalogId:guid}/items/cards", ([AsParameters] Queries.ListCatalogItemsCards query)
        //     => ApplicationApi.ListAsync<CatalogService.CatalogServiceClient, CatalogItemCard>
        //         (query, (client, ct) => client.ListCatalogItemsCardsAsync(query, cancellationToken: ct)));
        //
        // group.MapGet("/{catalogId:guid}/items/{itemId:guid}/details", ([AsParameters] Queries.GetCatalogItemDetails query)
        //     => ApplicationApi.GetAsync<CatalogService.CatalogServiceClient, CatalogItemDetails>
        //         (query, (client, ct) => client.GetCatalogItemDetailsAsync(query, cancellationToken: ct)));
        //
        // group.MapDelete("/{catalogId:guid}/items/{itemId:guid}", ([AsParameters] Commands.RemoveCatalogItem removeCatalogItem)
        //     => ApplicationApi.SendCommandAsync(removeCatalogItem));

        return builder;
    }

    public static IVersionedEndpointRouteBuilder MapCatalogApiV2(this IVersionedEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(BaseUrl).HasApiVersion(2);

        // group.MapGet("/grid-items", ([AsParameters] Queries.ListCatalogsGridItems query)
        //     => ApplicationApi.ListAsync<CatalogService.CatalogServiceClient, CatalogGridItem>
        //         (query, (client, ct) => client.ListCatalogsGridItemsAsync(query, cancellationToken: ct)));

        return builder;
    }
}