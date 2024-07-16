using Contracts.Abstractions.Messages;
using Contracts.Abstractions.Paging;
using Contracts.Services.Cataloging.Query.Protobuf;

namespace Contracts.Boundaries.Cataloging.Catalog;

public static class Query
{
    public record ListCatalogsGridItems(Paging Paging) : IQuery
    {
        public static implicit operator ListCatalogsGridItems(ListCatalogsGridItemsRequest request)
            => new(request.Paging);
    }

    public record ListCatalogItemsListItems(string CatalogId, Paging Paging) : IQuery
    {
        public static implicit operator ListCatalogItemsListItems(ListCatalogItemsListItemsRequest request)
            => new(new(request.CatalogId), request.Paging);
    }

    public record ListCatalogItemsCards(string CatalogId, Paging Paging) : IQuery
    {
        public static implicit operator ListCatalogItemsCards(ListCatalogItemsCardsRequest request)
            => new(new(request.CatalogId), request.Paging);
    }

    public record GetCatalogItemDetails(string CatalogId, string ItemId) : IQuery
    {
        public static implicit operator GetCatalogItemDetails(GetCatalogItemDetailsRequest request)
            => new(new(request.CatalogId), new(request.ItemId));
    }
}