namespace WebAPI.APIs.Catalogs;

public static class Queries
{
    // public record ListCatalogsGridItems(CatalogService.CatalogServiceClient Client, int? Size, int? Number, Token Token)
    //     : Validatable<ListCatalogsGridItemsValidator>, IQuery<CatalogService.CatalogServiceClient>
    // {
    //     public static implicit operator ListCatalogsGridItemsRequest(ListCatalogsGridItems request)
    //         => new() { Paging = new() { Size = request.Size, Number = request.Number } };
    // }
    //
    // public record ListCatalogItemsListItems(CatalogService.CatalogServiceClient Client, Guid CatalogId, int? Size, int? Number, Token Token)
    //     : Validatable<ListCatalogItemsListItemsRequestValidator>, IQuery<CatalogService.CatalogServiceClient>
    // {
    //     public static implicit operator ListCatalogItemsListItemsRequest(ListCatalogItemsListItems request)
    //         => new() { CatalogId = request.CatalogId.ToString(), Paging = new() { Size = request.Size, Number = request.Number } };
    // }
    //
    // public record ListCatalogItemsCards(CatalogService.CatalogServiceClient Client, Guid CatalogId, int? Size, int? Number, Token Token)
    //     : Validatable<ListCatalogItemsCardsValidator>, IQuery<CatalogService.CatalogServiceClient>
    // {
    //     public static implicit operator ListCatalogItemsCardsRequest(ListCatalogItemsCards request)
    //         => new() { CatalogId = request.CatalogId.ToString(), Paging = new() { Size = request.Size, Number = request.Number } };
    // }
    //
    // public record GetCatalogItemDetails(CatalogService.CatalogServiceClient Client, Guid CatalogId, Guid ItemId, Token Token)
    //     : Validatable<GetCatalogItemDetailsValidator>, IQuery<CatalogService.CatalogServiceClient>
    // {
    //     public static implicit operator GetCatalogItemDetailsRequest(GetCatalogItemDetails request)
    //         => new() { CatalogId = request.CatalogId.ToString(), ItemId = request.ItemId.ToString() };
    // }
}