using Fluxor;
using Refit;
using WebAPP.Abstractions;
using WebAPP.Store.Catalogs.Events;

namespace WebAPP.Store.Catalogs.Queries;

public record ListCatalogItems(string CatalogId, Paging Paging, CancellationToken CancellationToken);

public class ListCatalogItemsReducer : Reducer<CatalogState, ListCatalogItems>
{
    public override CatalogState Reduce(CatalogState state, ListCatalogItems action)
        => state with { IsFetching = true, HasError = false };
}

public interface IListCatalogItemsApi
{
    [Get("/v1/catalogs/{catalogId}/items")]
    Task<IApiResponse<IPagedResult<CatalogItem>>> ListItemsAsync(string catalogId, [Query] Paging paging, CancellationToken cancellationToken);
}

public class ListCatalogItemsEffect(IListCatalogItemsApi api) : Effect<ListCatalogItems>
{
    public override async Task HandleAsync(ListCatalogItems query, IDispatcher dispatcher)
    {
        var response = await api.ListItemsAsync(query.CatalogId, query.Paging, query.CancellationToken);

        dispatcher.Dispatch(response is { IsSuccessStatusCode: true, Content: not null }
            ? new CatalogItemsListed(response.Content)
            : new CatalogItemsListingFailed(response.Error?.Message ?? "Unknown error"));
    }
}