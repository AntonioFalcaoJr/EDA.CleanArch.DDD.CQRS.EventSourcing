using Application.Abstractions;
using Contracts.Abstractions.Protobuf;
using Contracts.Boundaries.Cataloging.Catalog;
using Contracts.Services.Cataloging.Query.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GrpcService;

public class CatalogGrpcService(IInteractor<Query.GetCatalogItemDetails, Projection.CatalogItemDetails> getCatalogItemDetailsInteractor,
        IPagedInteractor<Query.ListCatalogItemsCards, Projection.CatalogItemCard> listCatalogItemsCardsInteractor,
        IPagedInteractor<Query.ListCatalogsGridItems, Projection.CatalogGridItem> listCatalogsGridItemsInteractor,
        IPagedInteractor<Query.ListCatalogItemsListItems, Projection.CatalogItemListItem> listCatalogItemsListItemsInteractor)
    : CatalogingQueryService.CatalogingQueryServiceBase
{
    public override async Task<GetResponse> GetCatalogItemDetails(GetCatalogItemDetailsRequest request, ServerCallContext context)
    {
        var itemDetails = await getCatalogItemDetailsInteractor.InteractAsync(request, context.CancellationToken);

        return itemDetails is null
            ? new() { NotFound = new() }
            : new() { Projection = Any.Pack((CatalogItemDetails)itemDetails) };
    }

    public override async Task<ListResponse> ListCatalogsGridItems(ListCatalogsGridItemsRequest request, ServerCallContext context)
    {
        var pagedResult = await listCatalogsGridItemsInteractor.InteractAsync(request, context.CancellationToken);

        return pagedResult.Items.Any()
            ? new()
            {
                PagedResult = new()
                {
                    Projections = { pagedResult.Items.Select(item => Any.Pack((CatalogGridItem)item)) },
                    Page = pagedResult.Page
                }
            }
            : new() { NoContent = new() };
    }

    public override async Task<ListResponse> ListCatalogItemsListItems(ListCatalogItemsListItemsRequest request, ServerCallContext context)
    {
        var pagedResult = await listCatalogItemsListItemsInteractor.InteractAsync(request, context.CancellationToken);

        return pagedResult.Items.Any()
            ? new()
            {
                PagedResult = new()
                {
                    Projections = { pagedResult.Items.Select(item => Any.Pack((CatalogItemListItem)item)) },
                    Page = pagedResult.Page
                }
            }
            : new() { NoContent = new() };
    }

    public override async Task<ListResponse> ListCatalogItemsCards(ListCatalogItemsCardsRequest request, ServerCallContext context)
    {
        var pagedResult = await listCatalogItemsCardsInteractor.InteractAsync(request, context.CancellationToken);

        return pagedResult.Items.Any()
            ? new()
            {
                PagedResult = new()
                {
                    Projections = { pagedResult.Items.Select(item => Any.Pack((CatalogItemCard)item)) },
                    Page = pagedResult.Page
                }
            }
            : new() { NoContent = new() };
    }
}